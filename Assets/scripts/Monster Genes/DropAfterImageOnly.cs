using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropAfterImageOnly : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	public void DropDetectedImage()
	{
		GameObject newAfterImage = Instantiate (gameObject, transform.position, transform.rotation);
		//newAfterImage.GetComponent<DropAfterImageOnly> ().enabled = false;
		Destroy (newAfterImage.GetComponent<DropAfterImageOnly> ());


		newAfterImage.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;

		newAfterImage.GetComponent<SpriteRenderer> ().enabled = true;

		//Slowly count down alpha
		//StartCoroutine(DecreaseAlphaCoroutine(newAfterImage.GetComponent<SpriteRenderer>(), remainingFadeTime));
		StartCoroutine(DecreaseAlphaCoroutine(newAfterImage.GetComponent<SpriteRenderer>(), 1));

		//Put red dot on UI
		//		RadarController.mainRadarController.CreateRedDot(transform.position);
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
}
