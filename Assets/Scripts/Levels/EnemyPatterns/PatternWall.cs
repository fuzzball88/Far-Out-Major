using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternWall : EnemyPattern {

	public enum Side { Left, Right };
	public Side side;

	public float distance;
	public int length;

	public override void startSpawnPattern (Transform ship) {
		base.startSpawnPattern (ship);
		for (int i = 0; i < length; i++) {
			GameObject g = (GameObject)Instantiate (enemy, (startDistance + distance)* Vector3.forward + ((side == Side.Left) ? ((-70 + 10*i)*Vector3.right) : (70 - 10*i)*Vector3.right), Quaternion.identity);
			g.GetComponent<Rigidbody>().angularVelocity = Random.Range (-10, 10) * Vector3.one; 
		}
	}
}
