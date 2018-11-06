using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public enum ShipType { WarpShip, ClassicShip, HeavyShip };
	public enum ViewType { Menu, Level1 };

	public ShipType selected;
	static GameController instance = null;
	public int selectedShip = 1;

	int SelectedLevel = 1;
	int position = 0;

	Animator menuAnim;
	Animator guiAnim;
	Animator camAnim;

	public AudioSource fxSource1;
	public AudioClip shipChangeAudio;

    private bool restart;
    public enum GameState { TitleScreen, 
							PreLaunch, 
							Playing, 
							Finish,
						  };
    void Awake()
    {
		if (instance == null) {
			this.transform.SetParent (null);
			instance = this;
			DontDestroyOnLoad (gameObject);
		
			GameObject.FindObjectOfType<ShipSpinner> ().controller = this;
			Animator[] anims = FindObjectsOfType<Animator> ();
			foreach (Animator a in anims) {
				Debug.Log ("Anim: " + a.name);
				if (a.name == "GUI") {
					guiAnim = a;
				} else if (a.name == "ShipSpinner") {
					menuAnim = a;
				} else if (a.name == "Main Camera") {
					camAnim = a;
				}
			}		

			switch (selectedShip) {
			case 0:
				selected = ShipType.ClassicShip;
				break;
			case 1:
				selected = ShipType.WarpShip;
				break;
			case 2:
				selected = ShipType.HeavyShip;
				break;
			}

			SetShipDescription (selected);
		} else {
			GameObject.FindObjectOfType<ShipSpinner> ().controller = instance;
			//Destroy (this);
		}
    }


    void Start () {
        restart = false;
	}
	
	void Update() {
		if (Input.GetAxis("Cancel") != 0)
			Application.Quit ();
		
        if (restart)
        {
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				//SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			}
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
		
	public void LoadView( ViewType view )
	{
		switch (view) {
		case ViewType.Menu:
			SceneManager.LoadScene(1);
			break;
		case ViewType.Level1:
			SceneManager.LoadScene(0);
			break;
		default:
			break;
		}
	}

	public void QuitButtonIsPresssed()
	{
		Debug.Log ("Quit pressed");
		Application.Quit();
		//UnityEditor.EditorApplication.isPlaying = false; // Uncomment for testing in editor, comment out for build.
	}

	public void QuitGameAction()
	{
		Application.Quit();
	}


	public void StartTheGame()
	{
		guiAnim.SetTrigger("GUI_fadeout");
		menuAnim.SetTrigger("OnShipSelected");
		camAnim.SetTrigger("ShipSelected");
	}


	public void loadMenu()
	{
		SceneManager.LoadScene(1);
	}

	public void StartGame()
	{
		SceneManager.LoadScene(0);
	}


	public void LeftButtonClicked()
	{
		// Debug.Log ("left clicked");
		fxSource1.PlayOneShot(shipChangeAudio);
		menuAnim.SetTrigger ("OnRightPressed");

		if (selectedShip > 0) {
			selectedShip--;
		}
		else if (selectedShip-1 < 0) {
			selectedShip = 2;
		}
		switch (selectedShip) {
		case 0:
			selected = ShipType.ClassicShip;
			break;
		case 1:
			selected = ShipType.WarpShip;
			break;
		case 2:
			selected = ShipType.HeavyShip;
			break;
		}
		SetShipDescription(selected);
		//Debug.Log ("index: " + selectedShip);
	}

	public void RightButtonClicked()
	{
		// Debug.Log ("right clicked");

		fxSource1.PlayOneShot(shipChangeAudio);
		menuAnim.SetTrigger ("OnLeftPressed");

		if (selectedShip < 2) {
			selectedShip++;
		}else if (selectedShip+1 > 2) {		
			selectedShip = 0;
		}			
		switch (selectedShip) {
		case 0:
			selected = ShipType.ClassicShip;
			break;
		case 1:
			selected = ShipType.WarpShip;
			break;
		case 2:
			selected = ShipType.HeavyShip;
			break;
		}
		SetShipDescription(selected);
		//Debug.Log ("index: " + selectedShip);
	}

	void SetShipDescription( ShipType ship )
	{
		Text[] textFields = GameObject.FindObjectsOfType<Text> ();
		Text name = null;
		Text descr = null;

		foreach (Text t in textFields) {
			if (t.name == "ShipDescription") {
				descr = t;
			} else if (t.name == "ShipName") {
				name = t;
			}
			Debug.Log (t.name);
		}

		switch (ship) {
		case ShipType.ClassicShip:
			name.text = "Classic ship";
			descr.text = "Developed mainly to catch asteroids and to move in thick asteroid fields, this ship is especially agile.";
			break;
		case ShipType.HeavyShip:
			name.text = "Heavy ship";
			descr.text = "Originally developed to haul meteorites, this ship has extra reinforcements in its hull.";
			break;
		case ShipType.WarpShip:
			name.text = "Warp ship";
			descr.text = "Used to light transportation over long distances, this ship is very fast and uses warp engine to quickly move from one place to another. However it lacks in maneurability.";
			break;
		}
	}
		
}
