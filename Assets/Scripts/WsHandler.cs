using System.Collections;
using System.Collections.Generic;
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


        // on message received
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message received: " + e.Data);

            // parse message
            SocketMessage message = Newtonsoft.Json.JsonConvert.DeserializeObject<SocketMessage>(e.Data);

            Debug.Log(message);

            // Check if the message is a jump message
            if (message.Jump)
            {
                Debug.Log("Jump received");
                JumpMessage jumpMessage = message.Jump;


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
                socketSpeed += playermaxForce * force * playerIndex;
                Debug.Log($"Current speed: {socketSpeed}");

            }
            if (message.button != null)
            {
                Debug.Log("Button pressed");
                ButtonMessage btnMessage = message.button;
                Debug.Log(btnMessage);
            }

            // if message.inactive -> haai komt vooruit
        };
    }
}
