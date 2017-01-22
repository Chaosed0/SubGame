using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ship : MonoBehaviour {
    private float _depth = 0.0f;
    private float moveFactor = 0.0f;

    public float moveSpeed = 10.0f;

    public Level level;
    public Unit[] crew;

    [System.Serializable]
    public class StationEvent : UnityEvent<Unit, TileType> { };
    public StationEvent onStationEntered = new StationEvent();
    public StationEvent onStationExited = new StationEvent();

    public float depth {
        get { return _depth; }
    }

	void Start () {
        for (int i = 0; i < crew.Length; i++) {
            Unit member = crew[i];
            Pathfinder pathfinder = member.GetComponent<Pathfinder>();
            pathfinder.onPathStarted.AddListener(() => OnPathStarted(member));
            pathfinder.onPathFinished.AddListener(() => OnPathFinished(member));
        }

        onStationEntered.AddListener(OnPilotStationEntered);
        onStationExited.AddListener(OnPilotStationExited);
	}

    void Update () {
        _depth += moveSpeed * moveFactor * Time.deltaTime;
        MoveShipDown(moveSpeed);

    }

    public void SetMoveFactor(float moveFactor) {
        this.moveFactor = moveFactor;
    }

    private void OnPathStarted(Unit unit) {
        // Unit is leaving a place, check if it's a station we need to know about
        Tile tile = level.TileAtWorldPosition(unit.transform.position);
        if (tile.tileType != TileType.Normal) {
            if (onStationExited != null) {
                onStationExited.Invoke(unit, tile.tileType);
            }
        }
    }

    private void OnPathFinished(Unit unit) {
        // Unit is entering a place, check if it's a station we need to know about
        Tile tile = level.TileAtWorldPosition(unit.transform.position);
        if (tile.tileType != TileType.Normal) {
            if (unit.IsOperating()) {
                if (onStationEntered != null) {
                    onStationEntered.Invoke(unit, tile.tileType);
                }
            } else {
                // The unit entered the station but he isn't operating it (probably repairing)
                if (onStationExited != null) {
                    onStationExited.Invoke(unit, tile.tileType);
                }
            }
        }
    }

    private void OnPilotStationEntered(Unit unit, TileType type) {
        if (type == TileType.Pilot) {
            moveFactor = 1.0f * unit.unitStats.Steering;
        }
    }

    private void OnPilotStationExited(Unit unit, TileType type) {
        if (type == TileType.Pilot) {
            moveFactor = 0.0f;
        }
    }

    public void MoveShipDown(float moveAmount)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - (moveAmount), transform.position.z);
        level.transform.position = new Vector3(level.transform.position.x, level.transform.position.y - (moveAmount), level.transform.position.z);
    }
}
