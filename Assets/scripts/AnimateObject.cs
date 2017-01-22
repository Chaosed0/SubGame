using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateObject : MonoBehaviour {

    public bool playingIdle;
    public Sprite[] idleAnim;
    public int currentFrame;
    public float idleAnimTimerBase;
    public float idleAnimTimer;

    public bool playingWalk;
    public Sprite[] walkAnim;
    public float walkAnimTimerBase;
    public float walkAnimTimer;

    public bool playingRepair;
    public Sprite[] repairAnim;
    public float repairAnimTimerBase;
    public float repairAnimTimer;

    public bool playingPanic;
    public Sprite[] panicAnim;
    public float panicAnimTimerBase;
    public float panicAnimTimer;

    public bool playingClimb;
    public Sprite[] climbAnim;
    public float climbAnimTimerBase;
    public float climbAnimTimer;

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
                GoToNextFrame(idleAnim);

                idleAnimTimer = idleAnimTimerBase;
            }
        }
        else if (playingWalk == true)
        {
            if (walkAnimTimer > 0)
            {
                walkAnimTimer -= Time.deltaTime;
            }
            else
            {
                GoToNextFrame(walkAnim);

                walkAnimTimer = walkAnimTimerBase;
            }
        }
        else if (playingRepair == true)
        {
            if (repairAnimTimer > 0)
            {
                repairAnimTimer -= Time.deltaTime;
            }
            else
            {
                GoToNextFrame(repairAnim);

                repairAnimTimer = repairAnimTimerBase;
            }
        }
        else if (playingPanic == true)
        {
            if (panicAnimTimer > 0)
            {
                panicAnimTimer -= Time.deltaTime;
            }
            else
            {
                GoToNextFrame(panicAnim);

                panicAnimTimer = panicAnimTimerBase;
            }
        }
        else if(playingClimb == true)
        {
            if (climbAnimTimer > 0)
            {
                climbAnimTimer -= Time.deltaTime;
            }
            else
            {
                GoToNextFrame(climbAnim);

                climbAnimTimer = climbAnimTimerBase;
            }
        }
	}

    void GoToNextFrame(Sprite[] sprites)
    {
        if (currentFrame < sprites.Length - 1)
        {
            currentFrame += 1;
            GetComponent<SpriteRenderer>().sprite = sprites[currentFrame];
        }
        else
        {
            currentFrame = 0;
            GetComponent<SpriteRenderer>().sprite = sprites[currentFrame];
        }
    }

    public void StartWalkAnim()
    {
        playingIdle = false;
        playingWalk = true;
    }

    public void StartIdleAnim()
    {
        playingIdle = true;
        playingWalk = false;
    }
}
