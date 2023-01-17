using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System.Net.Sockets;
using System;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Utilities;


public class PlayerControls : MonoBehaviour
{
    // variables
    private LevelController levelControllerScript;
    //private float speed;
    private float xBounds = 3.5f;
    //private int playerIndex;

    public bool hasCollided = false;

    // leeg scriptvariabele
    private WsHandler wsHandler;

    private float speed;

    public float maxForce = 10.0f;

    
    // Start is called before the first frame update
    void Start()
    {
        // start coroutines
        StartCoroutine(SlowDown());

        // get level controller script
        levelControllerScript = GameObject.Find("LevelController").GetComponent<LevelController>();

        //vraag script op adhv leeg object in de scene en dat steek je in je scripthandler variable
        wsHandler = GameObject.Find("SocketController").GetComponent<WsHandler>();

        //variabele uit socket script gelijk stellen aan lokale variabele
        // speed = wsHandler.socketSpeed;
    }


    // Update is called once per frame
    void Update()
    {
        // Get keyboard input from players (temporary input)
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            speed -= 2f;
            Debug.Log($"current speed: {speed}");
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            speed += 2f;
            Debug.Log($"current speed: {speed}");
        }


        //move the player left or right based on speed
        transform.Translate(Vector3.right * Time.deltaTime * speed);    

        
        // position constraints
        if (transform.position.x > xBounds)
        {
            transform.position = new Vector3(xBounds, transform.position.y, transform.position.z);
            speed = 0;
            Debug.Log($"Player hit left wall: speed is {speed}");
        }
        
        if (transform.position.x < -xBounds)
        {
            transform.position = new Vector3(-xBounds, transform.position.y, transform.position.z);
            speed = 0;
            Debug.Log($"Player hit right wall: speed is {speed}");
        }     
    }



    // Coroutine to slow down the player
    IEnumerator SlowDown()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (speed != 0)
            {
                if (speed > 0)
                {
                    speed -= 1;
                }
                else if (speed < 0)
                {
                    speed += 1;
                }
                Debug.Log($"Current speed: {speed}");
            }
        }
    }

    // On collision
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            levelControllerScript.ScrollState = false;
            speed = 0;
            Debug.Log("BOEM");
            hasCollided = true;
        }
    }
            
    // On collision exit
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            levelControllerScript.ScrollState = true;
            hasCollided = false;
        }
    }
}
        