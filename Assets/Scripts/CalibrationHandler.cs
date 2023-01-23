using DefaultNamespace;
using Models;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WebSocketSharp;
using UnityEngine.SceneManagement;

public class CalibrationHandler : MonoBehaviour
{

    #region event variables

    public UnityEvent onCalibrationStarted;   
    public UnityEvent onCalibrationFinished;
    public UnityEvent onCalibratingP1;
    public UnityEvent onCalibratingP2;

    #endregion

    #region calibration variables
    
    bool[] playerCalibratedArray = { false, false };
    bool calibrationChanged = false;

    bool isCalibrationStarted = false;
    bool isCalibrationFinished = false;
    bool isCalibrating = false;

    #endregion

    #region websocket

    private WebSocket ws;

    #endregion



    
    // Start is called before the first frame update
    void Start()
    {
        #region initialize events

        onCalibrationStarted ??= new UnityEvent();
        onCalibrationFinished ??= new UnityEvent();
        onCalibratingP1 ??= new UnityEvent();
        onCalibratingP2 ??= new UnityEvent();
      
        #endregion

        #region event listeners
        
        onCalibrationStarted.AddListener(() =>
        {
            StartCalibration();
        });

        onCalibratingP1.AddListener(() =>
        {
            // Start calibration of player 0 (async)
            StartCoroutine(CalibratePlayer(0));
        });

        onCalibratingP2.AddListener(() =>
        {
            // Start calibration of player 1 (async)
            StartCoroutine(CalibratePlayer(1));
        });

        onCalibrationFinished.AddListener(() =>
        {
            CompleteCalibration();
        });

        #endregion

        #region websocket

        // Connect to websocket
        ws = new WebSocket(General.SocketUrl);
        ws.Connect();

        
        // Subscribe to events
        ws.OnMessage += (sender, e) =>
        {
            // Deserialize message
            var message = JsonConvert.DeserializeObject<SocketOnMessage>(e.Data);
            
            if(message.CalibrationChanged != null)
            {
                calibrationChanged = true;
            }
        };

        #endregion
   
        #region intro in debug

        Debug.Log("Press 'A' to start the calibration");
      
        #endregion
    }
      
    // Update is called each frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameVariablesHolder.testString = "testjee uit calibration";
            SceneManager.LoadScene("BoatGame");
        }
        


        // On start calibration (execute only once because of isCalibrationStarted)
        if (Input.GetKeyDown(KeyCode.A) && !isCalibrationStarted)
        {
            // Start calibration p1
            onCalibrationStarted.Invoke();
        }

        if ((playerCalibratedArray[0] && !playerCalibratedArray[1]) && !isCalibrating)
        {           
            changeCalibrationPlayer(1);
            onCalibratingP2.Invoke();
        }

        if (!playerCalibratedArray[0] && playerCalibratedArray[1] && !isCalibrating)
        {
            changeCalibrationPlayer(0);
            onCalibratingP1.Invoke();
        }
        
        if (playerCalibratedArray[0] && playerCalibratedArray[1] && !isCalibrationFinished)
        {
            onCalibrationFinished.Invoke();
        }
    }

     
    #region calibration handling 

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
            Debug.Log("Calibration started");
            SendCalibrationMessage(CalibrationStatus.STARTED, 0);
            onCalibratingP1.Invoke();
        }
    }
  
    void changeCalibrationPlayer(int playerIndex)
    {
        Debug.Log($"Switch to player{playerIndex}");
        SendCalibrationMessage(CalibrationStatus.SWITCH_PLAYER, playerIndex);
    }
  
    void CompleteCalibration()
    {
        isCalibrationFinished = true;
        Debug.Log("Calibration finished");
        SendCalibrationMessage(CalibrationStatus.FINISHED, 0);
    }

    #endregion


    #region calibration function

    IEnumerator CalibratePlayer(int playerIndex)
    {
        isCalibrating = true;

        Debug.Log($"Player{playerIndex} is calibrating");
        Debug.Log($"Player{playerIndex} start jumping!");

        // Debug.Log($"DEBUG: PRESS SPACE TO CONTINUE");
        // yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        yield return new WaitUntil(() => calibrationChanged);
        calibrationChanged = false;
        Debug.Log($"Almost there, Keep jumping!");
        yield return new WaitForSeconds(5);
        Debug.Log($"Well done!");
        playerCalibratedArray[playerIndex] = true;
        isCalibrating = false;
    }
    
    #endregion
    
}
