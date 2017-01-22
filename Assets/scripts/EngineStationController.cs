using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineStationController : MonoBehaviour {

    //Reference to the ship 
    public Ship ship;

    public bool engineOn;
    public AudioSource engineSound;

	// Use this for initialization
	void Start () 
    {
        ship.onStationEntered.AddListener (OnEnterEffect);//Function with unit and tile type
        ship.onStationExited.AddListener (OnExitEffect);
	}
	
	// Update is called once per frame
	void Update () 
    {
	    	
	}

    void OnEnterEffect(Unit unit, TileType type)
    {
        if (type == TileType.Engine)
        {
            if (engineOn == true)
            {
                engineSound.mute = true;
                engineSound.gameObject.GetComponent<AddsToAudioBubble>().makingSound = false;
                engineOn = false;
            }
            else
            {
                engineSound.mute = false;
                engineSound.gameObject.GetComponent<AddsToAudioBubble>().makingSound = true;
                engineOn = true;
            }
        }
    }

    void OnExitEffect(Unit unit, TileType type)
    {
        
    }
}
