using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarStation : MonoBehaviour {

	//Reference to the ship 
	public Ship ship;

	public enum SonarNumber{Room1,Room2}

	public SonarNumber sonarNumber;

	//Time for a normally skilled person to trigger
	private float sonarMaxCooldown = 2f;
	private float sonarCurrentCooldown = 0f;
    private float sonarCooldownMultiplier = 0f;
	private bool isOccupied = false;

	public RadarEmitter sonarEmitter;

	// Use this for initialization
	void Start () {
		ship.onStationEntered.AddListener (OnEnterEffect);//Function with unit and tile type
		ship.onStationExited.AddListener (OnExitEffect);
	}
	
	// Update is called once per frame
	void Update () {

		if (isOccupied) 
		{
			//Emit when we can
			if (sonarCurrentCooldown <= 0) 
			{
				sonarEmitter.CreatePulse ();
				sonarCurrentCooldown = sonarMaxCooldown;
			} 
			//else countdown
			else 
			{
				sonarCurrentCooldown -= Time.deltaTime * sonarCooldownMultiplier;
			}
		}
		
	}

	void OnEnterEffect(Unit unit, TileType type)
	{
		//Get rid of all non-relevant cases
		if (sonarNumber == SonarNumber.Room1) 
		{
			if (type != TileType.Sonar1)
				return;
		} 
		else if (sonarNumber == SonarNumber.Room2) 
		{
			if (type != TileType.Sonar2)
				return;
		}

		Debug.Log ("Entered Sonar");
	    sonarCooldownMultiplier = 1.0f * unit.unitStats.Sonar;
		isOccupied = true;
	}

	void OnExitEffect(Unit unit, TileType type)
	{

		//Get rid of all non-relevant cases
		if (sonarNumber == SonarNumber.Room1) 
		{
			if (type != TileType.Sonar1)
				return;
		} 
		else if (sonarNumber == SonarNumber.Room2) 
		{
			if (type != TileType.Sonar2)
				return;
		}

		Debug.Log ("Exited Sonar");
	    sonarCooldownMultiplier = 0f;
		isOccupied = false;
	}
}
