using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pathfinder : MonoBehaviour {
    private int pathIndex = 0;
    private bool isPathing = false;
    private bool isSlowed = false;
    private Vector2[] path;
    private Vector2 destination;

    public float moveSpeed = 2.0f;
    public float slowMoveSpeed = 1.0f;
    public int unitId = 0;
    public PlayerInput playerInput;
    public Level level;

    public UnityEvent onPathFinished = new UnityEvent(); 
    public UnityEvent onPathStarted = new UnityEvent();

	void Start () {
        level.onMapChanged.AddListener(OnRepathNeeded);
	}

	void Update () {
        if (isPathing) {
            NavigateToNextPoint();
        }
	}

    public void StartPathing(Vector2 destination) {
        this.destination = destination;
        this.pathIndex = 0;
        this.isPathing = true;

        level.UpdateOccupancy(destination, this.unitId);
        path = level.FindPath(this.transform.position, destination);

        if (onPathStarted != null) {
            onPathStarted.Invoke();
        }
    }

    private void OnRepathNeeded() {
        if (this.isPathing) {
            // See if our path has been obstructed
            for (int i = pathIndex; i < path.Length; i++) {
                if (!level.IsTraversable(path[i])) {
                    System.Array.Resize(ref path, i);
                    level.UpdateOccupancy(path[i-1], this.unitId);
                }
            }
        }
    }

    private void NavigateToNextPoint() {
        Vector2 position = new Vector2(this.transform.position.x, this.transform.position.y);
        Vector2 direction = path[pathIndex] - position;
        Vector2 movement = direction.normalized * (isSlowed ? slowMoveSpeed : moveSpeed) * Time.deltaTime;

        this.transform.position = new Vector3(this.transform.position.x + movement.x, this.transform.position.y + movement.y, this.transform.position.z);

        Vector2 newPosition = new Vector2(this.transform.position.x, this.transform.position.y);
        if ((path[pathIndex] - newPosition).magnitude < 0.05f) {
            this.transform.position = path[pathIndex];

            pathIndex++;

            if (pathIndex >= path.Length) {
                isPathing = false;

                if (onPathFinished != null) {
                    onPathFinished.Invoke();
                }
            }
        }
    }

    void OnMouseDown() {
        playerInput.SelectPathfinder(this);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Pathfinder>() != null) {
            isSlowed = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.GetComponent<Pathfinder>() != null) {
            isSlowed = false;
        }
    }
}
