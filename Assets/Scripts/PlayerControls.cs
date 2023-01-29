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
    #region variables

    // script references
    private SocketEvents socketEventsScript;

    // movement variables
    private float xBounds = 6.75f;
    private int speed;

    [SerializeField][Tooltip("This value determines how fast the player slows down (0 = instant stop, 1 = very slow stop)")]
    [Range(0, 1)] float slowDownValue = 0.5f;
    
    [SerializeField] [Tooltip("The maximum paddle force")]
    [Range(0, 10f)] private float maxForce = 5.0f;

    // player states
    [HideInInspector] public bool hasCollided = false;
    [HideInInspector] public bool hittedWall = false;
    [HideInInspector] public bool isBackwards;

    [Tooltip("Enable player control from keyboard")]
    public bool keyboardEnabled;

    // player indexen
    int leftPlayerIndex;
    int rightPlayerIndex;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        // Setup playerindex
        leftPlayerIndex = GameVariablesHolder.playerMapping[0];
        rightPlayerIndex = GameVariablesHolder.playerMapping[1];

        // slow down the speed over time
        StartCoroutine(SlowDown());

        // get socketcontroller script
        socketEventsScript = GameObject.Find("SocketController").GetComponent<SocketEvents>();

        // Paddle on jump from kinect        
        socketEventsScript.OnJump.AddListener((float force, int player) => { Paddle(force, player); });
    }


    // Update is called once per frame
    void Update()
    {
        // Move on keyboard input (if enabled)
        if (keyboardEnabled)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                speed -= Mathf.FloorToInt(maxForce);
                Debug.Log($"current speed: {speed}");
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                speed += Mathf.FloorToInt(maxForce);
                Debug.Log($"current speed: {speed}");
            }
        }

        //move the player left or right based on speed
        transform.Translate(Vector3.left * (Time.deltaTime * speed));

        // position constraints with bounce effect
        if (transform.position.x > xBounds)
        {
            var transform1 = transform;
            var position = transform1.position;
            position = new Vector3(xBounds, position.y, position.z);
            transform1.position = position;
            speed = -speed / 4;
            Debug.Log($"Player hit left wall: speed is {speed}");
            hittedWall = true;
        }

        if (transform.position.x < -xBounds)
        {
            var transform1 = transform;
            var position = transform1.position;
            position = new Vector3(-xBounds, position.y, position.z);
            transform1.position = position;
            speed = -speed / 4;
            Debug.Log($"Player hit right wall: speed is {speed}");
            hittedWall = true;
        }
    }


    // Coroutine to slow down the player
    IEnumerator SlowDown()
    {
        while (true)
        {
            yield return new WaitForSeconds(slowDownValue);
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

    // Move backwards on collision with object
    IEnumerator ToggleBackwardsMovementAfterSeconds(float seconds)
    {
        isBackwards = true;
        yield return new WaitForSeconds(seconds);
        isBackwards = false;
    }


    // On collision
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("BOEM");
            StartCoroutine(ToggleBackwardsMovementAfterSeconds(0.5f));
        }
    }

    
    // Change color (Temperary)
    public void ChangeColor(Color newColor)
    {
        Debug.Log("Color should change");
        GetComponent<Renderer>().material.color = newColor;
    }

    // Paddle on jump from kinect
    public void Paddle(float force, int player)
    {
        if (player == rightPlayerIndex)
        {
            Debug.Log("Left player jumped with force: " + force);
            speed += Mathf.FloorToInt(maxForce * force);
        }
        else if (player == leftPlayerIndex)
        {
            Debug.Log("Right player jumped with force: " + force);
            speed -= Mathf.FloorToInt(maxForce * force);
        }
    }
}