using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
    public Level level;

	void Start () {
	}

	void Update () {
        bool click = Input.GetButtonDown("Fire1");

        if (click) {
            Vector3 mousePosition = Input.mousePosition;
            if (mousePosition.x >= 0.0f && mousePosition.y >= 0.0f &&
                mousePosition.x <= Screen.width && mousePosition.y <= Screen.height)
            {
                GameObject tile = level.TileAtPosition(new Vector2(mousePosition.x, mousePosition.y));
                Debug.Log(tile);
                if (tile != null) {
                    // Path
                }
            }
        }
    }
}
