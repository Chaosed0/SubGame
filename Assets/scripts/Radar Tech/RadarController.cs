using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarController : MonoBehaviour {

	public static RadarController mainRadarController;

	public static float maxRadarDistanceFromCenter = 20f;

	public GameObject MainSub;

	public GameObject RedDotPrefab;

	public SpriteRenderer radarUIBase;

	private float radarUIWidth;

	// Use this for initialization
	void Start () {

		mainRadarController = this;

		radarUIWidth = radarUIBase.GetComponent<SpriteRenderer> ().bounds.extents.x;
	}
	
	// Update is called once per frame
	void Update () {

//		radarUIBase.GetComponent<SpriteRenderer> ().bounds.extents.x;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;

		Gizmos.DrawWireSphere (MainSub.transform.position, maxRadarDistanceFromCenter);

	//	Gizmos.color = Color.blue;
	//	Gizmos.DrawLine (transform.position - new Vector3(0,radarUIBase.GetComponent<SpriteRenderer> ().bounds.extents.x,0),
	//		transform.position + new Vector3(0,radarUIBase.GetComponent<SpriteRenderer> ().bounds.extents.x,0));
	}

	//We need to take worldObjectLocation and transfer into dot on UI
	public void CreateRedDot(Vector3 worldObjectLocation)
	{
		//Find position
		Vector3 ratioToSub = new Vector3 ((worldObjectLocation.x-MainSub.transform.position.x)/maxRadarDistanceFromCenter, (worldObjectLocation.y-MainSub.transform.position.y)/maxRadarDistanceFromCenter, 1);

		Vector3 position = radarUIBase.transform.position + radarUIWidth * ratioToSub;

		GameObject reddot = Instantiate (RedDotPrefab, position, transform.rotation);
		reddot.transform.SetParent (gameObject.transform);
	}
}
