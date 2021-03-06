﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof (Pathfinder))]
public class Unit : MonoBehaviour {
    public float stress = 0.0f;
    public float stressMinOnStart;
    public float stressMaxOnStart;
    private Pathfinder pathfinder;
    private AnimateObject animateObject;
    private AddsToAudioBubble audioBubble;
    private State state = State.Idling;
    private Breach breachBeingRepaired = null;

    private float stressGainMultiplier = 1.0f;

    public UnitStats unitStats;
    public float ambientStressGain = 0.1f;
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
        animateObject = GetComponent<AnimateObject>();
        audioBubble = GetComponent<AddsToAudioBubble>();

        stress = Random.Range(stressMinOnStart, stressMaxOnStart);
	}

    void Update() {
        if (state != State.Panicked && stress >= panicThreshold) {
            // Panic!
            // Uhhh, I can't explain why this panics really.
            pathfinder.onPathFinished.Invoke();
        } else if (state == State.Panicked) {
            stress -= panickingStressLoss * Time.deltaTime;
            if (stress <= panicOffThreshold) {
                // unpanic...
                state = State.Idling;
                audioBubble.makingPanicSound = false;
                if (pathfinder.onPathFinished != null) {
                    pathfinder.onPathFinished.Invoke();
                }
                if (onUnpanicked != null) {
                    onUnpanicked.Invoke();
                }
            }
        } else {
            if (state == State.Resting) {
                //audioBubble.makingRecRoomSound = true;
            } else {
                stress += ambientStressGain * stressGainMultiplier * Time.deltaTime;
                //audioBubble.makingRecRoomSound = false;
            }
        }

        if (state == State.Repairing)
        {
            breachBeingRepaired.doWorkOnBreach(unitStats.Repair);
            audioBubble.makingRepairSound = true;
            animateObject.playingRepair = true;
            animateObject.playingIdle = false;
        }
        else if (state == State.Idling || state == State.Operating)
        {
            audioBubble.makingRepairSound = false;
            animateObject.playingRepair = false;
            animateObject.playingIdle = true;
        }
        else if(state == State.Panicked)
        {
            audioBubble.makingRepairSound = false;
            animateObject.playingPanic = true;
            animateObject.playingIdle = false;
            animateObject.playingRepair = false;
        }
        else if (state == State.Moving)
        {
            Vector2 movement = pathfinder.GetMovement();
            audioBubble.makingRepairSound = false;
            animateObject.playingIdle = false;
            animateObject.playingPanic = false;
            animateObject.playingRepair = false;
            if (Mathf.Abs(movement.y) > Mathf.Abs(movement.x))
            {
                animateObject.playingWalk = false;
                animateObject.playingClimb = true;
            }
            else
            {
                audioBubble.makingRepairSound = false;
                animateObject.playingWalk = true;
                animateObject.playingClimb = false;
            }
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

    public void SetStressMultiplier(float multiplier) {
        stressGainMultiplier = multiplier;
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
        else
        {
            audioBubble.makingRecRoomSound = false;
        }
        return false;
    }

    private void OnPathFinished() {
        if (this.IsPanicked()) {
            return;
        }

        if (stress >= panicThreshold) {
            state = State.Panicked;
            audioBubble.makingPanicSound = true;
            if (onPanicked != null) {
                onPanicked.Invoke();
            }
            return;
        }

        Breach repairCandidate = pathfinder.level.getAdjacentUntraversableTile(pathfinder.level.WorldToTilePosition(this.transform.position));
        if (repairCandidate != null && repairCandidate.GetComponent<Breach>().permanent == false) {
            breachBeingRepaired = repairCandidate;
            breachBeingRepaired.onBreachFixed.AddListener(OnBreachFixed);
            state = State.Repairing;
        } else {
            Tile tile = pathfinder.level.TileAtWorldPosition(this.transform.position);

            if (tile.tileType != TileType.Normal) {
                state = State.Operating;

            } else {
                state = State.Idling;
            }
        }
    }
}
