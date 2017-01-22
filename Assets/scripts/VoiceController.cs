using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceController : MonoBehaviour {

    public int characterNumber;

    public GameObject[] char01ShortVoiceClips;
    public GameObject[] char02ShortVoiceClips;
    public GameObject[] char03ShortVoiceClips;

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
        if (characterNumber == 1)
        {
            Instantiate(char01ShortVoiceClips[Random.Range(0, char01ShortVoiceClips.Length)], transform.position, Quaternion.identity);
        }
        else if (characterNumber == 2)
        {
            Instantiate(char01ShortVoiceClips[Random.Range(0, char02ShortVoiceClips.Length)], transform.position, Quaternion.identity);
        }
        else if (characterNumber == 3)
        {
            Instantiate(char01ShortVoiceClips[Random.Range(0, char03ShortVoiceClips.Length)], transform.position, Quaternion.identity);
        }
    }
}
