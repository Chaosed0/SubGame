using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarEmitter : MonoBehaviour {

	public GameObject RadarPulsePrefab;

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

		Instantiate (RadarPulsePrefab, transform.position, transform.rotation);

	}
}
