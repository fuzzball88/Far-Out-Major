using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthRotation : MonoBehaviour {

	public float speed;

	void Update () {
        //transform.RotateAround(Vector3.zero, Vector3.up, 30 * Time.deltaTime);
		transform.Rotate(Vector3.forward, speed * Time.deltaTime);
    }
}
