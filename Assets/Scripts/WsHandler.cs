using System.Collections;
using System.Collections.Generic;
using Models;
using Newtonsoft.Json;
using UnityEngine;
using WebSocketSharp;

public class WsHandler : MonoBehaviour
{

    WebSocket ws;
    public float socketSpeed;
    private int playerIndex;
    PlayerControls playerControls;
    private float playermaxForce;

    void Start()
    {
        Debug.Log("script started");
        //reference naar de maxForce uit de PlayerControls
        playerControls = GameObject.Find("Boat").GetComponent<PlayerControls>();
        playermaxForce = playerControls.maxForce;
        
        // setup sockets
        SetupSocket();
    }


    void SetupSocket()
    {
        // Setup new connection to socket
        ws = new WebSocket("ws://172.30.248.55:3000");
        ws.Connect();
        
        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("Socket Open!");
        };

        // on message received
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message received: " + e.Data);

            // parse message
            var message = JsonConvert.DeserializeObject<SocketOnMessage>(e.Data);
            // Check if the message is a jump message
            if (message.Jump != null)
            {
                Debug.Log("Jump received");
                var jumpMessage = message.Jump;

                playerIndex = jumpMessage.Player switch
                {
                    // check which player has jumped
                    0 => 1,
                    1 => -1,
                    _ => playerIndex
                };

                var force = jumpMessage.Force;

                // add the force 
                socketSpeed += playermaxForce * force * playerIndex;
                Debug.Log($"Current speed: {socketSpeed}");

            }
            if (message.Button != null)
            {
                Debug.Log("Button pressed");
                var btnMessage = message.Button.BtnState;
                Debug.Log(btnMessage);
                if (btnMessage == BtnState.BOTH)
                {
                    Debug.Log("Both buttons pressed");
                }
                else if (btnMessage == BtnState.LEFT)
                {
                    Debug.Log("Left button pressed");
                }
                else if (btnMessage == BtnState.RIGHT)
                {
                    Debug.Log("Right button pressed");
                }
            }

            // if message.inactive -> haai komt vooruit
        };
    }
}
