using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddsToAudioBubble : MonoBehaviour {

    public AudioBubble bubble;
    
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
	    
    }

    void LateUpdate()
    {
        if (constant == true)
        {
            if (makingSound == true)
            {
                bubble.totalSound += amount + Random.Range(-0.1f, 0.1f);
            }
        }
    }
}
