using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleRandom : EnemyPattern {

	public float spawnInterval;

	public override void startSpawnPattern (Transform ship) {
		base.startSpawnPattern (ship);
		InvokeRepeating ("spawn", 0, spawnInterval);
		Debug.Log ("start!");
	}

	void spawn() {
		Vector3 shipSpeed = ship.GetComponent<Rigidbody> ().velocity;
		GameObject g = (GameObject)Instantiate (enemy, ship.position + 500 * Vector3.forward + 20*Random.Range(-2.0f, 2.0f)*Vector3.right, Quaternion.identity);
		Vector3 asteroidToShip = ship.position + shipSpeed - g.transform.position;
		g.GetComponent<Rigidbody>().velocity = 0.125f*Vector3.Magnitude (asteroidToShip)*Vector3.Normalize(asteroidToShip); 
		g.GetComponent<Rigidbody>().angularVelocity = Random.Range (-10, 10) * Vector3.one; 
		Destroy (g, 10);
	}

	public override void stopSpawnPattern () {
		CancelInvoke("spawn");
		Debug.Log ("stop!");
	}	

}
