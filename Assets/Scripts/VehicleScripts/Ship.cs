using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ShipMoving {

	public bool Left;
	public bool Right;
	public bool Forward;

	public bool LeftAlreadyStopped;
	public bool RightAlreadyStopped;
	public bool ForwardAlreadyStopped;

	public bool special;

	public ShipMoving( bool l, bool r, bool f ) 
	{
		Left	= l;
		Right 	= r;
		Forward = f;

		LeftAlreadyStopped = false;
		RightAlreadyStopped = false;
		ForwardAlreadyStopped = false;

		special = false;
	}
}

public class Ship : MonoBehaviour {

	public enum ShipAnim { ForwardStart, ForwardStop, LeftStart, LeftStop, RightStart, RightStop, SpecialAction };
	public ShipMoving movement = new ShipMoving(false, false, false);

	public enum ShipState { Alive, Stunned, Dead }; 			
	public enum ControlType { Shooter, Asteroids }; 	// Available control types.
	public enum Direction { Left, Right, Up, Down }; 	// Available movement directions.

	public ControlType controlType = ControlType.Shooter; // Set default controls.
	public bool previewMode = false;

	protected ShipState state = ShipState.Alive;

	public ShipState State { get { return state; } set { state = value; } }

	public float energy = 100;
	public int armor = 100;
	public float fuelBurnRate = 1;
	protected float currentEnergy;
	protected EnergyBar energyBar;

	public Vector3 acceleration;

	public float maxSpeed;
	public float turnSpeed;
	public float forcedForwardSpeed;

	public Vector3 tiltAngles;
	public float tiltSpeed;

	protected Rigidbody shipBody;

	/*
	 * Each ship must have at least 3 "basic" particle damage animation,
	 * that represent different levels of damage to the ship.
	 */
	protected ParticleSystem damageLevel1;
	protected ParticleSystem damageLevel2;
	protected ParticleSystem damageLevel3;
	/* Each ship must have 1 death animation which is played when ship health drops to 0 */
	//protected ParticleSystem deathAnim;

	void Start()
	{
		if (!previewMode) {
			energyBar = GameObject.Find ("energyBar").GetComponent<EnergyBar> ();
			energyBar.setLength (1f);
			shipBody = GetComponent<Rigidbody> ();
			currentEnergy = energy;
		}
		initShip();
	}

	public virtual void initShip()
	{
		Debug.Log ("base init");
		ParticleSystem[] pSystems = GetComponentsInChildren<ParticleSystem>();
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
		}
	}

	public virtual void Update()
	{
		
		ReadUserInput();
		if (!previewMode) {
			if (movement.Left) {
				Move (Direction.Left);
			} 
			if (movement.Right) {
				Move (Direction.Right);
			}
			if (movement.Forward) {
				Move (Direction.Up);
			}
		}

	}

	protected void ReduceEnergy( float amount )
	{
		switch (state) {
		case ShipState.Alive:
			//Debug.Log("currentEnergy: " + currentEnergy + " damage: " + amount);
			currentEnergy = Mathf.Clamp(currentEnergy - amount, 0, energy);
			energyBar.setLength((float)currentEnergy/(float)energy);
			if (currentEnergy <= 0) {
				State = ShipState.Stunned;
			}
			break;
		default:
			break;
		}
	}

	protected void ReduceArmor( int amount )
	{
		switch (state) {
		case ShipState.Alive:
			if (armor <= 0) {
				State = ShipState.Dead;
			} else {
				State = ShipState.Stunned;
				if (IsInvoking("resetShip"))
					CancelInvoke("resetShip");
				Invoke("resetShip", 2);
			}
			break;
		default:
			break;
		}
	}

	protected void ReadUserInput()
	{
		if (Input.GetKeyDown("left")) {
			movement.Left = true;
		} 
		if (Input.GetKeyUp("left")) {
			movement.Left = false;
		}
		if (Input.GetKeyDown("right")) {
			movement.Right = true;
		}
		if (Input.GetKeyUp("right")) {
			movement.Right = false;
		}
		if (Input.GetKeyDown("up")) {
			movement.Forward = true;
            SoundManager.instance.PlayRocketAccelerate();           
        }
		if (Input.GetKeyUp("up")) {
			movement.Forward = false;
            SoundManager.instance.PlayRocketShutDown();
		}
		if (Input.GetKeyDown("space")) {
			movement.special = true;
            SoundManager.instance.PlayLoadWarp();
		}
		if (Input.GetKeyUp("space")) {
			movement.special = false;
            SoundManager.instance.PlayWarp();
		}
	}

	public virtual void Move(Direction d)
	{
		switch (controlType) {
		case ControlType.Asteroids:
			AsteroidsMove (d);
			break;
		case ControlType.Shooter:
			ShooterMove(d);
			break;
		default:
			break;
		}
	}

	/*
	public void ApplyDamage(int damage)
	{
		
		if (state == ShipState.Stunned)
			return;
		
		Debug.Log("currentEnergy: " + currentEnergy + " damage: " + damage);
		currentEnergy = Mathf.Clamp(currentEnergy - damage, 0, energy);

		energyBar.setLength((float)currentEnergy/(float)energy);
		if (currentEnergy <= 0) {
			State = ShipState.Dead;
		} else {
			State = ShipState.Stunned;
			if (IsInvoking("resetShip"))
				CancelInvoke("resetShip");
			Invoke("resetShip", 2);
		}
	}
	*/
	protected virtual void resetShip()
	{
		shipBody.constraints |= RigidbodyConstraints.FreezeRotationY;
		shipBody.velocity = Vector3.zero;
		shipBody.angularVelocity = Vector3.zero;
		transform.rotation = Quaternion.identity;
		transform.position = Vector3.Scale(Vector3.forward, transform.position);
		state = ShipState.Alive;
	}

	void OnCollisionEnter(Collision collision)
	{	
		OnTriggerEnter(collision.collider);
	}

	void OnTriggerEnter(Collider collider)
	{	
		string n = collider.name;
		n.Replace ("(Clone)", "");
		Debug.Log ("name: " + n);
		switch (n) {
		case "fuel1":
			Destroy (collider.gameObject);
                SoundManager.instance.PlayFuelPickup();
			currentEnergy = Mathf.Clamp (currentEnergy + 20, 0, energy);
			//ReduceEnergy (Mathf.Clamp (currentEnergy + 20, 0, energy));
			energyBar.setLength((float)currentEnergy / (float)energy);
			break;
		case "meteorite1":
		case "meteorite2":
        case "Asteroid3":
        case"Asteroid4":
			shipBody.constraints &= ~RigidbodyConstraints.FreezeRotationY;
			shipBody.angularVelocity = Random.Range (-100, 100) * Vector3.one;
			ReduceArmor(30);                
			break;
		default:
			break;
		}
		CheckCollision (collider);
	}

	protected virtual void CheckCollision(Collider collider) { }

	protected void AsteroidsMove( Direction d ) 
	{
		Vector3 newVelocity = shipBody.velocity;

		switch (d) {
		case Direction.Down:
			break;
		case Direction.Left:
			transform.Rotate (Vector3.down, Time.fixedDeltaTime * turnSpeed);
			break;
		case Direction.Right:
			transform.Rotate (Vector3.up, Time.fixedDeltaTime * turnSpeed);
			break;
		case Direction.Up:
			if (currentEnergy > 0) {
				newVelocity += (Time.fixedDeltaTime * acceleration.z * transform.TransformDirection (Vector3.forward));
				ReduceEnergy (fuelBurnRate);
				//currentEnergy -= fuelBurnRate;
				energyBar.setLength ((float)currentEnergy / (float)energy);
			} 
			break;
		default:
			break;
		}

		newVelocity = Mathf.Clamp(newVelocity.magnitude, 0, maxSpeed) * Vector3.Normalize(newVelocity);
		if (newVelocity.z < forcedForwardSpeed)//Gravity toward earth
			newVelocity.z = forcedForwardSpeed;
		// Set new velocity for the ship.
		shipBody.velocity = newVelocity;
	}

	protected void ShooterMove( Direction d ) 
	{
		Vector3 newVelocity = shipBody.velocity;
		Vector3 v = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
		Quaternion roll = Quaternion.AngleAxis (v.x * tiltAngles.x, Vector3.back);
		Quaternion pitch = Quaternion.AngleAxis (v.z * tiltAngles.z, Vector3.right);
		transform.rotation = Quaternion.RotateTowards (transform.rotation, roll*pitch, tiltSpeed);

		v.Scale (shipBody.mass*acceleration);
		shipBody.AddForce (v);
	}
}
