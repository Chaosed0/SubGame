using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour {

    public GameObject objToFollow;

	// Use this for initialization
	void Start () 
    {
	    	
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.position = new Vector3(objToFollow.transform.position.x, objToFollow.transform.position.y, transform.position.z);
	}
}
