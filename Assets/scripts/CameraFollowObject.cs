using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour {

    public GameObject objToFollow;

    public float zoomedInAmount;

    public float zoomedOutAmount;

    public bool zoomedOut;
    public bool pressingZoomToggle;

	// Use this for initialization
	void Start () 
    {
	    	
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.position = new Vector3(objToFollow.transform.position.x, objToFollow.transform.position.y, transform.position.z);

        if (Input.GetAxis("Space") != 0)
        {
            if (pressingZoomToggle == false)
            {
                if (zoomedOut == false)
                {
                    GetComponent<Camera>().orthographicSize = zoomedOutAmount;
                    zoomedOut = true;
                }
                else
                {
                    GetComponent<Camera>().orthographicSize = zoomedInAmount;
                    zoomedOut = false;
                }

                pressingZoomToggle = true;
            }
        }

        if (Input.GetAxis("Space") == 0)
        {
            pressingZoomToggle = false;
        }

       
	}
}
