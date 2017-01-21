using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
    public int occupyingUnitId = -1;
    public bool traversable = true;

    public bool CanUnitMoveHere(int unitId) {
        return occupyingUnitId < 0 && traversable;
    }
}
