using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseController : MonoBehaviour {
    private const int breachesOnLose = 3;
    private const float fadeOutTime = 3.0f;

    private bool hasLost = false;
    private float timer = 0.0f;

	void Start () {
	}

	void Update () {
        Breach[] breachObjects = FindObjectsOfType<Breach>();
        int breaches = 0;
        foreach (Breach breach in breachObjects) {
            if (breach.permanent) {
                breaches++;
            }
        }

        if (breaches >= breachesOnLose) {
            hasLost = true;
        }

        if (hasLost) {
        }
	}
}
