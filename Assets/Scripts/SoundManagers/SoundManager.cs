using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance = null;
    public AudioSource fxSource1;
    public AudioSource fxSource2;
    public AudioSource musicSource;

    public AudioClip rocketAccelerate;
    public AudioClip rocketShutDown;
    public AudioClip collision;
    public AudioClip warpLoad;
    public AudioClip warpSound;
    public AudioClip fuelPickup;


	// Use this for initialization
	void Awake ()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

            DontDestroyOnLoad(gameObject);
        

        //fxSource1 = GetComponent<AudioSource>();	
	}
	
    public void PlayRocketAccelerate()
    {
        fxSource1.clip = rocketAccelerate;
        fxSource1.loop = true;
        fxSource1.Play();
        
    }

    public void PlayRocketShutDown()
    {
        fxSource1.clip = rocketAccelerate;
        fxSource1.Stop();
        fxSource1.PlayOneShot(rocketShutDown);
    }

    public void PlayLoadWarp()
    {
        fxSource2.volume = 1.5f;
        fxSource2.PlayOneShot(warpLoad);
    }
	
    public void PlayWarp()
    {
        fxSource2.clip = warpLoad;
        fxSource2.volume = 0.5f;
        fxSource2.Stop();
        fxSource2.PlayOneShot(warpSound);
    }

    public void PlayCollision()
    {
        fxSource2.PlayOneShot(collision);
    }

    public void PlayFuelPickup()
    {
        fxSource2.PlayOneShot(fuelPickup);
    }




	void Update () {
		
	}
}
