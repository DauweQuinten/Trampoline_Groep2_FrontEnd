using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    private float speed;
    private LevelController levelControllerScript;

    // On start
    void Start()
    {     
        levelControllerScript = GameObject.Find("LevelController").GetComponent<LevelController>();
        speed = levelControllerScript.scrollSpeed;
    }


    // Update is called once per frame
    void Update()
    {
        if (levelControllerScript.ScrollState)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);           
        }     
    }
}
