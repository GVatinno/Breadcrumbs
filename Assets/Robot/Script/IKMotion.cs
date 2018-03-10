using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKMotion
{
	private Animator mAnimator = null;
	private Vector3 mTarget;
	private float mWeight = 0.0f;

	public IKMotion(Animator animator)
	{
		mAnimator = animator;
	}
	
	public void SetTarget( Vector3 target )
	{
		mTarget = target;
		mWeight = 0.0f;
	}

	public void IKPass () 
	{
		mAnimator.SetLookAtPosition (mTarget);
		mAnimator.SetLookAtWeight (mWeight);
		mWeight += 0.01f;
	}
}
