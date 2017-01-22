using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AutomaticSpawnerTemp : MonoBehaviour {

	public SmallMonsterSpawner monsterSpawner;

	// Use this for initialization
	void Start () {

		StartCoroutine (tempSpawner());
	}

	IEnumerator tempSpawner()
	{
		float cycleTime = 5f;

		while (true) 
		{
			monsterSpawner.SpawnMonstersInTime (2, cycleTime);

			yield return new WaitForSeconds (cycleTime);
		}
	}
}
