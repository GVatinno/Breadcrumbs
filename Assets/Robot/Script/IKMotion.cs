using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKMotion
{
	private Animator mAnimator = null;
	private Vector3 mTarget;
	private Vector3 mPrevTarget;
	private float mWeight = 0.0f;

	public IKMotion(Animator animator)
	{
		mAnimator = animator;
		mPrevTarget = Vector3.zero;
		mTarget = Vector3.zero;
	}
	
	public void SetTarget( Vector3 target )
	{
		mPrevTarget = mTarget;
		mTarget = target;
		mWeight = 0.0f;
	}

	public void IKPass () 
	{
		mAnimator.SetLookAtPosition(Vector3.Lerp(mPrevTarget, mTarget, mWeight));
		mAnimator.SetLookAtWeight (1.0f);
		mWeight += 0.5f * Time.deltaTime;
	}
}
