using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof (Unit))]
public class Pathfinder : MonoBehaviour {
    private int pathIndex = 0;
    private bool isPathing = false;
    private bool isSlowed = false;
    private Vector2[] path;
    private Vector2 destination;
    private Unit unit;
    private Vector2 movement;

    public float moveSpeed = 2.0f;
    public float slowMoveSpeed = 1.0f;
    public int unitId = 0;
    public PlayerInput playerInput;
    public Level level;

    public UnityEvent onPathFinished = new UnityEvent(); 
    public UnityEvent onPathStarted = new UnityEvent();
    public UnityEvent onSelected = new UnityEvent();

    public VoiceController vc;

	void Start () {
        unit = GetComponent<Unit>();
        level.onMapChanged.AddListener(OnRepathNeeded);

        vc = GetComponent<VoiceController>();
        transform.parent = level.transform;

        unit.onPanicked.AddListener(OnPanicked);

	}

	void Update () {
        if (unit.IsPanicked()) {
            return;
        }

        if (isPathing) {
            NavigateToNextPoint();
        }
	}

    public void StartPathing(Vector2 destination) {
        this.destination = destination;
        this.pathIndex = 0;
        this.isPathing = true;

        level.UpdateOccupancy(level.WorldToTilePosition(destination), this.unitId);
        path = level.FindPath(level.WorldToTilePosition(this.transform.position), level.WorldToTilePosition(destination));

        if (onPathStarted != null) {
            onPathStarted.Invoke();
        }
    }

    private void OnPanicked() {
        // Immediately stop moving
        this.isPathing = false;
        level.UpdateOccupancy(level.WorldToTilePosition(this.transform.position), this.unitId);
    }

    private void OnRepathNeeded() {
        if (this.isPathing) {
            // See if our path has been obstructed
            for (int i = pathIndex; i < path.Length; i++) {
                if (!level.IsTraversable(level.LocalToTilePosition(path[i]))) {
                    System.Array.Resize(ref path, i);
                    level.UpdateOccupancy(level.LocalToTilePosition(path[i-1]), this.unitId);
                }
            }
        } else {
            // Fire an event to let everyone know they might need to re-figure what they're doing
            if (onPathFinished != null) {
                onPathFinished.Invoke();
            }
        }
    }

    private void NavigateToNextPoint() {
        if (pathIndex >= path.Length) {
            isPathing = false;

            if (onPathFinished != null) {
                onPathFinished.Invoke();
            }
            return;
        }

        Vector2 position = new Vector2(this.transform.localPosition.x, this.transform.localPosition.y);
        Vector2 direction = path[pathIndex] - position;
        movement = direction.normalized * (isSlowed ? slowMoveSpeed : moveSpeed) * Time.deltaTime;

        this.transform.localPosition = new Vector3(this.transform.localPosition.x + movement.x, this.transform.localPosition.y + movement.y, this.transform.localPosition.z);

        if (movement.x > 0)
        {
            transform.localScale = new Vector3(5, transform.localScale.y, transform.localScale.z);
        }
        else if (movement.x < 0)
        {
            transform.localScale = new Vector3(-5, transform.localScale.y, transform.localScale.z);
        }

        Vector2 newPosition = new Vector2(this.transform.localPosition.x, this.transform.localPosition.y);
        if ((path[pathIndex] - newPosition).magnitude < 0.05f) {
            this.transform.localPosition = path[pathIndex];

            pathIndex++;

            if (pathIndex >= path.Length) {
                isPathing = false;

                if (onPathFinished != null) {
                    onPathFinished.Invoke();
                }
            }
        }
    }

    public Vector2 GetMovement() {
        if (isPathing) {
            return movement;
        }
        return new Vector2(0.0f, 0.0f);
    }

    void OnMouseDown() {
        playerInput.SelectPathfinder(this);
        
        if (onSelected != null) {
            onSelected.Invoke();
        }

        if (vc != null)
        {
            vc.PlayShortVoiceClip();
        }
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
