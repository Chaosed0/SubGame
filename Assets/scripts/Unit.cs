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
        Resting
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
            state = State.Idling;
        }
    }

    void Update() {
        if (state != State.Idling) {
            Debug.Log(state);
        }
    }
}
