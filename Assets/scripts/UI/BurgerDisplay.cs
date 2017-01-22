using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BurgerDisplay : MonoBehaviour {
    public bool isActive = false;
    public Image image;
    public Text text;
    public RectTransform rectTransform;
    public CanvasGroup canvasGroup;
    void Start() {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
}
