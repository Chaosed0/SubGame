using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ColorToggler : MonoBehaviour
{
    public Color startColor = Color.green;
    public Color otherColor = Color.red;

    private bool isCurrentlyStartColor;

    void Start()
    {
        SetColor(startColor);
        isCurrentlyStartColor = true;
    }

    private void SetColor(Color colorToSet)
    {
        Image imageComponent = GetComponent<Image>();
        imageComponent.color = colorToSet;
    }

    public void ToggleColor()
    {
        if (isCurrentlyStartColor)
        {
            SetColor(otherColor);
            isCurrentlyStartColor = false;
        }
        else
        {
            SetColor(startColor);
            isCurrentlyStartColor = true;
        }
    }
}
