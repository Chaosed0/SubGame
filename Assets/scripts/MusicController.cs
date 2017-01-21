using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {

    public AudioSource music;

    public GameObject theSub;
    public GameObject theCreature;
    public float creatureDistanceFromSub;
    public float maxDistanceCreatureCanBeFromSub;

	// Use this for initialization
	void Start () 
    {
	    	
	}
	
	// Update is called once per frame
	void Update () 
    {
        //creatureDistanceFromSub = Mathf.Abs(theSub.transform.position - theCreature.transform.position);

        //come back to this
	}
}
