using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowObject : MonoBehaviour {

    public GameObject objToFollow;

    public float zoomedInAmount;

    public float zoomedOutAmount;

    public bool zoomedOut;
    public bool pressingZoomToggle;

    public bool isLerping;
    public float currentTime;
    public float lerpTime;
    public float ratio;

    float from;
    float to;

	// Use this for initialization
	void Start () 
    {
	    	
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.position = new Vector3(objToFollow.transform.position.x, objToFollow.transform.position.y, transform.position.z);

        if (isLerping == true)
        {
            if (currentTime < lerpTime)
            {
                currentTime += Time.deltaTime;
            }
            else
            {
                currentTime = lerpTime;
                isLerping = false;
            }

            ratio = currentTime / lerpTime;

            GetComponent<Camera>().orthographicSize = Mathf.SmoothStep(from, to, ratio);
        }
            

        if (Input.GetAxis("Space") != 0 && isLerping == false)
        {
            if (pressingZoomToggle == false)
            {
                if (zoomedOut == false)
                {
                    //GetComponent<Camera>().orthographicSize = zoomedOutAmount;
                    //zoomedOut = true;

                    from = zoomedInAmount;
                    to = zoomedOutAmount;
                    currentTime = 0;
                    zoomedOut = true;
                    isLerping = true;
                }
                else
                {
                    //GetComponent<Camera>().orthographicSize = zoomedInAmount;
                    //zoomedOut = false;

                    from = zoomedOutAmount;
                    to = zoomedInAmount;
                    currentTime = 0;
                    zoomedOut = false;
                    isLerping = true;
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
