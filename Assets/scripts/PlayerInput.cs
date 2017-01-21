using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    public Level level;
    public Camera camera;

    public Pathfinder selectedPathfinder = null;

	void Start () {
	}

	void Update () {
        bool select = Input.GetButtonDown("Select");
        bool testselect = Input.GetButtonDown("TestSelect");
        Vector3 mousePosition = Input.mousePosition;

        if (select) {
            this.HandleMouseClick(mousePosition);
        }

        if (testselect) {
            Vector3 worldPoint = camera.ScreenToWorldPoint(mousePosition);
            level.SetTraversable(worldPoint, !level.IsTraversable(worldPoint));
        }
    }

    void HandleMouseClick(Vector3 mousePosition) {
        if (mousePosition.x < 0.0f || mousePosition.y < 0.0f ||
            mousePosition.x > Screen.width || mousePosition.y > Screen.height)
        {
            // Mouse is out of bounds
            return;
        }

        Vector3 worldPoint = camera.ScreenToWorldPoint(mousePosition);
        if (selectedPathfinder != null) {
            // Try to move the last selected thing to the selected tile
            Tile tile = level.TileAtPosition(new Vector2(worldPoint.x, worldPoint.y));

            // Make sure the tile is valid
            if (tile != null && tile.CanUnitMoveHere(selectedPathfinder.unitId)) {
                Vector2[] path = level.FindPath(selectedPathfinder.transform.position, worldPoint);
                selectedPathfinder.StartPathing(path);
                level.UpdateOccupancy(worldPoint, selectedPathfinder.unitId);
            }
        }
    }

    public void SelectPathfinder(Pathfinder pathfinder) {
        this.selectedPathfinder = pathfinder;
    }
}
