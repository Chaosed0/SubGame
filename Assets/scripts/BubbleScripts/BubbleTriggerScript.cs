using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleTriggerScript : MonoBehaviour {

    //this is a variable that is sent from the audioBubble.cs script, since it resets to 0 there. we don't want it to reset to 0 here
    public float bubblesTotalAudio;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		

	void OnTriggerEnter2D(Collider2D collider)
	{
		SmallMonsterScript monsterScript;
		
		if (monsterScript = collider.GetComponent<SmallMonsterScript> ()) 
		{
            AudioBubble audioBubble = transform.parent.gameObject.GetComponent<AudioBubble>();
            if (bubblesTotalAudio > audioBubble.minScale)//only trigger an attack if player is making sound
            {
                print("attacked");
                monsterScript.TriggerToAttack(transform);
            }
		}
	}


}
