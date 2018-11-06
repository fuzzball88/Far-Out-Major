using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpinner : MonoBehaviour {

	//public StartMenuScript menu;
	public GameController controller;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void starGame()
	{
		controller.StartGame ();
	}
		
	public void StartWarpShip()
	{
		GameObject ob = GameObject.FindGameObjectWithTag ("WarpShip");
		GameObject ob1 = GameObject.FindGameObjectWithTag ("ClassicShip");
		GameObject ob2 = GameObject.FindGameObjectWithTag ("HeavyShip");
		/*
		Debug.Log ("warp: " + ob.name);
		Debug.Log ("classic: " + ob1.name);
		Debug.Log ("haevy: " + ob2.name);
*/
		ob.GetComponent<WarpShip>().PlaySoundAndAnim ("startForward");
		ob1.GetComponent<ClassicShip>().PlaySoundAndAnim ("startForward");
		ob2.GetComponent<HeavyShip>().PlaySoundAndAnim ("startForward");
		//ob.GetComponent<ParticleSystem>().Play ();
	}

	public void StopShipEngines() {
		Debug.Log ("Stopping ship engines");
		GameObject ob = GameObject.FindGameObjectWithTag ("WarpShip");
		ob.GetComponent<WarpShip>().PlaySoundAndAnim ("stopForward");
		GameObject ob1 = GameObject.FindGameObjectWithTag ("ClassicShip");
		//ClassicShip cs = ob1.GetComponent<ClassicShip> ();
		ob1.GetComponent<ClassicShip>().PlaySoundAndAnim ("stopForward");
		GameObject ob2 = GameObject.FindGameObjectWithTag ("HeavyShip");
		ob2.GetComponent<HeavyShip>().PlaySoundAndAnim ("stopForward");
	}

	public void onLightPostCollision() {
		//gameObject.Find ("PostExplosion");
		GameObject ob = GameObject.FindGameObjectWithTag("LightPost");
		GameObject ex = GameObject.FindGameObjectWithTag("Explosion");
		ex.GetComponent<ParticleSystem> ().Play ();
		ob.SetActive (false);
	}


}
