using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour {

	public float seconds = 2;

	// Use this for initialization
	void Start () {

		StartCoroutine (StartCountDown ());
	}
	
	IEnumerator StartCountDown()
	{
		yield return new WaitForSeconds (seconds);

		Destroy (gameObject);
	}
}
