using System;
using UnityEngine.UI;
using UnityEngine;

public class TextToggler : MonoBehaviour
{
    public String startText = "";
    public String otherText = "";

    private bool isCurrentlyStartText;

	void Start()
    {
		SetText(startText);
        isCurrentlyStartText = true;
    }

    private void SetText(String textToSet)
    {
        Text textComponent = GetComponent<Text>();
        textComponent.text = textToSet;
    }

    public void ToggleText()
    {
        if (isCurrentlyStartText)
        {
            SetText(otherText);
            isCurrentlyStartText = false;
        }
        else
        {
            SetText(startText);
            isCurrentlyStartText = true;
        }
    }
}
