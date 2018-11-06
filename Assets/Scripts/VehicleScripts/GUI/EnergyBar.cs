using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBar : MonoBehaviour {

	public RectTransform bar;

	public void setLength(float percentage) {
		bar.localScale = new Vector3 (percentage, 1f, 1f);
	}
}
