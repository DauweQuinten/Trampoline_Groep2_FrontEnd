using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


public class LevelController : MonoBehaviour
{
    public UnityEvent onGameOver;

    public float scrollSpeed = 10f;
    public bool ScrollState = true;
    public bool gameOver = false;
    private int score = 0;
    private float timeToScore = 0.5f;


    private void Start()
    {
        onGameOver ??= new UnityEvent();


        #region startup logging

        Debug.Log("Game started!");
        Debug.Log("Players are mapped as follows:");
        Debug.Log($"left player's index is: {GameVariablesHolder.playerMapping[0]}");
        Debug.Log($"right player's index is: {GameVariablesHolder.playerMapping[1]}");

        #endregion

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


    private void Update()
    {
        if (gameOver == true)
        {
            GameVariablesHolder.score = score;
            onGameOver.Invoke();

            // SceneManager.LoadScene(""); 

        }
        
    }
    
}
