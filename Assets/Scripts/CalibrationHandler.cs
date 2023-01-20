using DefaultNamespace;
using Models;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WebSocketSharp;

public class CalibrationHandler : MonoBehaviour
{
    // events
    public UnityEvent onCalibrationStarted;
    public UnityEvent onPlayer1Calibrated;
    public UnityEvent onPlayer2Calibrated;
    public UnityEvent onCalibrationFinished;

    // event states
    private bool calibrationStartTrigger = false;
    private bool player1CalibratedTrigger = false;
    private bool Player2CalibratedTrigger = false;
    private bool calibrationFinishTrigger = false;

    
    // websocket
    private WebSocket ws;

    // calibration states
    bool player1Calibrated = false;
    bool player2Calibrated = false;

    // Start is called before the first frame update
    void Start()
    {
        #region initialize events

        if (onCalibrationStarted == null)
        {
            onCalibrationStarted = new UnityEvent();
        }
        if (onPlayer1Calibrated == null)
        {
            onPlayer1Calibrated = new UnityEvent();
        }
        if (onPlayer2Calibrated == null)
        {
            onPlayer2Calibrated = new UnityEvent();
        }
        if (onCalibrationFinished == null)
        {
            onCalibrationFinished = new UnityEvent();
        }

        #endregion

        // Connect to websocket
        ws = new WebSocket(General.SocketUrl);
        ws.Connect();


        // Subscribe to events
        ws.OnMessage += (sender, e) =>
        {
            // Deserialize message
            var message = JsonConvert.DeserializeObject<SocketOnMessage>(e.Data);

            // Check if message is a calibration message
            if (true) // If calibration p1 completed
            {
                player1CalibratedTrigger = true;               
            }

            if (true) // If calibration p2 completed
            {
                Player2CalibratedTrigger = true;
            }          
        };
    }

    private void Update()
    {
        // On start calibration
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Start calibration
            onCalibrationStarted.Invoke();
        }

        if (player1CalibratedTrigger)
        {
            onPlayer1Calibrated.Invoke();
            player1CalibratedTrigger = false;
        }

        if (Player2CalibratedTrigger)
        {
            onPlayer2Calibrated.Invoke();
            Player2CalibratedTrigger = false;
        }

        if (player1Calibrated && player2Calibrated)
        {
            onCalibrationFinished.Invoke();
            
            // reset calibration states
            player1Calibrated = false;
            player2Calibrated = false;
        }
    }
    
    
    void SendCalibrationMessage(CalibrationStatus status, int player)
    {
        // Send message: calibration started
        CalibrationMessage calibrationMessage = new CalibrationMessage(status, player);

        // parse json to string with Newtonsoft.Json
        string calibrationString = JsonConvert.SerializeObject(calibrationMessage);
        Debug.Log(calibrationString);
        
        // send message
        ws.Send(calibrationString);
    }

    void CompleteCalibration()
    {
        SendCalibrationMessage(CalibrationStatus.FINISHED, 0);
    }      
}
