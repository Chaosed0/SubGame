using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType {
    Normal,
    Pilot,
    Break,
}

public class Tile : MonoBehaviour {
    public int occupyingUnitId = -1;
    public bool traversable = true;
    public TileType tileType;

    public bool CanUnitMoveHere(int unitId) {
        return occupyingUnitId < 0 && traversable;
    }
}
