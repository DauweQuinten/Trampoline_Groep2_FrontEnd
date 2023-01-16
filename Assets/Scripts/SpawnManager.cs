using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Variables
    public GameObject[] obstaclePrefabs;
    public float spawnRangeX = 4;
  
    private Vector3 spawnPos;
    private LevelController levelControllerScript;


    // Start is called before the first frame update
    void Start()
    {
        levelControllerScript = GameObject.Find("LevelController").GetComponent<LevelController>();
        StartCoroutine(SpawnRandonObstacle());
    }

    IEnumerator SpawnRandonObstacle()
    {
        while (true)
        {
            if (levelControllerScript.ScrollState)
            {
                int obstacleIndex = Random.Range(0, obstaclePrefabs.Length);
                spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, -50);
                Instantiate(obstaclePrefabs[obstacleIndex], spawnPos, obstaclePrefabs[obstacleIndex].transform.rotation);
                
            }
            yield return new WaitForSeconds(2);
        }
    }
}
