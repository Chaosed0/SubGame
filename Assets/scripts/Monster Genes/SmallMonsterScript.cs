using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SmallMonsterScript : MonoBehaviour {


	public Vector3 direction;

	public float speed; 

	private float lifeTime = 25f;

	public GameObject DetectedImagePrefab;

	private float afterImageFadeTime = 2f;

	private bool hasTriggeredAttack = false;

	// Use this for initialization
	void Start () {

		direction = direction.normalized;

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

		//Slowly count down alpha
		StartCoroutine(DecreaseAlphaCoroutine(newAfterImage.GetComponent<SpriteRenderer>(), remainingFadeTime));

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

			newColor.a = lifeTime / RadarPulse.maxLifeTime;

			sprite.color = newColor;

			yield return new WaitForEndOfFrame ();
		}

		Destroy (sprite.gameObject);
	}

	public void TriggerToAttack(Vector3 target)
	{
		if (hasTriggeredAttack)
			return;

		hasTriggeredAttack = true;

		//appear
		GetComponent<SpriteRenderer>().enabled = true;

		//Change direction to ship middle
		speed *=.1f;
		StartCoroutine(StartAttack (target - transform.position, 20, target));


		/*
		SetDirection(target-transform.position);
		direction.z = 0f;

		speed *= 10f;

*/
	}

	IEnumerator StartAttack(Vector3 targetDirection, float targetSpeed, Vector3 target)
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
		while(Vector3.Distance(target, transform.position) < 10f )
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
