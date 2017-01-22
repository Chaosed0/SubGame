using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineStationController : MonoBehaviour {

    //Reference to the ship 
    public Ship ship;

    public bool engineOn = true;
    public AudioSource engineSound;

    public Light[] normalLights;
    public Light[] lowLights;
    public Unit[] units;

    public SonarStation sonar1;
    public SonarStation sonar2;
    public Kitchen kitchen;
    public Ship steering;

    public AudioSource engineOffSound;

	// Use this for initialization
	void Start () 
    {
        //ship.onStationEntered.AddListener (OnEnterEffect);
        //ship.onStationExited.AddListener (OnExitEffect);

        units = FindObjectsOfType<Unit>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    	
	}

    //public void OnEnterEffect(Unit unit, TileType type)
    public void SetEngineOn(bool engineOn)
    {
        if (this.engineOn == engineOn) {
            return;
        }

        this.engineOn = engineOn;
        if (!engineOn)
        {
            engineSound.mute = true;
            engineSound.gameObject.GetComponent<AddsToAudioBubble>().makingSound = false;
            for (int i = 0; i < normalLights.Length; i++)
            {
                normalLights[i].enabled = false;
                lowLights[i].enabled = true;
            }

            ShutDownSystems();
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
        }
    }

    public bool IsEngineOn() {
        return engineOn;
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

        foreach (Unit unit in units) {
            unit.SetStressMultiplier(10.0f);
        }
    }

    public void TurnOnSystems()
    {
        sonar1.lostPower = false;
        sonar2.lostPower = false;
        kitchen.lostPower = false;
        steering.lostPower = false;

        foreach (Unit unit in units) {
            unit.SetStressMultiplier(1.0f);
        }
    }
}
