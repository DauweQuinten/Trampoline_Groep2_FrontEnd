using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region variables

    // reference to prefabs to spawn
    public GameObject[] obstaclePrefabs;

    // range of spawn positions
    private float spawnRangeX = 5.4f;
  
    // holder for random spawn position
    private Vector3 spawnPos;

    // reference to other scripts
    private LevelController levelControllerScript;
    private PlayerControls playerControlsScript;

    // spawn rate
    [SerializeField][Tooltip("The time between each spawn")][Range(0, 10)]
    private float spawnRate = 4;

    // reference to coroutine
    Coroutine spawnRoutine;
    
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        // get reference to other scripts
        levelControllerScript = GameObject.Find("LevelController").GetComponent<LevelController>();
        playerControlsScript = GameObject.Find("Boat").GetComponent<PlayerControls>();

        // start spawning obstacles
        spawnRoutine = StartCoroutine(SpawnRandonObstacle(spawnRate));

        // trigger increase difficulty event from level controller
        levelControllerScript.onIncreaseDifficulty.AddListener(IncreaseDifficulty);
    }
    
    // spawn obstacles at random position
    IEnumerator SpawnRandonObstacle(float spawnRate)
    {
        while (!levelControllerScript.gameOver)
        {
            if (!playerControlsScript.isBackwards)
            {
                int obstacleIndex = Random.Range(0, obstaclePrefabs.Length);
                spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, -50);
                Instantiate(obstaclePrefabs[obstacleIndex], spawnPos, obstaclePrefabs[obstacleIndex].transform.rotation);
                
            }
            yield return new WaitForSeconds(spawnRate);
        }
    }

    void IncreaseDifficulty() 
    {
        // increase spawn rate
        StopCoroutine(spawnRoutine);
        spawnRate -= 1f;
        spawnRoutine = StartCoroutine(SpawnRandonObstacle(spawnRate));
    }


}
