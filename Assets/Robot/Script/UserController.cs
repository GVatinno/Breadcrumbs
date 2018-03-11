using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UserController : MonoBehaviour {
		
	Locomotion locomotion = null;
	IKMotion iKMotion = null;
	int layerMask = 0;
	Vector3 target;

	void Start () 
	{
		Animator animator = GetComponent<Animator> ();
		layerMask = 1 <<  LayerMask.NameToLayer("ground");
		locomotion = new Locomotion (animator, 2.0f, 5.0f);
		iKMotion = new IKMotion (animator);
	}
	

	void Update () 
	{
		if (Input.GetMouseButtonUp (0)) 
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit info;
			if (Physics.Raycast (ray, out info, 100.0f, layerMask)) 
			{
				target = info.point;
				locomotion.MoveTo (target);
				iKMotion.SetTarget (target);

			}

		}
		locomotion.Update ();
	}

	void OnAnimatorIK()
	{
		iKMotion.IKPass ();
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(target, 1.0f);
	}
}
