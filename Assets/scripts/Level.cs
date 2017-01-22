using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour {
    private Tile[,] levelTiles;
    private int[,] levelMap = {
        { 1, 1, 1, 0, 0, 3, 1, 4 },
        { 5, 1, 0, 0, 0, 0, 0, 1 },
        { 1, 1, 1, 1, 1, 1, 1, 1 },
        { 1, 0, 0, 0, 2, 0, 0, 1 },
        { 1, 1, 1, 1, 1, 1, 1, 1 },
    };

    private Dictionary<int, Breach> breaches = new Dictionary<int, Breach>();

    // DON'T USE TILE 0
    public Sprite[] tileImages;
    public Sprite breachOverlay;
    public Tile tilePrefab;
    public Breach breachPrefab;
    public Vector2 tileSize;

    public int tileBreakRoom;
    public int tilePilotRoom;
	public int tileSonar1Room;
	public int tileSonar2Room;
	public int tileEngineRoom;
	public int tileKitchenRoom;

    public Vector2 tileScale = new Vector2(2.0f, 2.0f);
    public float pixelsPerUnit = 108.0f;

    // Invoked when the map changes, e.g. when there's a hull breach
    public UnityEvent onMapChanged = new UnityEvent(); 

	void Start () {
        levelTiles = new Tile[levelMap.GetLength(0), levelMap.GetLength(1)];

        // Create child gameobjects for each tile
        for (int y = 0; y < levelMap.GetLength(0); y++) {
            for (int x = 0; x < levelMap.GetLength(1); x++) {
                int tileId = levelMap[y,x];
                if (tileId > 0 && tileId < tileImages.Length) {
                    Sprite tileImg = tileImages[tileId];
                    Vector3 position = positionForTileGameObject(x, y);
                    Tile tile = Instantiate<Tile>(tilePrefab, position, Quaternion.identity, this.transform);
                    tile.gameObject.name = "tile_" + x + "_" + y;
                    tile.transform.localScale = new Vector3(tileScale.x, tileScale.y, 0.0f);

                    SpriteRenderer tileSpriteRenderer = tile.GetComponent<SpriteRenderer>();
                    tileSpriteRenderer.sprite = tileImg;
                    levelTiles[y,x] = tile;

                    if (tileId == tileBreakRoom) {
                        tile.tileType = TileType.Break;
                    } else if (tileId == tilePilotRoom) {
                        tile.tileType = TileType.Pilot;
                    }
					else if (tileId == tileSonar1Room) {
						tile.tileType = TileType.Sonar1;
					}
					else if (tileId == tileSonar2Room) {
						tile.tileType = TileType.Sonar2;
					}
					else if (tileId == tileEngineRoom) {
						tile.tileType = TileType.Engine;
					}
					else if (tileId == tileKitchenRoom) {
						tile.tileType = TileType.Kitchen;
					}
                } else {
                    levelTiles[y,x] = null;
                }
            }
        }

        // Register all pathfinders with the level
        Pathfinder[] pathfinders = GameObject.FindObjectsOfType<Pathfinder>();
        for (int i = 0; i < pathfinders.Length; i++) {
            this.UpdateOccupancy(pathfinders[i].transform.position, pathfinders[i].unitId);
        }
	}

    private Vector3 positionForTileGameObject(int tileX, int tileY) {
        return new Vector3(
            (tileX * tileSize.x * tileScale.x + tileSize.x * tileScale.x / 2.0f) / pixelsPerUnit + transform.position.x,
            -(tileY * tileSize.y * tileScale.y + tileSize.y * tileScale.y / 2.0f) / pixelsPerUnit + transform.position.y,
            0.0f);
    }

    public bool IsTraversable(Vector2 worldPoint) {
        Vector2 tilePos = WorldToTilePosition(worldPoint);
        Tile tile = levelTiles[(int)tilePos.y, (int)tilePos.x];
        if (tile == null) {
            return false;
        }
        return tile.traversable;
    }

    public void SetTraversable(Vector2 worldPoint, bool traversable) {
        Vector2 tilePos = WorldToTilePosition(worldPoint);
        Tile tile = levelTiles[(int)tilePos.y, (int)tilePos.x];
        if (tile == null) {
            return;
        }

        if (tile.traversable != traversable) {
            tile.traversable = traversable;

            int tileId = TilePositionToTileId(tilePos);
            if (!traversable) {
                if (!breaches.ContainsKey(tileId)) {
                    Vector3 position = positionForTileGameObject((int)tilePos.x, (int)tilePos.y);
                    Breach breach = Instantiate<Breach>(breachPrefab, position, Quaternion.identity, this.transform);
                    breach.level = this;
                    breach.tile = tile;

                    SpriteRenderer renderer = breach.GetComponent<SpriteRenderer>();
                    renderer.sprite = breachOverlay;
                    renderer.transform.localScale = tileScale;
                    breaches.Add(tileId, breach);
                }
            } else {
                if (breaches.ContainsKey(tileId)) {
                    Breach breach = breaches[tileId];
                    breaches.Remove(tileId);
                    Destroy(breach.gameObject);
                }
            }

            if (onMapChanged != null) {
                onMapChanged.Invoke();
            }
        }
    }

    public Breach getAdjacentUntraversableTile(Vector2 worldPoint) {
        Vector2 tilePosition = WorldToTilePosition(worldPoint);

        Vector2 rightTilePosition = new Vector2(tilePosition.x+1, tilePosition.y);
        Vector2 leftTilePosition = new Vector2(tilePosition.x-1, tilePosition.y);
        Vector2 downTilePosition = new Vector2(tilePosition.x, tilePosition.y+1);
        Vector2 upTilePosition = new Vector2(tilePosition.x, tilePosition.y-1);

        Breach breach;
        Breach rightBreach;
        Breach leftBreach;
        Breach downBreach;
        Breach upBreach;

        bool exists = breaches.TryGetValue(TilePositionToTileId(tilePosition), out breach);
        bool rightExists = breaches.TryGetValue(TilePositionToTileId(rightTilePosition), out rightBreach);
        bool leftExists = breaches.TryGetValue(TilePositionToTileId(leftTilePosition), out leftBreach);
        bool downExists = breaches.TryGetValue(TilePositionToTileId(downTilePosition), out downBreach);
        bool upExists = breaches.TryGetValue(TilePositionToTileId(upTilePosition), out upBreach);

        if (exists) {
            return breach;
        }
        if (rightExists) {
            return rightBreach;
        }
        if (leftExists) {
            return leftBreach;
        }
        if (downExists) {
            return downBreach;
        }
        if (upExists) {
            return upBreach;
        }
        return null;
    }

    // Call this when the player selects a tile to move the unit to, to mark that no other
    // unit should move to that square.
    public void UpdateOccupancy(Vector2 worldPoint, int unitId) {
        Tile tile = TileAtWorldPosition(worldPoint);
        if (tile == null) {
            Debug.LogError("Tried to move person to null tile");
            return;
        }
        MoveUnit(tile, unitId);
    }

    public bool IsOccupied(Vector2 worldPoint) {
        Tile tile = TileAtWorldPosition(worldPoint);
        if (tile == null) {
            // It's occupied by water or hull, I guess
            return true;
        }
        return tile.occupyingUnitId >= 0;
    }

    private void MoveUnit(Tile newTile, int unitId) {
        Tile currentOccupiedTile = null;
        for (int y = 0; y < levelTiles.GetLength(0); y++) {
            for (int x = 0; x < levelTiles.GetLength(1); x++) {
                if (levelTiles[y,x] != null && 
                    levelTiles[y,x].occupyingUnitId == unitId) {
                    currentOccupiedTile = levelTiles[y,x];
                }
            }
        }

        if (currentOccupiedTile != null && currentOccupiedTile != newTile) {
            // Unit moved from one tile to another
            currentOccupiedTile.occupyingUnitId = -1;
            newTile.occupyingUnitId = unitId;
        } else if (currentOccupiedTile == null) {
            // Unit doesn't exist yet
            newTile.occupyingUnitId = unitId;
        }
        // Else is unit on same tile
    }

    public Tile TileAtWorldPosition(Vector2 worldPoint) {
        return TileAtTileCoords(WorldToTilePosition(worldPoint));
    }

    public Vector2[] FindPath(Vector2 start, Vector2 finish)
    {
        Vector2 startTile = WorldToTilePosition(start);
        Vector2 finishTile = WorldToTilePosition(finish);

        if (!isTilePassable(startTile)) {
            return new Vector2[] { start };
        }

        List<Vector2> queue = new List<Vector2>();
        queue.Add(startTile);

        Dictionary<int, Vector2> previousVisited = new Dictionary<int, Vector2>();
        previousVisited.Add(TilePositionToTileId(startTile), start);

        while (queue.Count > 0) {
            Vector2 tilePosition = queue[0];
            queue.RemoveAt(0);

            int tileX = (int)tilePosition.x;
            int tileY = (int)tilePosition.y;

            if (tileX == (int)finishTile.x &&
                tileY == (int)finishTile.y) {
                break;
            }
            
            Vector2 rightTile = new Vector2(tileX + 1, tileY);
            Vector2 leftTile = new Vector2(tileX - 1, tileY);
            Vector2 downTile = new Vector2(tileX, tileY + 1);
            Vector2 upTile = new Vector2(tileX, tileY - 1);

            if (isTilePassable(rightTile) && !previousVisited.ContainsKey(TilePositionToTileId(rightTile))) {
                queue.Add(rightTile);
                previousVisited.Add(TilePositionToTileId(rightTile), tilePosition);
            }
            if (isTilePassable(leftTile) && !previousVisited.ContainsKey(TilePositionToTileId(leftTile))) {
                queue.Add(leftTile);
                previousVisited.Add(TilePositionToTileId(leftTile), tilePosition);
            }
            if (isTilePassable(downTile) && !previousVisited.ContainsKey(TilePositionToTileId(downTile))) {
                queue.Add(downTile);
                previousVisited.Add(TilePositionToTileId(downTile), tilePosition);
            }
            if (isTilePassable(upTile) && !previousVisited.ContainsKey(TilePositionToTileId(upTile))) {
                queue.Add(upTile);
                previousVisited.Add(TilePositionToTileId(upTile), tilePosition);
            }
        }

        if (!previousVisited.ContainsKey(TilePositionToTileId(finishTile))) {
            // Didn't find a path
            return new Vector2[] { start };
        }

        // Reconstruct the path from the finish
        List<Vector2> tilePath = new List<Vector2>();
        tilePath.Add(finishTile);
        Vector2 pathTile = finishTile;
        while ((int)pathTile.x != (int)startTile.x || (int)pathTile.y != (int)startTile.y) {
            bool gotten = previousVisited.TryGetValue(TilePositionToTileId(pathTile), out pathTile);
            if (!gotten) {
                // Error
                Debug.LogError("We thought we found a path but we really didn't");
                break;
            }
            tilePath.Add(pathTile);
        }

        // Reverse tilePath and convert to world coords
        List<Vector2> worldPath = new List<Vector2>();
        for (int i = tilePath.Count - 1; i >= 0; i--) {
            worldPath.Add(TileToWorldPosition(tilePath[i]));
        }
        
        return worldPath.ToArray();
    }

    private int TilePositionToTileId(Vector2 tilePosition) {
        int tileX = (int)tilePosition.x;
        int tileY = (int)tilePosition.y;
        return tileY * levelMap.GetLength(1) + tileX;
    }

    private Vector2 WorldToTilePosition(Vector2 worldPoint) {
        float localX = (worldPoint.x - this.transform.position.x) * pixelsPerUnit;
        float localY = - (worldPoint.y - this.transform.position.y) * pixelsPerUnit;
        float tileX = Mathf.Floor(localX / (tileSize.x * tileScale.x));
        float tileY = Mathf.Floor(localY / (tileSize.y * tileScale.y));

        return new Vector2(tileX, tileY);
    }

    // Note, returns position centered on tile
    private Vector2 TileToWorldPosition(Vector2 tilePosition) {
        float localX = (tilePosition.x + 0.5f) * tileSize.x * tileScale.x;
        float localY = (tilePosition.y + 0.5f) * tileSize.y * tileScale.y;
        float worldX = localX / pixelsPerUnit + this.transform.position.x;
        float worldY = - localY / pixelsPerUnit + this.transform.position.y;

        return new Vector2(worldX, worldY);
    }

    private Tile TileAtTileCoords(Vector2 tilePosition) {
        int tileX = (int)tilePosition.x;
        int tileY = (int)tilePosition.y;

        if (tileY >= 0 &&
            tileX >= 0 &&
            tileY < levelTiles.GetLength(0) &&
            tileX < levelTiles.GetLength(1) &&
            levelTiles[tileY, tileX] != null)
        {
            return levelTiles[tileY, tileX];
        }

        return null;
    }

    private bool isTilePassable(Vector2 tilePosition) {
        Tile tile = TileAtTileCoords(tilePosition);
        if (tile == null) {
            return false;
        }
        return tile.traversable;
    }
}
