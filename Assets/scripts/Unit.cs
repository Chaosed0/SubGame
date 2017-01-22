using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof (Pathfinder))]
public class Unit : MonoBehaviour {
    private float stress = 0.0f;
    private Pathfinder pathfinder;
    private State state = State.Idling;
    private Breach breachBeingRepaired = null;

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
    }

    void Update() {
        if (state != State.Panicked && stress >= panicThreshold) {
            // Panic!
            pathfinder.onPathFinished.Invoke();
        } else if (state == State.Panicked) {
            stress -= restingStressLoss * Time.deltaTime;
            if (stress <= panicOffThreshold) {
                // unpanic...
                state = State.Idling;
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
            } else {
                stress += ambientStressGain * Time.deltaTime;
            }
        }

        Debug.Log(stress);

        if (state == State.Repairing) {
            breachBeingRepaired.doWorkOnBreach();
        }
    }

    public bool IsOperating() {
        return state == State.Operating;
    }

    public bool IsPanicked() {
        return state == State.Panicked;
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

    private void OnPathFinished() {
        if (stress >= panicThreshold) {
            state = State.Panicked;
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
            if (tile.tileType == TileType.Break) {
                state = State.Resting;
            } else if (tile.tileType != TileType.Normal) {
                state = State.Operating;
            } else {
                state = State.Idling;
            }
        }
    }
}
