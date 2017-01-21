using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSoundsController : MonoBehaviour {

    public AudioSource[] shortSounds;
    public AudioSource[] longSounds;

    public float shortTimerMin;
    public float shortTimerMax;

    public float shortTimer;

    public float longTimerMin;
    public float longTimerMax;

    public float longTimer;


	// Use this for initialization
	void Start () 
    {
        shortTimer = Random.Range(shortTimerMin, shortTimerMax);

        longTimer = Random.Range(longTimerMin, longTimerMax);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (shortTimer > 0)
        {
            shortTimer -= Time.deltaTime;
        }
        else
        {
            //play sound
            AudioSource soundChoice = shortSounds[Random.Range(0, shortSounds.Length)];

            if (soundChoice.isPlaying == false)
            {
                soundChoice.Play();
            }

            //reset timer
            shortTimer = Random.Range(shortTimerMin, shortTimerMax);
        }

        if (longTimer > 0)
        {
            longTimer -= Time.deltaTime;
        }
        else
        {
            //play sound
            AudioSource soundChoice = longSounds[Random.Range(0, longSounds.Length)];

            if (soundChoice.isPlaying == false)
            {
                soundChoice.Play();
            }

            //reset timer
            longTimer = Random.Range(longTimerMin, longTimerMax);
        }
	}
}
