using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DepthMeter : MonoBehaviour {
    public Ship ship;

    private Text text;

	void Start () {
        text = GetComponent<Text>();
	}

	void Update () {
        text.text = string.Format("{0:0.00}", ship.depth);
	}
}
