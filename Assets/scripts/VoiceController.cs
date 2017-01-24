using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceController : MonoBehaviour {

    public bool randomCharacter;

    public GameObject[] voiceClips;

	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void PlayShortVoiceClip()
    {
        int index = Random.Range(0, voiceClips.Length);
        Instantiate(voiceClips[index], transform.position, Quaternion.identity);
    }
}
