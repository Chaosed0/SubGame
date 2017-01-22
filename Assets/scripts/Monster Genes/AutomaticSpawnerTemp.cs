using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AutomaticSpawnerTemp : MonoBehaviour {

	public SmallMonsterSpawner monsterSpawner;

	//Time wise(horizontal), max (value of 1) is 5
	//Vertical, max (value of 1)
	public AnimationCurve curve;

	public Ship ship;

	float maxSpawnPerTwentySeconds = 3f;

	// Use this for initialization
	void Start () {

		StartCoroutine (tempSpawner());
	}

	IEnumerator tempSpawner()
	{
        float cycleTime = 20;//5f;

		while (true) 
		{

			float t = ship._depth / 5f;
			int numberMonster = Mathf.RoundToInt(curve.Evaluate (t) * maxSpawnPerTwentySeconds);

			Debug.Log ("curve" + curve.Evaluate (t));
			Debug.Log ("num mon:" + numberMonster);

			monsterSpawner.SpawnMonstersInTime (numberMonster, cycleTime);

			yield return new WaitForSeconds (cycleTime);
		}
	}
}
