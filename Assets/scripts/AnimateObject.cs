using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateObject : MonoBehaviour {

    public bool playingIdle;
    public Sprite[] idleAnim;
    public int currentFrame;
    public float idleAnimTimerBase;
    public float idleAnimTimer;

	// Use this for initialization
	void Start () 
    {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (playingIdle == true)
        {
            if (idleAnimTimer > 0)
            {
                idleAnimTimer -= Time.deltaTime;
            }
            else
            {
                GoToNextFrame();

                idleAnimTimer = idleAnimTimerBase;
            }
        }
	}

    void GoToNextFrame()
    {
        if (currentFrame < idleAnim.Length - 1)
        {
            currentFrame += 1;
            GetComponent<SpriteRenderer>().sprite = idleAnim[currentFrame];
        }
        else
        {
            currentFrame = 0;
            GetComponent<SpriteRenderer>().sprite = idleAnim[currentFrame];
        }
    }
}
