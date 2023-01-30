using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    #region variables

    // Move speed   
    private float speed;
    
    // References to scripts
    private LevelController levelControllerScript;
    private SharkController sharkControllerScript;
    private PlayerControls playerControlsScript;

    // public gameobjects
    public GameObject player;
    public GameObject shark;
    public GameObject levelController;

    #endregion


    // On start
    void Awake()
    {
        //if (levelController == null)
        //{
        //    levelController = GameObject.Find("LevelController");
        //}
        //if (player == null)
        //{
        //    player = GameObject.Find("Boat");
        //}
        //if (shark == null)
        //{
        //    shark = GameObject.Find("Shark");
        //}
           
        levelControllerScript = levelController.GetComponent<LevelController>();
        sharkControllerScript = shark.GetComponent<SharkController>();
        playerControlsScript = player.GetComponent<PlayerControls>();
    }

    
    // Update is called once per frame
    void Update()
    {
        // translate forward over time based on scroll speed
        transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.World);

        // define speed based on game state
        if (levelControllerScript.gameOver)
        {
            speed = 0;
        }       
        else if (playerControlsScript.isBackwards == true)
        {
            speed = -levelControllerScript.scrollSpeed / 4;
        }       
        else if (sharkControllerScript.isMoving)
        {
            speed = levelControllerScript.scrollSpeed / 2;
        }
        else
        {    
            speed = levelControllerScript.scrollSpeed;
        }     
    }

}
