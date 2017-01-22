﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof (Pathfinder))]
public class Unit : MonoBehaviour {
    public float stress = 0.0f;
    private Pathfinder pathfinder;
    private State state = State.Idling;
    private Breach breachBeingRepaired = null;

    public UnitStats unitStats;
    public float ambientStressGain = 0.1f;
    public float restingStressLoss = 0.5f;
    public float panickingStressLoss = 10.0f;
    public float panicThreshold = 100.0f;
    public float panicOffThreshold = 50.0f;

    public float timeToRepair = 5.0f;

    public UnityEvent onPanicked = new UnityEvent();
    public UnityEvent onUnpanicked = new UnityEvent();

    public enum State {
        Idling,
        Moving,
        Repairing,
        Operating,
        Resting,
        Panicked,
    }

	void Start () {
        pathfinder = GetComponent<Pathfinder>();
        pathfinder.onPathStarted.AddListener(OnPathStarted);
        pathfinder.onPathFinished.AddListener(OnPathFinished);

	    unitStats = GetComponent<UnitStats>();
	}

    void Update() {
        if (state != State.Panicked && stress >= panicThreshold) {
            // Panic!
            pathfinder.onPathFinished.Invoke();
        } else if (state == State.Panicked) {
            stress -= panickingStressLoss * Time.deltaTime;
            if (stress <= panicOffThreshold) {
                // unpanic...
                state = State.Idling;
                GetComponent<AddsToAudioBubble>().makingPanicSound = false;
                if (pathfinder.onPathFinished != null) {
                    pathfinder.onPathFinished.Invoke();
                }
                if (onUnpanicked != null) {
                    onUnpanicked.Invoke();
                }
            }
        } else {
            if (state == State.Resting) {
                stress -= restingStressLoss * Time.deltaTime;
                GetComponent<AddsToAudioBubble>().makingRecRoomSound = true;
            } else {
                stress += ambientStressGain * Time.deltaTime;
                GetComponent<AddsToAudioBubble>().makingRecRoomSound = false;
            }
        }

        //Debug.Log(stress);

        if (state == State.Repairing)
        {
            breachBeingRepaired.doWorkOnBreach(unitStats.Repair);
            GetComponent<AddsToAudioBubble>().makingRepairSound = true;
            GetComponent<AnimateObject>().playingRepair = true;
            GetComponent<AnimateObject>().playingIdle = false;
        }
        else if (state == State.Idling)
        {
            GetComponent<AddsToAudioBubble>().makingRepairSound = false;
            GetComponent<AnimateObject>().playingRepair = false;
            GetComponent<AnimateObject>().playingIdle = true;
        }
        else if(state == State.Panicked)
        {
            GetComponent<AnimateObject>().playingPanic = true;
            GetComponent<AnimateObject>().playingIdle = false;
        }
    }

    public bool IsOperating() {
        return state == State.Operating;
    }

    public bool IsPanicked() {
        return state == State.Panicked;
    }

    public float GetStressLevel() {
        return stress / panicThreshold;
    }

    private void OnBreachFixed() {
        breachBeingRepaired = null;
        // slight hack: invoke pathfinder's onPathFinished which resets our state and
        // notifies other systems that this unit is ready for another task
        pathfinder.onPathFinished.Invoke();
    }

    private void OnPathStarted() {
        state = State.Moving;
    }

    public bool EatFood(float destressAmount)
    {
        if (stress > 0)
        {
            stress = Mathf.Max(stress - destressAmount, 0.0f);
            return true;
        }
        return false;
    }

    private void OnPathFinished() {
        if (stress >= panicThreshold) {
            state = State.Panicked;
            GetComponent<AddsToAudioBubble>().makingPanicSound = true;
            if (onPanicked != null) {
                onPanicked.Invoke();
            }
            return;
        }

        Breach repairCandidate = pathfinder.level.getAdjacentUntraversableTile(this.transform.position);
        if (repairCandidate != null) {
            breachBeingRepaired = repairCandidate;
            breachBeingRepaired.onBreachFixed.AddListener(OnBreachFixed);
            state = State.Repairing;
        } else {
            Tile tile = pathfinder.level.TileAtWorldPosition(this.transform.position);
            if (tile.tileType == TileType.Rec) {
                state = State.Resting;
            } else if (tile.tileType != TileType.Normal) {
                state = State.Operating;
            } else {
                state = State.Idling;
            }
        }
    }
}
