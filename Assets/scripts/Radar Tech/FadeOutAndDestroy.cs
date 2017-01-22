using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutAndDestroy : MonoBehaviour {

	public float startAlpha = 1;

	public float maxLifetime = 2;

	float curLife;

	SpriteRenderer sprite;

	// Use this for initialization
	void Start () {

		curLife = maxLifetime;

		sprite = GetComponent<SpriteRenderer> ();

		StartCoroutine (Countdown());
	}

	IEnumerator Countdown()
	{

		while (curLife > 0) 
		{
			curLife -= Time.deltaTime;

			Color c = sprite.color;

			c.a = curLife / maxLifetime;

			sprite.color = c;

			yield return new WaitForEndOfFrame();
		}

		Destroy (gameObject);


	}
}
