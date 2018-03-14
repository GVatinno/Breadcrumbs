using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotion
{
	private Animator mAnimator = null;
	private Transform mTransform = null;
	private float mSqrtRadius = 0.0f;
	private float mSqrtArrivalRadius = 0.0f;

	private int mAngleLeftFRightParameter;
	private int mAngleLeftRightParameter;
	private int mSpeedFParameter;
	private int mSpeedParameter;

	private Vector3 mLastTarget;
	private float mLerpValue = 0.0f;
	private float mSpeed = 0.0f;
	private float mAcceleration = 0.5f;

	public Locomotion(Animator animator, float arrivalRadius)
	{
		mAnimator = animator;
		mAngleLeftFRightParameter = Animator.StringToHash ("angleLeftRightf");
		mAngleLeftRightParameter = Animator.StringToHash ("angleLeftRight");
		mSpeedFParameter = Animator.StringToHash ("speedf");
		mSpeedParameter = Animator.StringToHash ("speed");
		mSqrtRadius = arrivalRadius * arrivalRadius;

		mSqrtArrivalRadius = mSqrtRadius * 0.2f;
		mTransform = animator.GetComponent<Transform> ();
		mLerpValue = 0.0f;
	}

	public void MoveTo(Vector3 position)
	{
		mLastTarget = position;
		mLerpValue = 0.0f;
	}

	public void Update()
	{
		SetAnimatorSpeed (0.0f);
		SetAnimatorAngles (0.0f);

		Vector3 targetDirection = (mLastTarget - mTransform.position).normalized;
		float angle = Vector3.Dot(targetDirection, mTransform.forward);
		float direction = Mathf.Sign(Vector3.Dot(Vector3.Cross (targetDirection, mTransform.forward).normalized, mTransform.up));
		float angleLeftRight = angle * direction;

		if (angle >= 0.0f)
		{
			float sqrtDistance = (mTransform.position - mLastTarget).sqrMagnitude;
			if (sqrtDistance > mSqrtRadius) 
			{
				
				mSpeed = Mathf.Clamp(mSpeed + mAcceleration * Time.deltaTime, 0.0f, 1.0f);
				SetAnimatorSpeed (mSpeed);
				RotateWhileMoving (targetDirection);
			}
			else
			{
				if (sqrtDistance < mSqrtArrivalRadius) 
				{
					mSpeed = 0.0f;
					SetAnimatorSpeed (mSpeed);
				} 
				else 
				{
					mSpeed = Mathf.Clamp ((sqrtDistance / mSqrtRadius), 0.0f, 1.0f);
					SetAnimatorSpeed (mSpeed);
					RotateWhileMoving (targetDirection);
				}

			}

		}
		else 
		{
			if (mSpeed > 0.0f) 
			{
				mSpeed = Mathf.Clamp (mSpeed - mAcceleration * 2.0f * Time.deltaTime, 0.0f, 1.0f);
				SetAnimatorSpeed (mSpeed);
			} 
			else 
			{
				SetAnimatorAngles (angleLeftRight);
			}
		}
		mLerpValue += 0.5f * Time.deltaTime;

	}

	void RotateWhileMoving(Vector3 targetDirection)
	{
		mTransform.rotation = Quaternion.RotateTowards(mTransform.rotation, Quaternion.LookRotation(targetDirection, Vector3.up), mLerpValue);
	}

	void SetAnimatorSpeed( float speed)
	{
		mAnimator.SetFloat (mSpeedFParameter, speed);
		mAnimator.SetInteger (mSpeedParameter, (int)(speed * 1000.0f));
	}

	void SetAnimatorAngles( float angles)
	{
		mAnimator.SetFloat (mAngleLeftFRightParameter, angles);
		mAnimator.SetInteger (mAngleLeftRightParameter, (int)(angles * 1000.0f));
	}

}
