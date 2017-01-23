using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoseController : MonoBehaviour {
    private const int breachesOnLose = 3;
    private const float fadeOutTime = 3.0f;
    private const float totalTime = 5.0f;

    private bool hasLost = false;
    private float timer = 0.0f;

    public CanvasGroup lossOverlayGroup;
    public Transform monster;
    public AudioSource source;

    public float finalX = 10.0f;
    private float initialX;

	void Start () {
        initialX = monster.transform.position.x;
	}

	void Update () {
        Breach[] breachObjects = FindObjectsOfType<Breach>();
        int breaches = 0;
        foreach (Breach breach in breachObjects) {
            if (breach.permanent) {
                breaches++;
            }
        }

        if (breaches >= breachesOnLose) {
            hasLost = true;
            source.Play();
        }

        if (hasLost) {
            timer += Time.deltaTime;
            if (timer >= 2.0f) {
                lossOverlayGroup.alpha = (timer - 2.0f) / fadeOutTime;
            }

            Vector2 position = monster.transform.position;
            position.x = initialX + (finalX - initialX) * timer / totalTime;
            monster.transform.position = position;

            if (timer >= totalTime) {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}
