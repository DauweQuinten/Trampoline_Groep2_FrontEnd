using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


public class LevelController : MonoBehaviour
{
    #region variables

    // Events
    public UnityEvent onGameOver;

    // Game variables
    public float scrollSpeed = 10f;
    public bool ScrollState = true;
    public bool gameOver = false;
    public int score = 0;
    private float timeToScore = .1f;

    // Distance variables
    private float distanceAtStart;
    private float distanceToShark;
    public float distancePercentage;

    // GameObjects
    private GameObject player;
    private GameObject shark;

    #endregion


    private void Start()
    {
        #region initialize events

        onGameOver ??= new UnityEvent();

        #endregion

        #region get game objects

        player = GameObject.Find("Boat");
        shark = GameObject.Find("Shark");

        #endregion

        #region startup logging

        Debug.Log("Game started!");
        Debug.Log("Players are mapped as follows:");
        Debug.Log($"left player's index is: {GameVariablesHolder.playerMapping[0]}");
        Debug.Log($"right player's index is: {GameVariablesHolder.playerMapping[1]}");

        #endregion


        // Get initial distance between player and shark at start of game
        distanceAtStart = Vector3.Distance(player.transform.position, shark.transform.position);

        // Start the score counter
        StartCoroutine(addScore(timeToScore));
    }

    private void Update()
    {
        // calculate distance between shark and player if the player is not dead
        if (player && shark)
        {
            distanceToShark = Vector3.Distance(player.transform.position, shark.transform.position);
            distancePercentage = distanceToShark / distanceAtStart;
        }

        // handle game over
        if (gameOver == true)
        {
            GameVariablesHolder.Score = score;
            onGameOver.Invoke();
            SceneManager.LoadScene("Game-over");
        }
    }


    // Add score per time survived
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