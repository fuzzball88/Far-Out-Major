using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour {

	public GameController controller;

	private Ship ship;
	public GameObject menu;

	// Use this for initialization
	void Start () {

		controller = FindObjectOfType<GameController>();
		Ship[] ships = FindObjectsOfType<Ship>();
		menu = GameObject.Find("Menu");
		menu.SetActive (false);

		switch (controller.selected) {
		case GameController.ShipType.WarpShip:
			ship = FindObjectOfType<WarpShip>();
			FindObjectOfType<HeavyShip>().gameObject.SetActive(false);
			FindObjectOfType<ClassicShip>().gameObject.SetActive(false);
			break;
		case GameController.ShipType.ClassicShip:
			ship = FindObjectOfType<ClassicShip>();
			FindObjectOfType<HeavyShip>().gameObject.SetActive(false);
			FindObjectOfType<WarpShip>().gameObject.SetActive(false);
			break;
		case GameController.ShipType.HeavyShip:
			ship = FindObjectOfType<HeavyShip>();
			FindObjectOfType<ClassicShip>().gameObject.SetActive(false);
			FindObjectOfType<WarpShip>().gameObject.SetActive(false);
			break;
		default:
			break;
		}

		/*
		if (controller) {
			foreach (Ship s in ships) {
				if (s.name == "WarpShip" && controller.selectedShip == GameController.ShipType.WarpShip) {
					ship = s;
				}
				if (s.name == "ClassicShip"&& controller.selectedShip == GameController.ShipType.ClassicShip) {
					ship = s;

				}
				if (s.name == "HeavyShip"&& controller.selectedShip == GameController.ShipType.HeavyShip) {
					ship = s;
				}
			}
		}*/

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			menu.SetActive (true);
			Time.timeScale = 0.0f;
		}
	}

	public void OnReplayClicked ()
	{ 
		if (controller) {
			controller.StartGame ();
		} else { Debug.Log("No gamecontroller found, maybe scene was not started from main menu?"); };
	}

	public void OnToMenuClicked()
	{
		if(controller) {
			controller.loadMenu ();
		} else { Debug.Log("No gamecontroller found, maybe scene was not started from main menu?"); };
	}
		
	public void TurnLeftDownMobile()
	{
		ship.movement.Left = true;
	}

	public void TurnLeftUpMobile()
	{
		ship.movement.Left = false;
	}

	public void GoForwardDownMobile()
	{
		ship.movement.Forward = true;
	}

	public void GoForwardUpMobile()
	{
		ship.movement.Forward = false;
	}

	public void TurnRightDownMobile()
	{
		ship.movement.Right = true;
	}

	public void TurnRightUpMobile()
	{
		ship.movement.Right = false;
	}

	public void ResumeGamePressed()
	{
		menu.SetActive (false);
		Time.timeScale = 1.0f;
	}

}
