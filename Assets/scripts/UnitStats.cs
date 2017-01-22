using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats : MonoBehaviour {
    //
    // Add Unit Stats component to a unit for this to work
    //


    //quartermaster - navigation
    //sonar specialist 1 and 2 - sonar
    //engineer/mechanic - fixing - needs to eat more?
    //gunner - weapons - sanity decreases faster?
    //chaplin - sanity
    //cook - sanity

    [Header("Stat Multipliers")]
    [SerializeField]
    private float steering = 1.0f;
    [SerializeField]
    private float sonar = 1.0f;
    [SerializeField]
    private float repair = 1.0f;
    [SerializeField]
    private float sanity = 1.0f;
    [SerializeField]
    private float cooking = 1.0f;


    public float Steering { get { return steering; } }
    public float Sonar { get { return sonar; } }
    public float Repair { get { return repair; } }
    public float Sanity { get { return sanity; } }
    public float Cooking { get { return cooking; } }
}
