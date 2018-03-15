using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMotion
{ 
	private Locomotion mLocomotion;
	private IKMotion mIKMotion;
	private Transform mTransform;
	private NavMeshPath mLastPath;
	private int mCurrentWayPointIndex;

	public AIMotion(Locomotion locomotion, IKMotion IK)
	{
		mLocomotion = locomotion;
		mIKMotion = IK;
		mTransform = locomotion.GetTransform ();
	}

	public void SetTarget(Vector3 target)
	{
		mCurrentWayPointIndex = 0;
		mLastPath = new NavMeshPath ();
		if (NavMesh.CalculatePath (mTransform.position, target, NavMesh.AllAreas, mLastPath)) 
		{
			if (mLastPath.corners.Length > 0 ) 
			{
				SetTargetWayPoint (mCurrentWayPointIndex);
			}

		} else
		{
			mLastPath = null;
		}
	}

	void SetTargetWayPoint(int index)
	{
		Vector3 wapoint = mLastPath.corners [index];
		wapoint.y = 0.0f; // TODO change when using slope
		mLocomotion.MoveTo (wapoint);
		mIKMotion.SetTarget (wapoint);
	}

	bool SetNextWayPointIndex()
	{
		if (mLastPath != null)
		{
			if (mCurrentWayPointIndex + 1 < mLastPath.corners.Length)
			{
				++mCurrentWayPointIndex;
				return true;
			}
		}
		return false;
	}

	public void Update()
	{
		mLocomotion.Update ();
		if ( mLocomotion.IsInSlowDownRadius () )
		{ 
			if (mLastPath != null)
			{
				//Debug.Log (mLastPath.status);
			}
				

			if (SetNextWayPointIndex ())
			{
				SetTargetWayPoint (mCurrentWayPointIndex);
			}
		}

	}

	public void IKPass()
	{
		mIKMotion.IKPass ();
	}
}
