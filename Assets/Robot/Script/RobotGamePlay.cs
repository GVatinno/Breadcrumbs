using System;
using UnityEngine;

public class RobotGamePlay
{
	AIMotion mAImotion;
	Animator mAnimator;
	int mBumpParameter;

	public RobotGamePlay ( AIMotion AImotion, Animator animator)
	{
		mAImotion = AImotion;
		mAnimator = animator;
		mBumpParameter = Animator.StringToHash ("gameplay_bump");

		AnimationEventHandler animH = animator.GetComponent<AnimationEventHandler> ();
		animH.AddAnimationEvent ((int)EventType.ROBOT_GAMEPLAY_BUMP, 1.10f, "Bump" , OnBumpEnd);
	}

	public void OnTriggerEnter(Collider other) 
	{
		if (other.CompareTag ("obstacle"))
		{
			mAImotion.Stop ();
			mAnimator.SetTrigger (mBumpParameter);
		}
	}

	public void OnBumpEnd()
	{
		mAImotion.Stop ();
	}

}


