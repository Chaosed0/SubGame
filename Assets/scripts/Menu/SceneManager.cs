using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{

    public String prevSceneName = "";
    public String nextSceneName = "";

    public void ChangeToPreviousScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(prevSceneName);
    }

    public void ChangeToNextScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
    }
}
