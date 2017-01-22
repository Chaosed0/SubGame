using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Pathfinder))]
public class Unit : MonoBehaviour {
    public enum State {
        Idling,
        Moving,
        Repairing,
        Operating,
    }

    Pathfinder pathfinder;
    State state = State.Idling;

	void Start () {
        pathfinder = GetComponent<Pathfinder>();
        pathfinder.onPathStarted.AddListener(OnPathStarted);
        pathfinder.onPathFinished.AddListener(OnPathFinished);
    }

    private void OnPathStarted() {
        state = State.Moving;
    }

    private void OnPathFinished() {
        if (pathfinder.level.getAdjacentUntraversableTile(this.transform.position)) {
            state = State.Repairing;
        } else {
            Tile tile = pathfinder.level.TileAtWorldPosition(this.transform.position);
            if (tile.tileType != TileType.Normal) {
                state = State.Operating;
            }
        }
    }
}
