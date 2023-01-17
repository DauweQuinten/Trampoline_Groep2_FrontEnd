using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft;
using System.Net.Sockets;

public class PlayerControls : MonoBehaviour
{
    // variables
    private WebSocket ws;
    private LevelController levelControllerScript;
    private float speed;
    private float xBounds = 3.5f;
    private int playerIndex;

    public float maxForce = 10.0f;

    
    // Start is called before the first frame update
    void Start()
    {
        // setup sockets
        SetupForceSocket();

        // start coroutines
        StartCoroutine(SlowDown());

        // get level controller script
        levelControllerScript = GameObject.Find("LevelController").GetComponent<LevelController>();
    }


    // Update is called once per frame
    void Update()
    {
        // Get keyboard input from players (temporary input)
        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    speed -= 2f;
        //    Debug.Log($"Current speed: {speed}");
        //}
        //else if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    speed += 2f;
        //    Debug.Log($"Current speed: {speed}");
        //}


        //move the player left or right based on speed
        // transform.Translate(Vector3.right * Time.deltaTime * speed);    

                               
        // position constraints
        if(transform.position.x > xBounds)
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


    // setup socket connection and add force
    void SetupForceSocket()
    {
        // Setup new connection to socket
        ws = new WebSocket("ws://172.30.248.55:3000");
        ws.Connect();


        // on message received
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message received: " + e.Data);
            var message = JsonUtility.FromJson<SocketMessage>(e.Data);

            // Check if the message is a jump message
            if (message.jump)
            {
                Debug.Log("Jump received");
                JumpMessage jumpMessage = message.jump;

               
                // check which player has jumped
                if (jumpMessage.Index == "p1")
                {
                    playerIndex = 1;
                }
                else if (jumpMessage.Index == "p2")
                {
                    playerIndex = -1;
                }

                float force = jumpMessage.Force;

                // add the force 
                speed += maxForce * force * playerIndex;
                Debug.Log($"Current speed: {speed}");
                
            }        
        };
    }


    // On collision
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            levelControllerScript.ScrollState = false;
            speed = 0;
            Debug.Log("BOEM");
        }
    }
            
    // On collision exit
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            levelControllerScript.ScrollState = true;
        }
    }
}
        