using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour {
    //
    // Add Unit Stats component to a unit for this to work
    //




    [Header("Stat Multipliers")]
    public float Steering = 1.0f;
    public float Sonar = 1.0f;
    public float Repair = 1.0f;
    public float Sanity = 1.0f;
    public float Cooking = 1.0f;


    public Sprite goodPortrait;
    public Sprite mediumPortrait;
    public Sprite badPortrait;
    public Sprite panicPortrait;

    void Start()
    {
        
    }
}
