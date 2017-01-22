using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineStationController : MonoBehaviour {

    //Reference to the ship 
    public Ship ship;

    public bool engineOn;
    public AudioSource engineSound;

    public Light[] normalLights;
    public Light[] lowLights;

    public SonarStation sonar1;
    public SonarStation sonar2;
    public Kitchen kitchen;
    public Ship steering;

    public AudioSource engineOffSound;

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
                for (int i = 0; i < normalLights.Length; i++)
                {
                    normalLights[i].enabled = false;
                    lowLights[i].enabled = true;
                }

                ShutDownSystems();
                engineOn = false;
            }
            else
            {
                engineSound.mute = false;
                engineSound.gameObject.GetComponent<AddsToAudioBubble>().makingSound = true;
                for (int i = 0; i < normalLights.Length; i++)
                {
                    normalLights[i].enabled = true;
                    lowLights[i].enabled = false;
                }

                TurnOnSystems();
                engineOn = true;
            }
        }
    }

    void OnExitEffect(Unit unit, TileType type)
    {
        
    }

    public void ShutDownSystems()
    {
        Instantiate(engineOffSound, transform.position, Quaternion.identity);
        sonar1.lostPower = true;
        sonar2.lostPower = true;
        kitchen.lostPower = true;
        steering.lostPower = true;
    }

    public void TurnOnSystems()
    {
        sonar1.lostPower = false;
        sonar2.lostPower = false;
        kitchen.lostPower = false;
        steering.lostPower = false;
    }
}
