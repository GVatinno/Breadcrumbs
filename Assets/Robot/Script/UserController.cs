using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class UserController : MonoBehaviour {
		
	AIMotion mAIMotion = null;
	int layerMask = 0;
	Vector3 target;

	void Start () 
	{
		Animator animator = GetComponent<Animator> ();
		layerMask = 1 <<  LayerMask.NameToLayer("ground");
		mAIMotion = new AIMotion (new Locomotion (animator, 2.0f), new IKMotion (animator));
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
				Debug.Log (target);
				mAIMotion.SetTarget (target);
			}

		}
		mAIMotion.Update ();
	}

	void OnAnimatorIK()
	{
		mAIMotion.IKPass ();
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(target, 2.0f);
	}
}
