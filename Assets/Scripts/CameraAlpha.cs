
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAlpha : MonoBehaviour
{
    [SerializeField]
    Material alpha;
    private Transform target = null;
	private Collider targetCollider = null;
    private GameObject last_obj;
    private Material last_material;
    private Renderer obj_render;
    private float alpha_value = 1.0f;

	private void Start()
	{
		target = GameObject.Find("ThirdPersonController").transform;
		targetCollider = target.GetComponent<Collider>();
	}

	public void Init(Transform follow)
    {
        target = follow;
    }

    private void Update()
    {
        if (target == null)
            return;
		Vector3 targetPos = target.position + new Vector3(0, targetCollider.bounds.size.y * 0.5f, 0);
        //为了调式时看的清楚画的线
        Debug.DrawLine(transform.position, targetPos, Color.red);
        RaycastHit hit;

        if (Physics.Linecast(transform.position, targetPos, out hit, 1 << LayerMask.NameToLayer("FarmBuilding")))
        {
			//判断
			if (hit.collider.gameObject.tag == "FarmBuilding" && last_obj == null)
			{
				last_obj = hit.collider.gameObject;
				//让遮挡物变半透明
				obj_render = last_obj.GetComponent<Renderer>();
				if (obj_render != null)
				{
					last_material = obj_render.material;
					obj_render.material = alpha;
					alpha_value = 1.0f;
				}
			}
		}//还原
        else if (last_obj != null)
        {
            if (obj_render != null)
            {
                obj_render.material = last_material;
            }
            last_obj = null;
            obj_render = null;
        }
    }
}
