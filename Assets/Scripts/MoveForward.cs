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

    #endregion
    
    
    // On start
    void Start()
    {
        // Get reference to script
        levelControllerScript = GameObject.Find("LevelController").GetComponent<LevelController>();
        sharkControllerScript = GameObject.Find("Shark").GetComponent<SharkController>();
        playerControlsScript = GameObject.Find("Boat").GetComponent<PlayerControls>();
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
