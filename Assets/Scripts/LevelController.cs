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
    public UnityEvent onIncreaseDifficulty;

    // Game variables
    [Tooltip("The relative speed of the player")]
    [Range(0, 10)] public float scrollSpeed = 10f;
    
    [HideInInspector] public bool ScrollState = true;
    [HideInInspector] public bool gameOver = false;
    [HideInInspector] public int score = 0;
    private float timeToScore = .1f;

    // Difficulty levels
    private bool level1 = true;
    private bool level2 = false;
    private bool level3 = false;

    // Distance variables
    private float distanceAtStart;
    private float distanceToShark;
    [HideInInspector] public float distancePercentage;

    // GameObjects
    private GameObject player;
    private GameObject shark;

    #endregion

    private void Start()
    {
        #region initialize events

        onGameOver ??= new UnityEvent();
        onIncreaseDifficulty ??= new UnityEvent();

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
        StartCoroutine(AddScore());
    }

    private void Update()
    {
        // calculate distance between shark and player if the player is not dead
        if (player && shark)
        {
            distanceToShark = Vector3.Distance(player.transform.position, shark.transform.position);
            distancePercentage = distanceToShark / distanceAtStart;
        }

        // start level 2 after 40 seconds survived
        if (score > 400 && !level2)
        {
            onIncreaseDifficulty.Invoke();
            level2 = true;
        }

        // start level 3 after 100 seconds survived
        if (score > 1000 && !level3)
        {
            onIncreaseDifficulty.Invoke();
            level3 = true;
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
    private IEnumerator AddScore()
    {
        while (true && !gameOver)
        {
            score += 1;
            yield return new WaitForSeconds(timeToScore);
            // Debug.Log($"Score: {score}");
        }
    }
}