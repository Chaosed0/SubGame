using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DepthMeter : MonoBehaviour {
    public Ship ship;

    private RectTransform rectTransform;
    private Image image;
    private float parentHeight = -1;

	void Start () {
        image = this.GetComponent<Image>();
        rectTransform = this.GetComponent<RectTransform>();
        RectTransform parent = this.transform.parent.GetComponent<RectTransform>();
        parentHeight = parent.rect.height;
	}

	void Update () {
        Vector2 anchoredPosition = rectTransform.anchoredPosition;
        anchoredPosition.y = -ship.GetDepthFraction() * parentHeight;
        rectTransform.anchoredPosition = anchoredPosition;
	}
}
