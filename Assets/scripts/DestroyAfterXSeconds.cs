using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterXSeconds : MonoBehaviour {

    public float timer;

	// Use this for initialization
	void Start () 
    {
	    	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
	}
}
