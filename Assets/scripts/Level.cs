using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
    private SpriteRenderer[,] levelTiles;
    private int[,] levelMap = {
        { 0, 0, 1, 1, 1, 1 },
        { 0, 1, 1, 1, 1, 0 },
        { 1, 1, 1, 1, 0, 0 },
    };

    // DON'T USE TILE 0
    public Sprite[] tileImages;
    public SpriteRenderer tilePrefab;
    public Vector2 tileSize;

    public Camera camera;

    public Vector2 tileScale = new Vector2(2.0f, 2.0f);
    public float pixelsPerUnit = 108.0f;

	void Start () {
        levelTiles = new SpriteRenderer[levelMap.GetLength(0), levelMap.GetLength(1)];
        // Create child gameobjects for each tile
        for (int y = 0; y < levelMap.GetLength(0); y++) {
            for (int x = 0; x < levelMap.GetLength(1); x++) {
                int tileId = levelMap[y,x];
                if (tileId > 0 && tileId < tileImages.Length) {
                    Sprite tileImg = tileImages[tileId];
                    Vector3 position = new Vector3(
                        (x * tileSize.x * tileScale.x + tileSize.x * tileScale.x / 2.0f) / pixelsPerUnit,
                        -(y * tileSize.y * tileScale.y + tileSize.y * tileScale.y / 2.0f) / pixelsPerUnit,
                        0.0f);
                    SpriteRenderer sprite = Instantiate<SpriteRenderer>(tilePrefab, position, Quaternion.identity, this.transform);
                    sprite.transform.localScale = new Vector3(tileScale.x, tileScale.y, 0.0f);
                    sprite.sprite = tileImg;
                    sprite.gameObject.name = x + " " + y;
                    levelTiles[y,x] = sprite;
                } else {
                    levelTiles[y,x] = null;
                }
            }
        }
	}

    public GameObject TileAtPosition(Vector2 mousePosition) {
        Vector3 worldPoint = camera.ScreenToWorldPoint(mousePosition);

        float localX = (worldPoint.x - this.transform.position.x) * pixelsPerUnit;
        float localY = - (worldPoint.y - this.transform.position.y) * pixelsPerUnit;
        int tileX = (int)Mathf.Floor(localX / (tileSize.x * tileScale.x));
        int tileY = (int)Mathf.Floor(localY / (tileSize.y * tileScale.y));

        if (tileY >= 0 &&
            tileX >= 0 &&
            tileY < levelTiles.GetLength(0) &&
            tileX < levelTiles.GetLength(1) &&
            levelTiles[tileY, tileX] != null)
        {
            return levelTiles[tileY, tileX].gameObject;
        }

        Debug.Log(localX + " " + localY + " " + tileX + " " + tileY);

        return null;
    }
}
