using Models;
using Newtonsoft.Json;
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
    public MyJumpEvent OnJump;
    public MyTestEvent btnPressedLeft;
    public UnityEvent btnPressedLeft2;
    public UnityEvent btnPressedRight;
    public UnityEvent btnPressedBoth;
    public UnityEvent btnReleasedLeft;
    public UnityEvent btnReleasedRight;
    public UnityEvent btnReleasedBoth;
    public UnityEvent ledLeftOn;
    public UnityEvent ledLeftOff;
    public UnityEvent ledRightOn;
    public UnityEvent ledRightOff;

    private bool leftPressed = false;
    private bool rightPressed = false;
    private bool bothPressed = false;
    private bool playerJumped = false;

    private float jumpForce = 0;
    private int player = 0;


    // websocket
    public WebSocket ws;

    #endregion

    private void Start()
    {
        #region initialize events

        OnJump ??= new MyJumpEvent();
        btnPressedLeft ??= new MyTestEvent();
        btnPressedLeft2 ??= new UnityEvent();
        btnPressedRight ??= new UnityEvent();
        btnPressedBoth ??= new UnityEvent();
        btnReleasedLeft ??= new UnityEvent();
        btnReleasedRight ??= new UnityEvent();
        btnReleasedBoth ??= new UnityEvent();
        ledLeftOn ??= new UnityEvent();
        ledLeftOff ??= new UnityEvent();
        ledRightOn ??= new UnityEvent();
        ledRightOff ??= new UnityEvent();

        #endregion

        #region previousButton

        var previousButtonState = new bool[3];

        #endregion

        #region websocket events

        // Connect to websocket
        ws = new WebSocket(General.SocketUrl);

        if (!ws.IsAlive)
        {
            ws.Connect();
        }

        // On socket connected
        ws.OnOpen += (sender, e) => { Debug.Log("Socket Open!"); };


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
                var btnMessage = message.Button.BtnStates;
                Debug.Log($"{btnMessage.BtnRight}, {btnMessage.BtnLeft}, {btnMessage.Both} ");
                if (btnMessage.Both == BtnValue.Pressed)
                {
                    Debug.Log("Both");
                    btnPressedBoth.Invoke();
                }

                if (btnMessage.Both == BtnValue.Released)
                {
                    Debug.Log("both released");
                    btnReleasedBoth.Invoke();
                }

                if (btnMessage.BtnLeft == BtnValue.Pressed)
                {
                    Debug.Log("Left");
                    // btnPressedLeft.Invoke(Color.red);
                    btnPressedLeft2.Invoke();
                }

                if (btnMessage.BtnLeft == BtnValue.Released)
                {
                    Debug.Log("left released");
                    btnReleasedLeft.Invoke();
                }

                if (btnMessage.BtnRight == BtnValue.Pressed)
                {
                    Debug.Log("Right");
                    btnPressedRight.Invoke();
                }

                if (btnMessage.BtnRight == BtnValue.Released)
                {
                    Debug.Log("right released");
                    btnReleasedRight.Invoke();
                }
            }
        };

        ListenToLedEvents();

        #endregion
    }

    private void ListenToLedEvents()
    {
        ledLeftOn.AddListener(() => { SendLedToSocket(LedType.Left, LedValue.On); });
        ledLeftOff.AddListener(() => { SendLedToSocket(LedType.Left, LedValue.Off); });
        ledRightOn.AddListener(() => { SendLedToSocket(LedType.Right, LedValue.On); });
        ledRightOff.AddListener(() => { SendLedToSocket(LedType.Right, LedValue.Off); });
    }

    private void SendLedToSocket(LedType ledType, LedValue ledState)
    {
        var ledMessage = new LedMessage
        {
            Id = ledType,
            Led = ledState
        };
        var socketMessage = new SocketLedMessage
        {
            LedMessage = ledMessage
        };
        var json = JsonConvert.SerializeObject(socketMessage);
        Debug.Log(json);
        ws.Send(json);
    }


    private void Update()
    {
        if (playerJumped)
        {
            Debug.Log("Player has jumped");
            OnJump.Invoke(jumpForce, player);
            playerJumped = false;
        }

        if (leftPressed)
        {
            Debug.Log("Invoke event");
            btnPressedLeft.Invoke(Color.red);
            btnPressedLeft2.Invoke();
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
    }

    private void OnDestroy()
    {
        if (ws.IsAlive)
        {
            ws.Close();
        }
    }
}