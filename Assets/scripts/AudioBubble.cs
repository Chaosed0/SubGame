using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBubble : MonoBehaviour {

    public float totalSound;

    public float minScale;//this is the scale that the bubble is at when at 0 volume

    public BubbleTriggerScript bubbleTriggerScript;

	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        //totalSound = 0;
	}

    void LateUpdate()
    {
		transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(totalSound + minScale, totalSound + minScale, totalSound + minScale), 0.1f);
        bubbleTriggerScript.bubblesTotalAudio = totalSound;

        totalSound = 0;
    }
}
