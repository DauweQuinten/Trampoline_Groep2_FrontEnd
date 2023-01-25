using Models;
using Newtonsoft.Json;
using System.Collections;
using UiScripts;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using WebSocketSharp;

public class CalibrationHandler : MonoBehaviour
{
    #region event variables

    public UnityEvent onCalibrationStarted;
    public UnityEvent onCalibrationFinished;
    public UnityEvent onCalibratingP1;
    public UnityEvent onCalibratingP2;
    private KalibratieUi _ui;

    #endregion

    #region calibration variables

    bool[] playerCalibratedArray = { false, false };
    // int[] playerMapping;

    bool calibrationChanged = false;

    bool isCalibrationStarted = false;
    bool isCalibrationFinished = false;
    bool isCalibrating = false;
    bool calibrationHasChanged = false;

    #endregion

    #region websocket

    private WebSocket ws;

    #endregion


    // Start is called before the first frame update
    void Awake()
    {
        // make reference to KalibratieUi
        _ui = GameObject.Find("calibration").GetComponent<KalibratieUi>();
        if (_ui == null) Debug.LogError("KalibratieUi not found");

        #region initialize events

        onCalibrationStarted ??= new UnityEvent();
        onCalibrationFinished ??= new UnityEvent();
        onCalibratingP1 ??= new UnityEvent();
        onCalibratingP2 ??= new UnityEvent();

        #endregion

        #region event listeners

        onCalibrationStarted.AddListener(() => { StartCalibration(); });

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

        onCalibrationFinished.AddListener(() => { CompleteCalibration(); });

        #endregion

        #region websocket

        // Get socket
        ws = GameObject.Find("SocketController").GetComponent<SocketEvents>().ws;

        if (!ws.IsAlive)
        {
            ws.Connect();
        }

        // Subscribe to events
        ws.OnMessage += (sender, e) =>
        {
            // Deserialize message
            var message = JsonConvert.DeserializeObject<SocketOnMessage>(e.Data);

            if (message.CalibrationChanged != null)
            {
                calibrationChanged = true;

                if (!playerCalibratedArray[0])
                {
                    Debug.Log("Left player index changed to " + message.CalibrationChanged.KinectIndex);
                    GameVariablesHolder.playerMapping[0] = message.CalibrationChanged.KinectIndex;
                    Debug.Log($"mapping is now: {GameVariablesHolder.playerMapping[0]}");
                }
                else if (!playerCalibratedArray[1])
                {
                    Debug.Log("Right player index changed to " + message.CalibrationChanged.KinectIndex);
                    GameVariablesHolder.playerMapping[1] = message.CalibrationChanged.KinectIndex;
                    Debug.Log($"mapping is now: {GameVariablesHolder.playerMapping[1]}");
                }
            }
        };

        #endregion

        #region intro in debug

        Debug.Log("Press 'A' to start the calibration");

        #endregion

        // start the sequence
        onCalibrationStarted.Invoke();
    }

    // Update is called each frame
    private void Update()
    {
        // On start calibration (execute only once because of isCalibrationStarted)

        if ((playerCalibratedArray[0] && !playerCalibratedArray[1]) && !isCalibrating)
        {
            if(calibrationHasChanged == true)
            {
                changeCalibrationPlayer(1);
                onCalibratingP2.Invoke();
                calibrationHasChanged = false;
            }
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
        Debug.Log($"Well done, left player is ready to go!");
        SendTextToUi("Goed gedaan, je bent klaar om te gaan!", 0);
        Debug.Log($"Right player, are you ready?");
        SendTextToUi("Ben je klaar?", 1);
  
        float currentSeconds = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < currentSeconds + 3)
        {
            SendTextToUi("I'm waitingg?", 0);
        }

        Debug.Log($"Switch to player{playerIndex}");
        SendTextToUi("Start met springen!", 1);
        SendCalibrationMessage(CalibrationStatus.SWITCH_PLAYER, playerIndex);
        calibrationHasChanged = true;
    }

    void CompleteCalibration()
    {
        isCalibrationFinished = true;
        Debug.Log("Calibration finished");
        SendCalibrationMessage(CalibrationStatus.FINISHED, 0);
        StartCoroutine(LoadGameScene());
    }

    #endregion

    #region calibration function

    IEnumerator CalibratePlayer(int playerIndex)
    {
        isCalibrating = true;

        Debug.Log($"Player{playerIndex} is calibrating");
        SendTextToUi("start met springen!", playerIndex);
        Debug.Log($"Player{playerIndex} start jumping!");
        // Debug.Log($"DEBUG: PRESS SPACE TO CONTINUE");
        // yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        yield return new WaitUntil(() => calibrationChanged);
        calibrationChanged = false;
        Debug.Log($"Almost there, Keep jumping!");
        SendTextToUi("Je bent er bijna!", playerIndex);
        yield return new WaitForSeconds(5);
        Debug.Log($"Well done!");
        SendTextToUi("Goed gedaan!", playerIndex);
        playerCalibratedArray[playerIndex] = true;
        isCalibrating = false;
    }

    private void SendTextToUi(string text, int playerIndex)
    {
        if (playerIndex == 0)
        {
            _ui.leftPlayerText = text;
        }
        else
        {
            _ui.rightPlayerText = text;
        }
    }

    #endregion

    #region Load game scene

    IEnumerator LoadGameScene()
    {
        Debug.Log("Well done! The game starts in 5 seconds");
        SendTextToUi("Het spel begint over 5 seconden!", 0);
        SendTextToUi("Het spel begint over 5 seconden!", 1);
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("BoatGame2.0");
    }

    #endregion
}