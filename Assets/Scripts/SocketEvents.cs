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


// Attribute to make the class showable in the inspector
[System.Serializable]
public class MyJumpEvent : UnityEvent<float, int>
{
    
}

#endregion


public class SocketEvents : MonoBehaviour
{
    #region Variables

    // Unity events
    public UnityEvent OnJump;
    public MyTestEvent btnPressedLeft;
    public UnityEvent btnPressedRight;
    public UnityEvent btnPressedBoth;

    private bool leftPressed = false;
    
    // websocket
    private WebSocket ws;

    #endregion

    private void Start()
    {                  
        #region initialize events
        
        if (OnJump == null)
        {
            OnJump = new UnityEvent();
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

                // check which player has jumped
                if (jumpMessage.Player == 0)
                {
                    // player 1 has jumped
                }
                else if (jumpMessage.Player == 1)
                {
                    // player 2 has jumped
                }
            }
            else if (message.Button != null)
            {            
                var btnMessage = message.Button;
                Debug.Log(btnMessage);
                var btnState = message.Button.BtnState;
                Debug.Log(btnState);

                if (btnState == BtnState.BOTH)
                {
                    Debug.Log("Both buttons pressed");
                    btnPressedBoth.Invoke();
                }
                else if (btnState == BtnState.LEFT)
                {
                    Debug.Log("Left button pressed");                
                    leftPressed = true;
                }
                else if (btnState == BtnState.RIGHT)
                {
                    Debug.Log("Right button pressed");           
                    btnPressedRight.Invoke();
                }
            }
        };

        #endregion
    }

    private void Update()
    {
        if (leftPressed)
        {
            Debug.Log("Invoke event");
            btnPressedLeft.Invoke(Color.red);
            leftPressed = false;
        }
    }

    private void OnDestroy()
    {
        ws.Close();
    }
}
