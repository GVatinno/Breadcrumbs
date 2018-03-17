using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour 
{
	[SerializeField] 
	GameObject target = null;
	[SerializeField] 
	float damping = 1;
	Vector3 offset;
	Quaternion orientation;

	void Start() {
		offset = target.transform.position - transform.position;
		orientation = transform.rotation;
	}

	void LateUpdate() {
		float currentAngle = transform.eulerAngles.y;
		float desiredAngle = target.transform.eulerAngles.y;
		float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * damping);

		Quaternion rotation = Quaternion.Euler(0, angle, 0);
		transform.position = target.transform.position - (rotation * offset);
		transform.rotation = Quaternion.Euler (new Vector3 (transform.eulerAngles.x, angle, transform.eulerAngles.z));

	}
}
