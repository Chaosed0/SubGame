using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Pathfinder))]
public class Unit : MonoBehaviour {
    private float stress = 0.0f;
    private Pathfinder pathfinder;
    private State state = State.Idling;
    private Breach breachBeingRepaired = null;

    public UnitStats unitStats;
    public float ambientStressGain = 0.1f;
    public float restingStressLoss = 0.25f;

    public float timeToRepair = 5.0f;

    public enum State {
        Idling,
        Moving,
        Repairing,
        Operating,
        Resting,
    }

	void Start () {
        pathfinder = GetComponent<Pathfinder>();
        pathfinder.onPathStarted.AddListener(OnPathStarted);
        pathfinder.onPathFinished.AddListener(OnPathFinished);

	    unitStats = GetComponent<UnitStats>();
	}

    void Update() {
        if (state == State.Resting) {
            stress -= restingStressLoss * Time.deltaTime;
        } else {
            stress += ambientStressGain * Time.deltaTime;
        }

        if (state == State.Repairing) {
            breachBeingRepaired.doWorkOnBreach(unitStats.Repair);
        }
    }

    public bool IsOperating() {
        return state == State.Operating;
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
