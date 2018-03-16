using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationEventHandler : MonoBehaviour
{
	public Dictionary<int, List<Action>> mEventIDCallbackMap;

	Animator mAnim;

	void Start()
	{
		mEventIDCallbackMap = new Dictionary<int, List<Action>> ();
		mAnim = GetComponent<Animator> ();
	}

	public void AddAnimationEvent( int eventId, float time, string state, Action callback)
	{
		AnimationClip c = FindAnimationClip (state);
		Debug.Assert (c != null, "Cannot find animation clip in Animator");
		AnimationEvent e = new AnimationEvent ();
		e.intParameter = eventId;
		e.functionName = "OnAnimationEvent";
		e.time = time;
		c.AddEvent (e);
		if (mEventIDCallbackMap.ContainsKey (eventId))
		{
			mEventIDCallbackMap [eventId].Add (callback);
		} 
		else
		{
			mEventIDCallbackMap [eventId] = new List<Action> { callback };
		}
	}

	AnimationClip FindAnimationClip( string state )
	{
		foreach (AnimationClip c in mAnim.runtimeAnimatorController.animationClips)
		{
			Debug.Log (c.name);
			if (c.name == state)
			{	
				return c;
			}
		}
		return null;
	}

	void OnAnimationEvent( int eventId )
	{
		if (mEventIDCallbackMap.ContainsKey (eventId))
		{
			foreach (var action in mEventIDCallbackMap[eventId])
			{
				action ();
			}
		}
	}
}


