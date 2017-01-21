using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddsToAudioBubble : MonoBehaviour {

    public AudioBubble bubble;
    public AudioSource mySound;

    public float amount;

    //if this is true, it's a continuous sound, like the engine running.
    public bool constant;

    public bool makingSound;



	// Use this for initialization
	void Start () 
    {
	    	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (constant == true)
        {
            if (makingSound == true)
            {
                bubble.totalSound += amount + Random.Range(-0.1f, 0.1f);
            }
        }
    }

    void LateUpdate()
    {
        
    }

    public void StartWalkingSound()
    {
        mySound.mute = false;
        makingSound = true;
    }

    public void StopWalkingSound()
    {
        mySound.mute = true;
        makingSound = false;
    }
}
