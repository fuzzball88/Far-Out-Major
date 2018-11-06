using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

	public EnemyPattern[] patterns;

	void Start() {
		for (int i = 0; i < patterns.Length; i++)
			patterns [i] = Instantiate (patterns [i]);
	}
}
