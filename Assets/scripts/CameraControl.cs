using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    public int pixelsPerUnit = 24;

    void Awake()
    {
        UpdateCameraSize();
    }

    void Update()
    {
        UpdateCameraSize();
    }

    void UpdateCameraSize()
    {
        int scalingFactor;
        if (Screen.height < 540)
        {
            scalingFactor = 1;
        }
        else if (Screen.height < 720)
        {
            scalingFactor = 2;
        }
        else if (Screen.height < 1080)
        {
            scalingFactor = 3;
        }
        else
        {
            scalingFactor = 4;
        }
        Camera.main.orthographicSize = Screen.height / (pixelsPerUnit * 2f * scalingFactor);
    }
}