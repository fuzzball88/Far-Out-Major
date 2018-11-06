using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyShip : Ship {
	
	private ParticleSystem dead;
	private ParticleSystem ps;

	private ParticleSystem damageSmoke1;
	private ParticleSystem damageSmoke2;
	private ParticleSystem explosion;

	private ParticleSystem leftThrottle;
	private ParticleSystem rightThrottle;

	private GameObject plane0;
	private GameObject plane1;
	private GameObject plane2;
	bool isPunching = false;
	Vector3 punchFlag;
	private Animator anim;

	public override void initShip()
	{
		ParticleSystem[] pSystems = GetComponentsInChildren<ParticleSystem>();
	
		plane0 = GameObject.Find("Plane1");	
		plane1 = GameObject.Find("Plane2");	
		plane2 = GameObject.Find("Plane3");	
		foreach(ParticleSystem p in pSystems)
		{
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
			if (p.name == "smoke")
			{
				ps = p;
				ps.Stop();
			}
			if (p.name == "SmokeDead")
			{
				dead = p;
				dead.Stop();
			}
			if (p.name == "DamageSmoke1")
			{
				damageSmoke1 = p;
				damageSmoke1.Stop ();
			}
			if (p.name == "DamageSmoke2")
			{
				damageSmoke2 = p;
				damageSmoke2.Stop ();
			}
			if (p.name == "Explosion")
			{
				explosion = p;
				explosion.Stop ();
			}
		}
		anim = GetComponentInChildren<Animator>();
	}

	public override void Update()
	{
		ReadUserInput();
		if (!previewMode) {
			//Debug.Log ("Forward: " + movement.Forward);
			/* 
			 * Check if user is pressing some movement button.
			 */
			if (State == ShipState.Alive && movement.Left || State == ShipState.Stunned && movement.Left) {
				PlaySoundAndAnim ("MoveLeft");
				Move (Direction.Left);
			} else {
				if (!movement.LeftAlreadyStopped)
					PlaySoundAndAnim ("StopLeft");
			}
			if (State == ShipState.Alive && movement.Right || State == ShipState.Stunned && movement.Right) {
				PlaySoundAndAnim ("MoveRight");
				Move (Direction.Right);
			} else {
				if (!movement.RightAlreadyStopped)
					PlaySoundAndAnim ("StopRight");
			}
			if (State == ShipState.Alive && movement.Forward) {
				PlaySoundAndAnim ("startForward");
				Move (Direction.Up);
			} else {
				if (!movement.ForwardAlreadyStopped) {
					PlaySoundAndAnim ("stopForward");
				}
			}
			/* 
			 * Check if user is pressing special action button.
			 */
			if (State == ShipState.Alive && movement.special) {
				//PlaySoundAndAnim ("SpecialAction");
				Grid g = GameObject.FindObjectOfType<Grid>();

				plane0.SetActive(true);
				plane1.SetActive(true);
				plane2.SetActive(true);

				plane0.transform.position =g.GetGridPosition(transform.position+transform.forward*20);
				plane0.transform.position = plane0.transform.position + new Vector3(0,-12f,0);
				plane0.transform.rotation = Quaternion.identity;

				plane1.transform.position = g.GetGridPosition(transform.position+transform.forward*35);
				plane1.transform.position = plane1.transform.position + new Vector3(0,-12f,0);
				plane1.transform.rotation = Quaternion.identity;

				plane2.transform.position = g.GetGridPosition(transform.position+transform.forward*50);
				plane2.transform.position = plane2.transform.position + new Vector3(0,-12f,0);
				plane2.transform.rotation = Quaternion.identity;
				/*warpPlane.SetActive(true);	
				//warpPlane.transform.position = transform.position+transform.forward*50 + transform.position+warpPlane.transform.position+ Vector3.down*5;
				warpPlane.transform.position = g.GetGridPosition(transform.position+transform.forward*50);
				warpPlane.transform.position = warpPlane.transform.position + new Vector3(0,-12f,0);
				warpPlane.transform.rotation = Quaternion.identity;
				//warpPlane.transform.position = warpPlane.transform.position+ Vector3.down*5;
				ShipStateAnimator.SetBool ("PlayWarp", true);*/
				isPunching = true;
				//isWarping = true;
			} 
			else if (State == ShipState.Alive && isPunching) { // If warping was started, play end animations	

				Grid g = GameObject.FindObjectOfType<Grid>();
				//isPunching = false;
				//transform.position = g.GetGridPosition(transform.position+transform.forward*50);
				if (punchFlag != transform.localPosition) {
					transform.localPosition = Vector3.Lerp (transform.localPosition, g.GetGridPosition (transform.position + transform.forward * 50), Time.deltaTime * 30);
					punchFlag = g.GetGridPosition(transform.position+transform.forward*50);;
				} else {
					isPunching = false;
				}
				//transform.position += v*Time.deltaTime*20;
				//warpPlane.SetActive(false);	
				//ShipStateAnimator.SetBool ("PlayWarp", false);
			} 
		}
	}/*
	void doPunch()
	{
		Grid g = GameObject.FindObjectOfType<Grid>();
		shipBody.transform.position = g.GetGridPosition (transform.position+transform.forward*50);
		shipBody.velocity = Vector3.zero;
		//ShipStateAnimator.Play ("Ship_grow");
		//isWarping = false;
	}
**/
	public void PlaySoundAndAnim( string action )
	{
		/*
		 * All animations and sounds are played here. 
		 */
		//Debug.Log("Heavy ship anim: " + action );
		switch (action) {
		case "startForward":
			//Debug.Log ("Forward animation started(Heavy)");
			//GetComponentInChildren<Animation>()["Rotor|Normal"].speed = 6f;
			if(ps && !ps.isPlaying) { ps.Play(); }
			movement.ForwardAlreadyStopped = false;
			break;
		case "stopForward":
			//GetComponentInChildren<Animation>()["Rotor|Normal"].speed = 1f;
			//Debug.Log("Stopping heavy ship!" );
			if (ps && ps.isPlaying) { Debug.Log("Stopping heavy ship!" ); ps.Stop (); }
			movement.ForwardAlreadyStopped = true;
			break;
		case "MoveRight":
			transform.localEulerAngles = new Vector3 (0, transform.eulerAngles.y, -15f);
			movement.RightAlreadyStopped = false;
			break;
		case "StopRight":
			transform.localEulerAngles = new Vector3 (0, transform.eulerAngles.y, 0);
			movement.RightAlreadyStopped = true;
			break;
		case "MoveLeft":
			transform.localEulerAngles = new Vector3 (0, transform.eulerAngles.y, 15f);
			movement.LeftAlreadyStopped = false;

			break;
		case "StopLeft":
			transform.localEulerAngles = new Vector3 (0, transform.eulerAngles.y, 0);
			movement.LeftAlreadyStopped = true;
	
			break;
		case "SpecialAction":
			break;
		default:
			break;
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		if (state == ShipState.Alive) {
			shipBody.constraints &= ~RigidbodyConstraints.FreezeRotationY;
			shipBody.angularVelocity = Random.Range (-100, 100) * Vector3.one;
			//AnimateAction ("StopAll");
			armor -= 20;
			Debug.Log ("armor:" + armor);
			ReduceArmor (30);
			//ApplyDamage (30);

			if (armor < 80) {
				damageSmoke1.Play ();
			}
			if (armor < 50) {
				Debug.Log ("Start smoke!: " + armor);
				damageSmoke2.Play ();
			}


		}
		else if (state == ShipState.Dead) {
			explosion.Play ();
		}
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
