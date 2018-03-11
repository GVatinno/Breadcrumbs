using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotion
{
	private Animator mAnimator = null;
	private Transform mTransform = null;
	private float mSqrtRadius = 0.0f;
	private float mSqrtRunRadius = 0.0f;
	private float mSqrtArrivalRadius = 0.0f;
	private bool mIsMoving = false;
	private float angleLeftRight = 0.0f;

	private int mWalkParameter;
	private int mTurnParameter;
	private int mAngleLeftRightParameter;
	private int mRunParameter;
	private int mSpeedParameter;


	private Vector3 lastTarget;
	private float lerpValue = 0.0f;

	public Locomotion(Animator animator, float arrivalRadius, float runRadius)
	{
		mAnimator = animator;
		mWalkParameter = Animator.StringToHash ("walk");
		mAngleLeftRightParameter = Animator.StringToHash ("angleLeftRight");
		mTurnParameter = Animator.StringToHash ("turn");
		mRunParameter = Animator.StringToHash ("run");
		mSqrtRadius = arrivalRadius * arrivalRadius;
		mSqrtRunRadius = runRadius * runRadius;
		Debug.Assert (mSqrtRunRadius > mSqrtRadius);
		mSqrtArrivalRadius = mSqrtRadius * 0.2f;
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
		mAnimator.SetBool (mRunParameter, false);

		Vector3 targetDirection = (lastTarget - mTransform.position).normalized;
		float angle = Vector3.Dot(targetDirection, mTransform.forward);
		float direction = Mathf.Sign(Vector3.Dot(Vector3.Cross (targetDirection, mTransform.forward).normalized, mTransform.up));
		angleLeftRight = angle * direction;

		if (angle >= 0.0f)
		{
			mAnimator.SetBool (mTurnParameter, false);
			// the target is in front of the agent
			float sqrtDistance = (mTransform.position - lastTarget).sqrMagnitude;
			if (sqrtDistance > mSqrtRadius) 
			{
				Move (sqrtDistance, angle, direction, sqrtDistance > mSqrtRunRadius );
			}
			else
			{
				if (sqrtDistance < mSqrtArrivalRadius ) 
				{
					mIsMoving = false;
				}
				else
				{
					Move (sqrtDistance, angle, direction, false);
				}
			}
		}
		else 
		{
			mAnimator.SetBool (mTurnParameter, true);
			mAnimator.SetFloat (mAngleLeftRightParameter, angleLeftRight);
		}
		lerpValue += 0.01f;

	}

	void Move( float sqrtDistance, float angle, float direction, bool run)
	{
		mIsMoving = true;
		mAnimator.SetBool (mWalkParameter, ! run);
		mAnimator.SetBool (mRunParameter, run);
		mTransform.rotation *= Quaternion.SlerpUnclamped(Quaternion.identity, Quaternion.AngleAxis (Mathf.Acos(angle)*direction, -Vector3.up), lerpValue);
	}

}
