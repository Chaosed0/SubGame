﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pathfinder : MonoBehaviour {
    public float moveSpeed = 0.5f;

    private int pathIndex = 0;
    private bool isPathing = false;
    private Vector2[] path;

    public UnityEvent onPathFinished = new UnityEvent(); 

	void Start () {
	}

	void Update () {
        if (isPathing) {
            NavigateToNextPoint();
        }
	}

    public void StartPathing(Vector2[] path) {
        this.path = path;
        this.pathIndex = 0;
        this.isPathing = true;
    }

    private void NavigateToNextPoint() {
        Vector2 position = new Vector2(this.transform.position.x, this.transform.position.y);
        Vector2 direction = path[pathIndex] - position;
        Vector2 movement = direction.normalized * moveSpeed;

        this.transform.position = new Vector3(this.transform.position.x + movement.x, this.transform.position.y + movement.y, this.transform.position.z);

        Vector2 newPosition = new Vector2(this.transform.position.x, this.transform.position.y);
        if ((path[pathIndex] - newPosition).magnitude < 0.05f) {
            pathIndex++;
            if (pathIndex >= path.Length) {
                isPathing = false;

                if (onPathFinished != null) {
                    onPathFinished.Invoke();
                }
            }
        }
    }
}