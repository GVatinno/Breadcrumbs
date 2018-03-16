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
		Vector3 position = mTransform.position; 
		if (mLocomotion.IsMoving () && !mLocomotion.IsInSlowDownRadius())
		{
			position += mTransform.forward * mLocomotion.GetSqrtSlowingDownRadius (); 
		}

		if (NavMesh.CalculatePath (position, target, NavMesh.AllAreas, mLastPath)) 
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

	public void Stop()
	{
		mLastPath = null;
		mLocomotion.Stop ();
		mIKMotion.SetTarget (mTransform.position);

	}

	public void DrawGizmo()
	{
		if (mLastPath != null)
		{
			for (uint i = 0; i < mLastPath.corners.Length; ++i)
			{
				if (i == mCurrentWayPointIndex)
				{
					Gizmos.color = Color.yellow;
				}
				Gizmos.DrawWireCube (mLastPath.corners[i], new Vector3 (0.2f, 0.2f, 0.2f));
				Gizmos.color = Color.red;
			}
		}		
	}
}
