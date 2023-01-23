using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System.Net.Sockets;
using System;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Utilities;
using Unity.VisualScripting;

public class PlayerControls : MonoBehaviour
{
    // variables
    private LevelController levelControllerScript;
    private float xBounds = 3.5f;

    public bool hasCollided = false;
    public bool hittedWall = false;
    public bool keyboardEnabled;
    public bool isBackwards;
    
    
    // leeg scriptvariabele

    private int speed;
    public float maxForce = 8.0f;
    

    // Start is called before the first frame update
    void Start()
    {
        // start coroutines
        StartCoroutine(SlowDown());

        // get level controller script
        levelControllerScript = GameObject.Find("LevelController").GetComponent<LevelController>();

        //vraag script op adhv leeg object in de scene en dat steek je in je scripthandler variable

        //variabele uit socket script gelijk stellen aan lokale variabele
        // speed = wsHandler.socketSpeed;
    }


    // Update is called once per frame
    void Update()
    {
        // Get keyboard input from players (temporary input)


        if (keyboardEnabled)
        {          
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                speed -= 2;
                Debug.Log($"current speed: {speed}");
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                speed += 2;
                Debug.Log($"current speed: {speed}");
            }
        }


        //move the player left or right based on speed
        transform.Translate(Vector3.right * (Time.deltaTime * speed));    

        
        // position constraints
        if (transform.position.x > xBounds)
        {
            var transform1 = transform;
            var position = transform1.position;
            position = new Vector3(xBounds, position.y, position.z);
            transform1.position = position;
            speed = -speed/4;
            Debug.Log($"Player hit left wall: speed is {speed}");
            hittedWall = true;
        }
        
        if (transform.position.x < -xBounds)
        {
            var transform1 = transform;
            var position = transform1.position;
            position = new Vector3(-xBounds, position.y, position.z);
            transform1.position = position;
            speed = -speed/4;
            Debug.Log($"Player hit right wall: speed is {speed}");
            hittedWall = true;
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


    IEnumerator ToggleBackwardsMovementAfterSeconds(float seconds)
    {
        isBackwards = true;
        yield return new WaitForSeconds(seconds);
        isBackwards = false;
    }




    // On collision
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("BOEM");
            StartCoroutine(ToggleBackwardsMovementAfterSeconds(0.5f));
        }
    }

    // Change color
    public void ChangeColor(Color newColor)
    {
        Debug.Log("Color should change");
        GetComponent<Renderer>().material.color = newColor;
    }


    public void Paddle(float force, int player)
    {
        switch (player)
        {
            case 0:
                Debug.Log("Player 1 jumped with force: " + force);
                speed += Mathf.FloorToInt(maxForce * force);
                break;
            case 1:
                Debug.Log("Player 2 jumped with force: " + force);
                speed -= Mathf.FloorToInt(maxForce * force);
                break;
        }
    }
}
        