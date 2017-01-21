using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour {

    //quartermaster - navigation
    //sonar specialist 1 and 2 - sonar
    //engineer/mechanic - fixing - needs to eat more?
    //gunner - weapons - sanity decreases faster?
    //chaplin - sanity
    //cook - sanity

    [Header("Pros")]
    public bool proSteering;
    public bool proSonar;
    public bool proRepairs;
    public bool proSanity;

    [Header("Cons")]
    public bool conSteering;
    public bool conSonar;
    public bool conRepairs;
    public bool conSanity;

	// Use this for initialization
	void Start () 
    {
    	
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}
}
