using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotion
{
	private Animator mAnimator = null;
	private Transform mTransform = null;
	private float sqrtRadius = 0.0f;
	private bool mIsMoving = false;
	private float angleLeftRight = 0.0f;

	private int mWalkParameter;
	private int mTurnParameter;
	private int mAngleLeftRightParameter;


	private Vector3 lastTarget;
	private Quaternion lastTargetRotation;
	private float lerpValue = 0.0f;

	public Locomotion(Animator animator, float arrivalRadius)
	{
		mAnimator = animator;
		mWalkParameter = Animator.StringToHash ("walk");
		mAngleLeftRightParameter = Animator.StringToHash ("angleLeftRight");
		mTurnParameter = Animator.StringToHash ("turn");
		sqrtRadius = arrivalRadius * arrivalRadius;
		mTransform = animator.GetComponent<Transform> ();
		mIsMoving = false;
		lerpValue = 0.0f;

		// fix arrival
	}

	public void MoveTo(Vector3 position)
	{
		lastTarget = position;
		lerpValue = 0.0f;
	}

	public void Update()
	{
		mAnimator.SetBool (mWalkParameter, false);
		mAnimator.SetBool (mTurnParameter, false);

		Vector3 targetDirection = (lastTarget - mTransform.position).normalized;
		lastTargetRotation = Quaternion.FromToRotation (mTransform.forward, targetDirection);
		float angle = Vector3.Dot(targetDirection, mTransform.forward);
		float direction = Mathf.Sign(Vector3.Dot(Vector3.Cross (targetDirection, mTransform.forward).normalized, mTransform.up));
		angleLeftRight = angle * direction;

		if (angle >= 0.0f)
		{
			mAnimator.SetBool (mTurnParameter, false);
			// the target is in front of the agent
			if ((mTransform.position - lastTarget).sqrMagnitude > sqrtRadius) 
			{
				mIsMoving = true;
				mAnimator.SetBool (mWalkParameter, true);
				mTransform.rotation *= Quaternion.SlerpUnclamped(Quaternion.identity, Quaternion.AngleAxis (Mathf.Acos(angle)*direction, -Vector3.up), lerpValue);
				Debug.Log (angleLeftRight);
			}
			else
			{
				mIsMoving = false;
				mAnimator.SetBool (mWalkParameter, false);
			}
		}
		else 
		{
			
			Debug.Log (direction);
			mAnimator.SetBool (mTurnParameter, true);
			mAnimator.SetFloat (mAngleLeftRightParameter, angleLeftRight);
		}
		lerpValue += 0.01f;

	}

}
