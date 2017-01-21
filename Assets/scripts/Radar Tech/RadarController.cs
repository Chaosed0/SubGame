using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarController : MonoBehaviour {

	public GameObject MainSub;

	public static float maxRadarDistanceFromCenter = 20f;

	float radarTravelSpeed = 10f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;

		Gizmos.DrawWireSphere (MainSub.transform.position, maxRadarDistanceFromCenter);
	}
}
