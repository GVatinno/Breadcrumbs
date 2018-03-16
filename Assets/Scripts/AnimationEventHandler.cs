using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventHandler : MonoBehaviour
{
	public Dictionary<int, List<Action>> eventIDCallbackMap;

	void Start()
	{
		eventIDCallbackMap = new Dictionary<int, List<Action>> ();
	}

	public void AddAnimationEvent( Animator anim, int eventId, float time, string state, Action callback)
	{
		AnimationClip c = FindAnimationClip (anim, state);
		Debug.Assert (c != null, "Cannot find animation clip in Animator");
		AnimationEvent e = new AnimationEvent ();
		e.intParameter = eventId;
		e.functionName = "OnAnimationEvent";
		e.time = time;
		c.AddEvent (e);
		if (eventIDCallbackMap.ContainsKey (eventId))
		{
			eventIDCallbackMap [eventId].Add (callback);
		} 
		else
		{
			eventIDCallbackMap [eventId] = new List<Action> { callback };
		}
	}

	AnimationClip FindAnimationClip( Animator anim, string state )
	{
		foreach (AnimationClip c in anim.runtimeAnimatorController.animationClips)
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
		if (eventIDCallbackMap.ContainsKey (eventId))
		{
			foreach (var action in eventIDCallbackMap[eventId])
			{
				action ();
			}
		}
	}
}


