using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningSquare : MonoBehaviour {

	public float blinkDelay;
	float startTime;
	bool isRed = true;
	SpriteRenderer sp;

	void Start () {
		sp = GetComponent<SpriteRenderer> ();
		startTime = Time.time;
	}
	
	void Update () {
		if (Time.time - startTime > blinkDelay) {
			startTime = Time.time;
			isRed = !isRed;
			if (isRed)
				sp.color = Color.red;
			else
				sp.color = new Color(0, 0, 0, 0);	
		}
	}
}
