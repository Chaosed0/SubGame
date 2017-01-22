using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DepthText : MonoBehaviour {
    public float fadeInTime = 1.0f;
    public float fadeOutTime = 1.0f;
    public float stayTime = 1.0f;

    public float timer = 0.0f;

    private Text text;
    private RectTransform rectTransform;

    private const float margin = 15.0f;
    private float initialY = 200.0f;

    enum State {
        fadeIn,
        stay,
        fadeOut,
        inactive
    };

    State state = State.inactive;

    void Start() {
        text = GetComponent<Text>();
        rectTransform = GetComponent<RectTransform>();
        initialY = rectTransform.anchoredPosition.y;
    }

	void Update () {
        if (state == State.inactive) {
            return;
        }

        timer += Time.deltaTime;

        Vector2 position = rectTransform.anchoredPosition;
        position.y = initialY - margin + (margin * 2 * timer / (fadeInTime + fadeOutTime + stayTime));
        rectTransform.anchoredPosition = position;

        if (timer <= fadeInTime) {
            text.color = new Color(text.color.r, text.color.g, text.color.b, timer / fadeInTime);
        }

        if (timer >= fadeInTime + stayTime) {
            state = State.fadeOut;
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1.0f - (timer - fadeInTime - stayTime) / fadeOutTime);
        }

        if (timer >= fadeInTime + stayTime + fadeOutTime) {
            state = State.inactive;
        }
	}

    public void ShowDepth(int depth) {
        text.text = depth + " METERS";
        Vector2 position = rectTransform.anchoredPosition;
        position.y = initialY-margin;
        rectTransform.anchoredPosition = position;

        state = State.fadeIn;
        timer = 0.0f;
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.0f);
    }
}
