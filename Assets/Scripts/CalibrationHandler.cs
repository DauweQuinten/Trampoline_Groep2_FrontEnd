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

    #region event variables

    // events
    public UnityEvent onCalibrationStarted;
    public UnityEvent onPlayer1Calibrated;
    public UnityEvent onPlayer2Calibrated;
    public UnityEvent onCalibrationFinished;

    #endregion

    #region event states (momenteel niet in gebruik)

    private bool calibrationStartTrigger = false;
    private bool player1CalibratedTrigger = false;
    private bool Player2CalibratedTrigger = false;
    private bool calibrationFinishTrigger = false;

    #endregion

    #region calibration variables

    int calibrationTime = 5;
    bool player1Calibrated = false;
    bool player2Calibrated = false;

    #endregion

    #region websocket

    private WebSocket ws;

    #endregion


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

        #region event listeners
        
        onCalibrationStarted.AddListener(() =>
        {
            StartCalibration();
        });

        onPlayer1Calibrated.AddListener(() =>
        {
            Debug.Log("Player 1 calibrated");
            player1Calibrated = true;
            changeCalibrationPlayer();
        });

        onPlayer2Calibrated.AddListener(() =>
        {
            Debug.Log("Player 2 calibrated");
            player2Calibrated = true;
            CompleteCalibration();
        });

        onCalibrationFinished.AddListener(() =>
        {
            CompleteCalibration();
        });

        #endregion

        #region websocket

        // Connect to websocket
        ws = new WebSocket(General.SocketUrl);
        // ws.Connect();


        // Subscribe to events
        ws.OnMessage += (sender, e) =>
        {
            // Deserialize message
            var message = JsonConvert.DeserializeObject<SocketOnMessage>(e.Data);
            
            if(message.CalibrationChanged != null)
            {
                var calibrationChanged = message.CalibrationChanged;
                if(calibrationChanged.PlayerIndex == 0)
                {
                    player1Calibrated = true;
                }
                else if (calibrationChanged.PlayerIndex == 1)
                {
                    player2Calibrated = true;
                }
            }
        };

        #endregion

        #region keyboard control
        
        Debug.Log("Press 'C' to start calibration");
        
        #endregion
    }
      
    // Update is called each frame
    private void Update()
    {
        #region keyboard control

        // On start calibration
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Start calibration
            onCalibrationStarted.Invoke();
        }

        #endregion
    }


    #region calibration methods

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

    void StartCalibration()
    {
        {
            Debug.Log("Calibration has started");
            Debug.Log("First calibration: player 1");
            // SendCalibrationMessage(CalibrationStatus.STARTED, 0);
            StartCoroutine(WaitForPlayer1(calibrationTime));
        }
    }
  
    void changeCalibrationPlayer()
    {
        Debug.Log("Switch to player 2");
        // SendCalibrationMessage(CalibrationStatus.SWITCH_PLAYER, 1);
        StartCoroutine(WaitForPlayer2(calibrationTime));
    }
  
    void CompleteCalibration()
    {
        Debug.Log("Calibration has finished");
        //SendCalibrationMessage(CalibrationStatus.FINISHED, 0);
    }

    #endregion


    #region calibration timers

    IEnumerator WaitForPlayer1(int waitTime)
    {      
        Debug.Log("Speler 1 spring om links te roeien");
        
        int currentTime = 0;
        while (!player1Calibrated)
        {           
            if (currentTime < waitTime)
            {
                currentTime++;
                Debug.Log($"Calibration done in {waitTime - currentTime} seconds");
                yield return new WaitForSeconds(1);
            }
            else
            {              
                
                onPlayer1Calibrated.Invoke();
            }
        }               
    }

    IEnumerator WaitForPlayer2(int waitTime)
    {
        Debug.Log("Speler 2 spring om rechts te roeien");
        
        int currentTime = 0;

        while (!player2Calibrated)
        {
            if (currentTime < waitTime)
            {
                currentTime++;
                Debug.Log($"Calibration done in {waitTime - currentTime} seconds");
                yield return new WaitForSeconds(1);
            }
            else
            {
                onPlayer2Calibrated.Invoke();
            }
        }
    }

    #endregion
}
