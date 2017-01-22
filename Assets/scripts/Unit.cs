using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Pathfinder))]
public class Unit : MonoBehaviour {
    private float stress = 0.0f;
    private Pathfinder pathfinder;
    private State state = State.Idling;

    public float ambientStressGain = 0.1f;
    public float restingStressLoss = 0.25f;

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
    }

    void Update() {
        if (state == State.Resting) {
            stress -= restingStressLoss * Time.deltaTime;
        } else {
            stress += ambientStressGain * Time.deltaTime;
        }
    }

    private void OnPathStarted() {
        state = State.Moving;
    }

    private void OnPathFinished() {
        if (pathfinder.level.getAdjacentUntraversableTile(this.transform.position)) {
            state = State.Repairing;
        } else {
            Tile tile = pathfinder.level.TileAtWorldPosition(this.transform.position);
            if (tile.tileType == TileType.Break) {
                state = State.Resting;
            } else if (tile.tileType != TileType.Normal) {
                state = State.Operating;
            }
        }
    }
}
