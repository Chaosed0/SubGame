using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDisplayController : MonoBehaviour {

    private RectTransform rectTransform;
    private FoodSack[] sacks;
    private BurgerDisplay[] displays;
    public BurgerDisplay prefab;

	void Start () {
        sacks = FindObjectsOfType<FoodSack>();
        displays = new BurgerDisplay[sacks.Length];

        for (int i = 0; i < sacks.Length; i++) {
            displays[i] = Instantiate<BurgerDisplay>(prefab, Vector3.zero, Quaternion.identity, this.transform);
            displays[i].GetComponent<CanvasGroup>().alpha = 0.0f;
            displays[i].isActive = false;

            int index = i;
            sacks[i].onFoodCountChanged.AddListener((x) => OnFoodCountChanged(index, x));
        }

        rectTransform = GetComponent<RectTransform>();
	}
	
	void Update () {
        for (int i = 0; i < displays.Length; i++) {
            if (!displays[i].isActive) {
                continue;
            }

            RectTransform CanvasRect = rectTransform;
            Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(sacks[i].transform.position);
            Vector2 WorldObject_ScreenPosition = new Vector2(((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
                ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)) + 12.0f);

            displays[i].rectTransform.anchoredPosition = WorldObject_ScreenPosition;
        }
    }

    private void OnFoodCountChanged(int sackIndex, int newCount) {
        if (newCount > 0) {
            displays[sackIndex].isActive = true;
            displays[sackIndex].canvasGroup.alpha = 1.0f;
            displays[sackIndex].text.text = "x" + newCount;
        } else {
            displays[sackIndex].isActive = false;
            displays[sackIndex].canvasGroup.alpha = 0.0f;
        }
    }
}
