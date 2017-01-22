using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RadarPulse : MonoBehaviour {

	float sizeIncrease = 2.94f;

	public static float maxLifeTime = 20f;
	float currentLifeTime;

	// Use this for initialization
	void Start () {

		currentLifeTime = maxLifeTime;
	}
	
	// Update is called once per frame
	void Update () {

		SpriteRenderer sprite = GetComponent<SpriteRenderer> ();

		//Countdown 
		if (currentLifeTime > 0) 
		{
			currentLifeTime -= Time.deltaTime;

			Color newColor = sprite.color;
			newColor.a = Mathf.Pow(currentLifeTime / maxLifeTime, 2);
			sprite.color = newColor;

			float scaleDelta = sizeIncrease * Time.deltaTime;
			transform.localScale += new Vector3 (scaleDelta,scaleDelta,scaleDelta);

		} 
		else 
		{
			//Delete object
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{

		SmallMonsterScript monsterScript;

		if (monsterScript = collider.GetComponent<SmallMonsterScript> ()) 
		{
			monsterScript.DropDetectedImage (currentLifeTime);
		}
	}
}
