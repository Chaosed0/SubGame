using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleSprite : MonoBehaviour {



	// Use this for initialization
	void Start () 
    {
        Bounds bounds = GetComponent<SpriteRenderer>().sprite.bounds;
        var xSize = bounds.size.x;
        transform.localScale = new Vector3(1 / xSize, 1 / xSize, transform.localScale.z);
	}
	
	// Update is called once per frame
    void Update () 
    {
		
	}
}
