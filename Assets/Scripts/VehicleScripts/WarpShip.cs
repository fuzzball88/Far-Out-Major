using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpShip : Ship {

	private Light orbGlow;
	private ParticleSystem deathAnim;

	private ParticleSystem thrustAnim1;
	private ParticleSystem thrustAnim2;

	private ParticleSystem explosion;

	private ParticleSystem leftThrottle;
	private ParticleSystem rightThrottle;

	private Animator ShipStateAnimator;
	private Animator ShipPartAnimator;

	private GameObject warpPlane;

	private AudioSource shipAudio;
	public AudioClip thrust;
	public AudioClip shutDown;

	private bool isWarping = false;

	public override void initShip()
	{
		shipAudio = GetComponent<AudioSource> ();
		ParticleSystem[] pSystems = GetComponentsInChildren<ParticleSystem>();
		GetComponentInChildren<Animation>().Play();
		MeshFilter[] m = gameObject.GetComponentsInChildren<MeshFilter>();
		warpPlane = GameObject.Find("WarpEffect");

		if (warpPlane) {
			warpPlane.SetActive (false);	
		}

		foreach(ParticleSystem p in pSystems)
		{
			if (p.name == "DamageLevel1")
			{
				damageLevel1 = p;
				damageLevel1.Stop();
			}
			if (p.name == "DamageLevel2")
			{
				damageLevel2 = p;
				damageLevel2.Stop();
			}
			if (p.name == "DamageLevel3")
			{
				damageLevel3 = p;
				damageLevel3.Stop();
			}
			if (p.name == "OrbTail1")
			{
				thrustAnim1 = p;
				thrustAnim1.Stop();
			}
			if (p.name == "OrbTail2")
			{
				thrustAnim2 = p;
				thrustAnim2.Stop();
			}
			if (p.name == "LeftThrottle")
			{
				leftThrottle = p;
				leftThrottle.Stop();
			}
			if (p.name == "RightThrottle")
			{
				rightThrottle = p;
				rightThrottle.Stop();
			}
			if (p.name == "SmokeDead")
			{
				deathAnim = p;
				deathAnim.Stop();
			}

			if (p.name == "Explosion")
			{
				explosion = p;
				explosion.Stop ();
			}
		}
		ShipStateAnimator = GetComponentInChildren<Animator>();
		orbGlow = GetComponentInChildren<Light>();
	}

	public override void Update()
	{
		ReadUserInput();
		if (!previewMode) {
			/* 
			 * Check if user is pressing some movement button.
			 */
			if (State == ShipState.Alive && movement.Left || State == ShipState.Stunned && movement.Left) {
				PlaySoundAndAnim ("MoveLeft");
				Move (Direction.Left);
			} else {
				if(!movement.LeftAlreadyStopped)
					PlaySoundAndAnim ("StopLeft");
			}
			if (State == ShipState.Alive && movement.Right || State == ShipState.Stunned && movement.Right) {
				PlaySoundAndAnim ("MoveRight");
				Move(Direction.Right);
			} else {
				if(!movement.RightAlreadyStopped)
					PlaySoundAndAnim ("StopRight");
			}
			if (State == ShipState.Alive && movement.Forward) {
				PlaySoundAndAnim ("startForward");
				Move(Direction.Up);
			} else {
				if(!movement.ForwardAlreadyStopped)
					PlaySoundAndAnim ("stopForward");
			}
			/* 
			 * Check if user is pressing special action button.
			 */
			if (State == ShipState.Alive && movement.special) {
				//PlaySoundAndAnim ("SpecialAction");
				Grid g = GameObject.FindObjectOfType<Grid>();
				warpPlane.SetActive(true);	
				//warpPlane.transform.position = transform.position+transform.forward*50 + transform.position+warpPlane.transform.position+ Vector3.down*5;
				warpPlane.transform.position = g.GetGridPosition(transform.position+transform.forward*50);
				warpPlane.transform.position = warpPlane.transform.position + new Vector3(0,-12f,0);
				warpPlane.transform.rotation = Quaternion.identity;
				ShipStateAnimator.SetBool ("PlayWarp", true);
				isWarping = true;
			} 
			else if(State == ShipState.Alive && isWarping) { // If warping was started, play end animations	
				warpPlane.SetActive(false);	
				ShipStateAnimator.SetBool ("PlayWarp", false);
			} 
		}
	}

	public void PlaySoundAndAnim( string action )
	{
		/*
		 * All animations and sounds are played here. 
		 */
		switch (action) {
		case "startForward":
			shipAudio.Play ();
			shipAudio.PlayOneShot (thrust);

			GetComponentInChildren<Animation>()["Rotor|Normal"].speed = 6f;
			thrustAnim1.Play();
			thrustAnim2.Play();
			if (orbGlow) {
				orbGlow.range = 10;
			}
			if (!previewMode) { movement.ForwardAlreadyStopped = false; }
			if (ShipStateAnimator) {
				ShipStateAnimator.SetTrigger("MoveForward");
			}
			break;
		case "stopForward":
			shipAudio.Stop ();
			shipAudio.PlayOneShot (shutDown);
			GetComponentInChildren<Animation>()["Rotor|Normal"].speed = 1f;
			thrustAnim1.Stop();
			thrustAnim2.Stop();
			if (orbGlow) {
				orbGlow.range = 5;
			}
			movement.ForwardAlreadyStopped = true;
			if (ShipStateAnimator) {
				ShipStateAnimator.SetTrigger("MoveStopped");
			}
			break;
		case "MoveRight":
			transform.localEulerAngles = new Vector3 (0, transform.eulerAngles.y, -15f);
			movement.RightAlreadyStopped = false;
			if (rightThrottle && !rightThrottle.isPlaying) {
				rightThrottle.gameObject.SetActive (true);
				Debug.Log ("Playing thrust!");
				rightThrottle.Play();
			}
			break;
		case "StopRight":
			transform.localEulerAngles = new Vector3 (0, transform.eulerAngles.y, 0);
			movement.RightAlreadyStopped = true;
			if (rightThrottle && rightThrottle.isPlaying) {
				rightThrottle.gameObject.SetActive(false);
				rightThrottle.Stop();
			}
			break;
		case "MoveLeft":
			transform.localEulerAngles = new Vector3 (0, transform.eulerAngles.y, 15f);
			movement.LeftAlreadyStopped = false;
			if (leftThrottle && !leftThrottle.isPlaying) {
				leftThrottle.gameObject.SetActive(true);
				leftThrottle.Play();
			}
			break;
		case "StopLeft":
			transform.localEulerAngles = new Vector3 (0, transform.eulerAngles.y, 0);
			movement.LeftAlreadyStopped = true;
			if (leftThrottle && leftThrottle.isPlaying) {
				leftThrottle.gameObject.SetActive(false);
				leftThrottle.Stop();
			}
			break;
		case "SpecialAction":
			Debug.Log ("Warping started");
			ShipStateAnimator.SetTrigger ("WarpStarted");
			break;
		default:
			break;
		}
	}
		
	void OnCollisionEnter(Collision collision)
	{
		if (state == ShipState.Alive) {
			warpPlane.SetActive(false);	
			isWarping = false;
			shipBody.constraints &= ~RigidbodyConstraints.FreezeRotationY;
			shipBody.angularVelocity = Random.Range (-100, 100) * Vector3.one;

			armor -= 20;
			Debug.Log ("armor:" + armor);
			ReduceArmor (30);
			if (armor < 80) {
				damageLevel1.Play();
			}
			if (armor < 50) {
				Debug.Log ("Start smoke!: " + armor);
				damageLevel2.Play();
			}
			if (armor < 30) {
				damageLevel3.Play();
			}
		}
		else if (state == ShipState.Dead) {
			explosion.Play ();
		}
	}
		
	void doWarp()
	{
		Grid g = GameObject.FindObjectOfType<Grid>();

		shipBody.transform.position = g.GetGridPosition (transform.position+transform.forward*50);
		shipBody.velocity = Vector3.zero;
		ShipStateAnimator.Play ("Ship_grow");
		isWarping = false;
	}

	protected override void resetShip()
	{
		shipBody.constraints |= RigidbodyConstraints.FreezeRotationY;
		shipBody.velocity = Vector3.zero;
		shipBody.angularVelocity = Vector3.zero;
		transform.rotation = Quaternion.identity;
		transform.position = Vector3.Scale(Vector3.forward, transform.position);
		state = ShipState.Alive;
	}
		
}
