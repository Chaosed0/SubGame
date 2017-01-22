using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallMonsterSpawner : MonoBehaviour {

	public GameObject mainSubObject;
	public GameObject smallMonsterPrefab; 

	//Vector2 maxMinSpeed = new Vector2 (2.8f,3.2f);
    Vector2 maxMinSpeed = new Vector2 (2.8f, 3.2f);

	//This is a range to spawn at
	Vector2 spawnBoxYDistance = new Vector2(-30f , 6f);

	//Spawns this X distance either left or right
	Vector2 spawnBoxXDistance = new Vector2(-45f, 45f);

	Vector2 targetBoxYDistanceFromMonster = new Vector2(-5,5);


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {


		if(Input.GetKeyDown(KeyCode.A))
		{
			SpawnMonstersInTime (1, 20);
		}
	}


	public void SpawnMonstersInTime(int numMonster, float time)
	{
		for (int i = 0; i < numMonster; i++) 
		{
			StartCoroutine (SpawnMonstersInTime(Random.Range(0,time)));
		}


	}


	IEnumerator SpawnMonstersInTime(float time)
	{
		yield return new WaitForSeconds (time);

		Vector3 subPos = mainSubObject.transform.position;

		//Generate starting position
		//pick y position 
		float y = subPos.y + Random.Range(spawnBoxYDistance.x, spawnBoxYDistance.y);

		//pick X position
		float x = subPos.x + (randomBool() ? spawnBoxXDistance.x : spawnBoxXDistance.y);

		//Generate Monster
		GameObject newMonster = Instantiate(smallMonsterPrefab);
		newMonster.transform.position = new Vector3 (x, y, smallMonsterPrefab.transform.transform.position.z);

		SmallMonsterScript smallMonsterScript = newMonster.GetComponent<SmallMonsterScript> ();

		//Generate direction
		Vector3 targetPosition = new Vector3(subPos.x,newMonster.transform.position.y + Random.Range(targetBoxYDistanceFromMonster.x, targetBoxYDistanceFromMonster.y), 0);
		smallMonsterScript.direction = (targetPosition - newMonster.transform.position).normalized;

		//Generate velocity
		smallMonsterScript.speed = Random.Range(maxMinSpeed.x, maxMinSpeed.y);

        print("monster");
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;

		Vector3 subPos = mainSubObject.transform.position;

		Gizmos.DrawLine (subPos + new Vector3(0, spawnBoxYDistance.x, 0) , subPos + new Vector3(0,spawnBoxYDistance.y, 0));

		Gizmos.DrawLine (subPos + new Vector3(spawnBoxXDistance.x, 0, 0) , subPos + new Vector3(spawnBoxXDistance.y, 0, 0));

		Gizmos.color = Color.blue;

		Gizmos.DrawLine (subPos + new Vector3(1, targetBoxYDistanceFromMonster.x, 0), subPos + new Vector3(1, targetBoxYDistanceFromMonster.y, 0));
	}


	bool randomBool()
	{
		if (Random.value >= 0.5f)
			return true;
		return false;
	}
}
