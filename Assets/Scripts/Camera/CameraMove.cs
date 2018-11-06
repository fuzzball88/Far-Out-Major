using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

	public float followSpeed;
	public Vector3 offset;
	public Vector3 rotation;
	public bool useTransform = false;

	private Transform followTarget;
	private Vector3 followTargetVelocity;

	void Start () {
		followTarget = GameObject.FindWithTag ("Player").transform;
		followTargetVelocity = followTarget.GetComponent<Rigidbody>().velocity;
		if (!useTransform) {
			offset = transform.position - followTarget.position;
		} else {
			transform.rotation = Quaternion.Euler(rotation);
		}
	}

	void FixedUpdate() {
		Vector3 newPos = followTarget.position + offset;

		if (followTargetVelocity.z < 0)
			newPos.z += followTargetVelocity.z;
		
		transform.position = Vector3.Lerp(transform.position, newPos, followSpeed);
	}
}
