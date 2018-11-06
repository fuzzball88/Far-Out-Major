using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPattern : Enemy {

	public EnemyType enemyType;
//	public EnemySpawnType enemySpawnType;

	public float startDistance;
	public float endDistance;

	public bool isSpawned = false;

	protected GameObject enemy;
	protected Transform ship;

	void Start() {
		string enemyString;
		enemyToString.TryGetValue (enemyType, out enemyString);
		enemy = (GameObject)Resources.Load(enemyString);

		/*switch (enemySpawnType) {
			case EnemySpawnType:
			break;
		}*/
	}

	public virtual void startSpawnPattern(Transform s) {
		ship = s;
		isSpawned = true;
	}
	public virtual void stopSpawnPattern() {
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Gizmos.DrawLine(startDistance*Vector3.forward + 80*Vector3.left, startDistance*Vector3.forward + 80*Vector3.right);
		Gizmos.color = Color.red;
		Gizmos.DrawLine(endDistance*Vector3.forward + 80*Vector3.left, endDistance*Vector3.forward + 80*Vector3.right);
	}

}
