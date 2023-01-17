using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    private float speed;
    private LevelController levelControllerScript;
    private SharkController sharkControllerScript;

    // On start
    void Start()
    {     
        levelControllerScript = GameObject.Find("LevelController").GetComponent<LevelController>();
        sharkControllerScript = GameObject.Find("Shark").GetComponent<SharkController>();
    }


    // Update is called once per frame
    void Update()
    {
        if (levelControllerScript.ScrollState)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
    
        if (sharkControllerScript.isMoving)
        {
            speed = levelControllerScript.scrollSpeed / 2;
        }
        else
        {    
            speed = levelControllerScript.scrollSpeed;
        }
        
        Debug.Log(speed);
    }
}
