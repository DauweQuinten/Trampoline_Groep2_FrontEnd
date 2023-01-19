using DefaultNamespace;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WebSocketSharp;


#region Custom events 

// Attribute to make the class showable in the inspector
[System.Serializable]
public class MyTestEvent : UnityEvent<Color>
{
    
}


[System.Serializable]
public class MyJumpEvent : UnityEvent<float, int>
{
    
}

#endregion


public class SocketEvents : MonoBehaviour
{
    #region Variables

    // Unity events
    public MyJumpEvent onJump;
    public MyTestEvent btnPressedLeft;
    public UnityEvent btnPressedRight;
    public UnityEvent btnPressedBoth;

    // event states
    private bool leftPressed = false;
    private bool rightPressed = false;
    private bool bothPressed = false;
    private bool playerJumped = false;

    // other variables
    private float jumpForce;
    private int player;
    
    
    // websocket
    private WebSocket ws;

    #endregion

    private void Start()
    {                  
        #region initialize events
        
        if (onJump == null)
        {
            onJump = new MyJumpEvent();
        }
      
        if (btnPressedLeft == null)
        {
            btnPressedLeft = new MyTestEvent();
        }

        if (btnPressedRight == null)
        {
            btnPressedRight = new UnityEvent();
        }
        
        if (btnPressedBoth == null)
        {
            btnPressedBoth = new UnityEvent();
        }

        #endregion
        
        #region websocket events

        // Connect to websocket
        ws = new WebSocket(General.SocketUrl);
        ws.Connect();

        // On socket connected
        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("Socket Open!");
        };

        // On message received
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message received: " + e.Data);

            // parse message to json
            var message = JsonConvert.DeserializeObject<SocketOnMessage>(e.Data);
            
            // Check message type
            if (message.Jump != null)
            {
                Debug.Log("Jump received");
                
                var jumpMessage = message.Jump;
                playerJumped = true;

                jumpForce = jumpMessage.Force;
                player = jumpMessage.Player;              
            }
            
            else if (message.Button != null)
            {            
                var btnMessage = message.Button;
                Debug.Log(btnMessage);
                var btnState = message.Button.BtnState;
                Debug.Log(btnState);

                if (btnState == BtnState.BOTH)
                {
                    bothPressed = true;
                }
                else if (btnState == BtnState.LEFT)
                {                
                    leftPressed = true;
                }
                else if (btnState == BtnState.RIGHT)
                {
                    rightPressed = true;
                }
            }
        };
        
        #endregion
    }
    
    private void Update()
    {
        #region invoke events 

        if (leftPressed)
        {
            btnPressedLeft.Invoke(Color.red);
            leftPressed = false;
        }
        if (rightPressed)
        {
            btnPressedRight.Invoke();
            rightPressed = false;
        }
        if (bothPressed)
        {
            btnPressedBoth.Invoke();
            bothPressed = false;
        }
        if (playerJumped)
        {
            Debug.Log("Player has jumped");
            onJump.Invoke(jumpForce, player);
            playerJumped = false;          
        }
        
        
        #endregion
    }

    private void OnDestroy()
    {
        ws.Close();
    }
}
