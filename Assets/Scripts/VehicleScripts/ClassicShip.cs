using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicShip : Ship {

	private ParticleSystem dead;
	private ParticleSystem ps;

	private ParticleSystem damageSmoke1;
	private ParticleSystem damageSmoke2;

	private ParticleSystem explosion;

	private ParticleSystem leftThrottle;
	private ParticleSystem rightThrottle;

	private Animator anim;

	public override void initShip()
	{
		ParticleSystem[] pSystems = GetComponentsInChildren<ParticleSystem>();

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
			/* 
			 * Check if user is pressing some movement button.
			 */
			if (State == ShipState.Alive && movement.Left || State == ShipState.Stunned && movement.Left) {
				PlaySoundAndAnim("MoveLeft");
				Move (Direction.Left);
			} else { if (!movement.LeftAlreadyStopped) PlaySoundAndAnim ("StopLeft"); }
			if (State == ShipState.Alive && movement.Right || State == ShipState.Stunned && movement.Right) {
				PlaySoundAndAnim("MoveRight");
				Move (Direction.Right);
			} else { if(!movement.RightAlreadyStopped) PlaySoundAndAnim("StopRight"); }
			if (State == ShipState.Alive && movement.Forward) {
				PlaySoundAndAnim("startForward");
				Move (Direction.Up);
			} else { if(!movement.ForwardAlreadyStopped) PlaySoundAndAnim("stopForward"); }
			/* 
			 * Check if user is pressing special action button.
			 */
			if (State == ShipState.Alive && movement.special) {

			}
		}
	}

	public void PlaySoundAndAnim( string action )
	{
		//Debug.Log("Classic ship anim: " + action );
		/*
		 * All animations and sounds are played here. 
		 */
		switch (action) {
		case "startForward":
			{
				//Debug.Log ("Forward animation started(Classic)");
				//GetComponentInChildren<Animation>()["Rotor|Normal"].speed = 6f;
				if (ps && !ps.isPlaying) {
					ps.Play ();
				}
				movement.ForwardAlreadyStopped = false;
			}
			break;
		case "stopForward":
			//GetComponentInChildren<Animation>()["Rotor|Normal"].speed = 1f;
			if (ps && ps.isPlaying) { ps.Stop (); }
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
        if (state == ShipState.Alive)
        {
            shipBody.constraints &= ~RigidbodyConstraints.FreezeRotationY;
            shipBody.angularVelocity = Random.Range(-100, 100) * Vector3.one;

            armor -= 20;
            Debug.Log("armor:" + armor);
            
			ReduceArmor(30);

            if (armor < 80)
            {
                damageLevel1.Play();
            }
            if (armor < 50)
            {
                Debug.Log("Start smoke!: " + armor);
                damageLevel2.Play();
            }
            if (armor < 30)
            {
                damageLevel3.Play();
            }
        }
        else if (state == ShipState.Dead)
        {
            explosion.Play();
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
