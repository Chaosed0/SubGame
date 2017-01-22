﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SmallMonsterScript : MonoBehaviour {


	public Vector3 direction;

	public float speed; 

	private float lifeTime = 120;

	public GameObject DetectedImagePrefab;

	private float afterImageFadeTime = 0.5f;

	private bool hasTriggeredAttack = false;

    public Sprite[] spriteChoices;

	// Use this for initialization
	void Start () {

		direction = direction.normalized;

        GetComponent<SpriteRenderer>().sprite = spriteChoices[Random.Range(0, spriteChoices.Length)];

	}

	// Update is called once per frame
	void Update () {

		//Move monster
		float movementRatio = (Mathf.Sin(Time.time*5) + 2)/3f;

		transform.position += direction * speed * Time.deltaTime * movementRatio ;

		//Self orient head to direction
		transform.up = direction;

		//See if we need to despawn monster - super easy way
		lifeTime -= Time.deltaTime;
		if (lifeTime < 0)
			Destroy (gameObject);

	}

	public void SetDirection(Vector3 newDirection)
	{
		direction = newDirection.normalized;
	}


	public void DropDetectedImage(float remainingFadeTime)
	{
		GameObject newAfterImage = Instantiate (DetectedImagePrefab, transform.position, transform.rotation);
        newAfterImage.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
		//Slowly count down alpha
		//StartCoroutine(DecreaseAlphaCoroutine(newAfterImage.GetComponent<SpriteRenderer>(), remainingFadeTime));
        StartCoroutine(DecreaseAlphaCoroutine(newAfterImage.GetComponent<SpriteRenderer>(), 1));

		//Put red dot on UI
		RadarController.mainRadarController.CreateRedDot(transform.position);
	}

	IEnumerator DecreaseAlphaCoroutine( SpriteRenderer sprite, float lifeTime )
	{
		Color newColor = sprite.color;

		newColor.a = 1;
		sprite.color = newColor;

		while (lifeTime > 0 ) 
		{

			lifeTime -= Time.deltaTime;

            newColor.a = lifeTime / 1;//RadarPulse.maxLifeTime;

			sprite.color = newColor;

			yield return new WaitForEndOfFrame ();
		}

		Destroy (sprite.gameObject);
	}

	public void TriggerToAttack(Transform target)
	{
		if (hasTriggeredAttack)
			return;

		hasTriggeredAttack = true;

		//appear
		GetComponent<SpriteRenderer>().enabled = true;

		//Change direction to ship middle
		speed *=.1f;
		StartCoroutine(StartAttack (target.position - transform.position, 20, target));


		/*
		SetDirection(target-transform.position);
		direction.z = 0f;

		speed *= 10f;

*/
	}

	IEnumerator StartAttack(Vector3 targetDirection, float targetSpeed, Transform target)
	{
		float lerpTime = 1f;

		while (lerpTime > 0) 
		{
			lerpTime -= Time.deltaTime;
			
			//speed = Mathf.Lerp (speed, targetSpeed, 0.05f);
		
			SetDirection( Vector3.Lerp (direction, targetDirection, 0.05f)  );
			direction.z = 0;

			yield return new WaitForEndOfFrame ();
		}

		speed = targetSpeed;

		//Check if close enough, if yes trigger hull breach
		while(Vector3.Distance(target.position, transform.position) > 15f )
		{
			yield return new WaitForEndOfFrame ();
		}

		//Trigger hull breach
		Tile randomTile = Level.levelReference.getRandomTraversableTile();
		if (randomTile == null)
			Debug.Log ("Adsfasdf");
		Level.levelReference.SetTraversable (randomTile.transform.position, false);
	}
}
