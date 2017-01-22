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

		Vector3 targetLocalScale = new Vector3 (totalSound + minScale, totalSound + minScale, totalSound + minScale);
		//add in some randomness
		targetLocalScale *= Random.Range(0.9f,1.1f);

		//Lerp
		transform.localScale = Vector3.Lerp(transform.localScale, targetLocalScale, 0.1f);
        bubbleTriggerScript.bubblesTotalAudio = totalSound;

        totalSound = 0;
    }
}
