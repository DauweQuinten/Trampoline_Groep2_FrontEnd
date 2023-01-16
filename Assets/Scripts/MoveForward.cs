using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public int speed = 10;
    private LevelController levelControllerScript;

    // On start
    void Start()
    {
        levelControllerScript = GameObject.Find("LevelController").GetComponent<LevelController>();
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
