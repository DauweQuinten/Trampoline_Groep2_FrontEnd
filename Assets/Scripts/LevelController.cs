using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public float scrollSpeed = 10f;
    public bool ScrollState = true;
    public bool gameOver = false;
    private int score = 0;
    private float timeToScore = 0.5f;


    private void Start()
    {
        StartCoroutine(addScore(timeToScore));
    }

    IEnumerator addScore(float timeToScore)
    {
        while (true && !gameOver)
        {
            score += 1;
            yield return new WaitForSeconds(timeToScore);
            Debug.Log($"Score: {score}");
        }
    }
}
