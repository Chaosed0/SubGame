using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarScreenPulse : MonoBehaviour {

	public KeyCode tempTrigger;

	private float increaseSpeed = 1.5f;
	private float alphaDecrease = 1f;

	private float maxScale;

	//public GameObject wavePrefab;

	// Use this for initialization
	void Start () {

		maxScale = transform.localScale.x;

		transform.localScale = Vector3.zero;

	}
	
	// Update is called once per frame
	void Update () {


		if(Input.GetKeyDown(tempTrigger))
		{
			StartCoroutine (Pulse());
		}

	}

	IEnumerator Pulse()
	{


		SpriteRenderer sprite = GetComponent<SpriteRenderer> ();
		transform.localScale = Vector3.zero;
		Color newSpriteColor = sprite.color;
		newSpriteColor.a = 1f;
		sprite.color = newSpriteColor;

		//Increase size
		while(transform.localScale.x < maxScale)
		{
			yield return new WaitForEndOfFrame ();

			float scaleAddition = maxScale * increaseSpeed * Time.deltaTime;
			
			transform.localScale += new Vector3 (scaleAddition, scaleAddition, scaleAddition);
		}

		transform.localScale = new Vector3 (maxScale, maxScale, maxScale);


		//Decrease alpha
		while (sprite.color.a > 0) 
		{

			newSpriteColor.a -= alphaDecrease * Time.deltaTime;
			sprite.color = newSpriteColor;

			yield return new WaitForEndOfFrame ();

		}


		yield return null;
	}
}
