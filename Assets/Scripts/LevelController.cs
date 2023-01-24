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
    
    private float distanceAtStart;
    private float distanceToShark;
    public float distancePercentage;

    // GameObjects
    private GameObject player;
    private GameObject shark;



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


        // Get initial distance between player and shark
        distanceAtStart = Vector3.Distance(player.transform.position, shark.transform.position);

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
        // calculate distance between shark and player if the player is not dead
        if (player && shark)
        {
            distanceToShark = Vector3.Distance(player.transform.position, shark.transform.position);
            distancePercentage = MathF.Floor((distanceToShark / distanceAtStart)*100);        
        }
        
        if (gameOver == true)
        {
            GameVariablesHolder.Score = score;
            onGameOver.Invoke();
            SceneManager.LoadScene("Game-over"); 
        }
        
    }
    
}
