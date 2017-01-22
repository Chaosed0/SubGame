using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alphasinscript : MonoBehaviour {

	float maxAlpha = 0.4f;

	//float randomTimeStart;

	SpriteRenderer sprite;

	// Use this for initialization
	void Start () {
		sprite = GetComponent<SpriteRenderer> ();

		//randomTimeStart = Random.Range (0, Mathf.PI);
	}
	
	// Update is called once per frame
	void Update () {

		Color newColor = sprite.color;

		newColor.a = ((Mathf.Sin (Time.time *3) + 1) / 2) * maxAlpha;


		sprite.color = newColor;
	}
}
