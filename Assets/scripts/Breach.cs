
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Breach : MonoBehaviour {
    public Level level;
    public Tile tile;
    public float workLeft = 5.0f;

    public UnityEvent onBreachFixed = new UnityEvent();

    public void doWorkOnBreach(float repairMulitplier) {
        workLeft -= Time.deltaTime * repairMulitplier;

        if (workLeft <= 0.0f) {
            level.SetTraversable(tile.transform.position, true);
            if (onBreachFixed != null) {
                onBreachFixed.Invoke();
            }
        }
    }
}
