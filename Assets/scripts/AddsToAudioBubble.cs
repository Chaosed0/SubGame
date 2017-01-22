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

    public float repairAmount;
    public bool makingRepairSound;
    public AudioSource myRepairSound;

    public float recRoomAmount;
    public bool makingRecRoomSound;
    public AudioSource myRecRoomSound;

    public float panicAmount;//this is the amount of sound a panicking unit makes
    public bool makingPanicSound;
    public GameObject myPanicSound;
    public GameObject[] panicSoundsToChooseFrom;

	// Use this for initialization
	void Start () 
    {
        //myPanicSound = ;

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (constant == true)
        {
            if (makingSound == true)
            {
                bubble.totalSound += amount;// * Random.Range(0.9f, 1.1f);
            }

            if (myRepairSound != null)
            {
                if (makingRepairSound == true)
                {
                    myRepairSound.mute = false;
                    bubble.totalSound += repairAmount;
                }
                else
                {
                    myRepairSound.mute = true;
                }
            }

            if (myRecRoomSound != null)
            {
                if (makingRecRoomSound == true)
                {
                    myRecRoomSound.mute = false;
                    bubble.totalSound += recRoomAmount;
                }
                else
                {
                    myRecRoomSound.mute = true;
                }
            }

           
            if (makingPanicSound == true)
            {
                if (myPanicSound == null)
                {
                    GameObject a = Instantiate(panicSoundsToChooseFrom[Random.Range(0, panicSoundsToChooseFrom.Length)], transform.position, Quaternion.identity) as GameObject;
                    myPanicSound = a;
                }
                //myPanicSound.mute = false;
                //Instantiate(myPanicSound, transform.position, Quaternion.identity);
                //myPanicSound.mute = false;

                bubble.totalSound += panicAmount;

            }
            else
            {
                Destroy(myPanicSound);
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
