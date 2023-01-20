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
    public UnityEvent btnReleasedLeft;
    public UnityEvent btnReleasedRight;
    public UnityEvent btnReleasedBoth;

    private bool leftPressed = false;
    
    // websocket
    private WebSocket ws;

    #endregion

    private void Start()
    {                  
        #region initialize events
        
        OnJump ??= new UnityEvent();
        btnPressedLeft ??= new MyTestEvent();
        btnPressedRight ??= new UnityEvent();
        btnPressedBoth ??= new UnityEvent();
        btnReleasedLeft ??= new UnityEvent();
        btnReleasedRight ??= new UnityEvent();
        btnReleasedBoth ??= new UnityEvent();
    
        #endregion

        #region previousButton
        var previousButtonState = new bool[3];
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
                var btnMessage = message.Button.BtnStates;
                
                if(previousButtonState[2] && !btnMessage[2])
                    btnReleasedBoth.Invoke();

                if (btnMessage[0] && btnMessage[1])
                {
                    Debug.Log("both buttons pressed");
                    btnPressedBoth.Invoke();
                }

                else if (btnMessage[0])
                {
                    Debug.Log("left button pressed");
                    btnPressedLeft.Invoke(Color.red);
                }
                else if (!btnMessage[0])
                {
                    Debug.Log("left button no longer pressed");
                    btnReleasedLeft.Invoke();
                }
                else if (btnMessage[1])
                {
                    Debug.Log("right button pressed");
                    btnPressedRight.Invoke();
                }
                else if (!btnMessage[1])
                {
                    Debug.Log("right button no longer pressed");
                    btnReleasedRight.Invoke();
                }
                
                previousButtonState = btnMessage;
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
