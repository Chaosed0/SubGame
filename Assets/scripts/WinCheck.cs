using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCheck : MonoBehaviour
{
    public GameObject fadePrefab;
    public float fadeTime = 2;

    private Ship ship;
    private float fadeAmount = 0;
    private SpriteRenderer fadeRenderer;
    private bool keepCheckingDepth = true;

    void Start()
	{
	    ship = GetComponent<Ship>();
	}
	
	// Update is called once per frame
	void Update()
    {
        if (keepCheckingDepth)
        {
            CheckDepth(); 
        }
	}

    private void CheckDepth()
    {
        if (ship.depth >= ship.maxDepth)
        {
            TriggerWin();
        }
    }

    private void TriggerWin()
    {
        keepCheckingDepth = false;
        GameObject fadeObject = Instantiate(fadePrefab, new Vector3(), new Quaternion());
        fadeRenderer = fadeObject.GetComponent<SpriteRenderer>();
        StartCoroutine(FadeToBlack());
    }

    IEnumerator FadeToBlack()
    {
        while (true)
        {
            fadeAmount = fadeAmount + (Time.deltaTime / fadeTime);
            fadeRenderer.color = new Color(0, 0, 0, fadeAmount);
            if (fadeAmount >= 1)
            {
                StopAllCoroutines();
                //SceneManager. .LoadScene("WinScene");
                UnityEngine.SceneManagement.SceneManager.LoadScene("WinScene");
                yield break;
            }
            yield return null;
        }
    }
}
