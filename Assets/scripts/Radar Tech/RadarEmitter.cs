﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarEmitter : MonoBehaviour {

	public GameObject RadarPulsePrefab;

    public AudioSource radarSound;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	

		if (Input.GetKeyDown (KeyCode.Q)) 
		{
			CreatePulse ();
		}
		

	}

	public void CreatePulse()
	{

        GameObject a = Instantiate (RadarPulsePrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
        a.transform.parent = transform.root;
        radarSound.Play();

	}
}
