using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMovementScript : MonoBehaviour {

	public Vector3 direction;
	public float speed = 5;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += direction * speed * Time.deltaTime;
	}
}
