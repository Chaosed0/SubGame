using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBubble : MonoBehaviour {

    public float totalSound;

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
		transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(totalSound, totalSound, totalSound), 0.1f);

        totalSound = 0;
    }
}
