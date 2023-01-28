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


    [Header("Events")]
    [Space(10)]

    public UnityEvent onCalibrationStarted;
    public UnityEvent onCalibrationFinished;
    public UnityEvent onCalibratingP1;
    public UnityEvent onCalibratingP2;
    private KalibratieUi _ui;

    #endregion

    #region calibration variables

    bool[] playerCalibratedArray = { false, false };

    bool calibrationChanged = false;

    bool isCalibrationStarted = false;
    bool isCalibrationFinished = false;
    bool isCalibrating = false;
    bool isListeningToSocket = false;


    [Header("Timing variables")]
    [Space(10)]

    [SerializeField]
    [Tooltip("Delay between player detected and calibration finished")]
    [Range(0, 3)]
    float calibrationDelay = 1.5f;

    [SerializeField]
    [Tooltip("Delay between calibration finished and the next step")]
    [Range(1, 3)]
    float delayAfterCalibration = 2f;

    [SerializeField]
    [Tooltip("Inversed countdown speed before the game starts")]
    [Range(0.4f, 1f)]
    float countdownSpeed = 0.6f;

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
            // show text for user instructions
            SendTextToUi("Even wachten, speler 1 is aan de beurt", 1);

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

            if (message.CalibrationChanged != null && isListeningToSocket)
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

        // start the sequence
        onCalibrationStarted.Invoke();
    }

    // Update is called each frame
    private void Update()
    {
        // On start calibration (execute only once because of isCalibrationStarted)

        if ((playerCalibratedArray[0] && !playerCalibratedArray[1]) && !isCalibrating)
        {
            changeCalibrationPlayer(1);           
            onCalibratingP2.Invoke();
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
        SendTextToUi("Je bent klaar om te gaan!", 0);
        Debug.Log($"Right player, are you ready?");
        SendTextToUi("Ben je klaar?", 1);
        Debug.Log($"Switch to player{playerIndex}");
        SendTextToUi("Start met springen!", 1);
        SendCalibrationMessage(CalibrationStatus.SWITCH_PLAYER, playerIndex);
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
        // Set calibration to true -> start listening to calibrationChanged messages
        isCalibrating = true;
        isListeningToSocket = true;

        Debug.Log($"Player{playerIndex} is calibrating");
        
        // send messages to the user
        SendTextToUi("start met springen!", playerIndex);
        Debug.Log($"Player{playerIndex} start jumping!");

        // Wait for calibrationChanged message -> calibration of player index is finished
        yield return new WaitUntil(() => calibrationChanged);

        // Keep jumping for a few seconds -> calibration of min & max jump 
        Debug.Log($"Almost there, Keep jumping!");
        SendTextToUi("Je bent er bijna!", playerIndex);
        yield return new WaitForSeconds(calibrationDelay);

        // calibration is finished
        Debug.Log($"Well done!");

        // reset the calibration states -> stop listening to calibrationChanged messages
        isListeningToSocket = false;
        calibrationChanged = false;

        // send feedback to the user en wait for a few seconds
        SendTextToUi("Goed gedaan! Stop met springen", playerIndex);
        yield return new WaitForSeconds(delayAfterCalibration);

        // set playerCalibrated to true
        playerCalibratedArray[playerIndex] = true;
        isCalibrating = false;
    }

    #endregion

    #region UI handler

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
        
        Debug.Log("Well done! The game starts in 3 seconds");

        int time = 3;
        
        while(time >= 0)
        {         
            SendTextToUi(time.ToString(), 0);
            SendTextToUi(time.ToString(), 1);
            yield return new WaitForSeconds(1);
            time--;         
        }
               
        SceneManager.LoadScene("BoatGame2.0");
    }

    #endregion
}