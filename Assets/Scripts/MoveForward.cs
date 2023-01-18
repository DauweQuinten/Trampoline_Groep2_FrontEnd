using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    private float speed;
    
    private LevelController levelControllerScript;
    private SharkController sharkControllerScript;
    private PlayerControls playerControlsScript;

    // On start
    void Start()
    {     
        levelControllerScript = GameObject.Find("LevelController").GetComponent<LevelController>();
        sharkControllerScript = GameObject.Find("Shark").GetComponent<SharkController>();
        playerControlsScript = GameObject.Find("Boat").GetComponent<PlayerControls>();
    }


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);

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
