using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    public Level level;
    public Camera camera;
    public Pathfinder testPathfinder;

	void Start () {
	}

	void Update () {
        bool click = Input.GetButtonDown("Fire1");

        if (click) {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPoint = camera.ScreenToWorldPoint(mousePosition);

            if (mousePosition.x >= 0.0f && mousePosition.y >= 0.0f &&
                mousePosition.x <= Screen.width && mousePosition.y <= Screen.height)
            {
                GameObject tile = level.TileAtPosition(new Vector2(worldPoint.x, worldPoint.y));
                if (tile != null) {
                    Vector2[] path = level.FindPath(testPathfinder.transform.position, worldPoint);
                    testPathfinder.StartPathing(path);
                }
            }
        }
    }
}
