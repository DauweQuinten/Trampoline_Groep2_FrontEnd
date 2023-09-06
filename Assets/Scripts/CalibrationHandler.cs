using Models;
using Newtonsoft.Json;
using System;
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

    [Header("Events")] [Space(10)] public UnityEvent onCalibrationStarted;
    public UnityEvent onCalibrationFinished;
    private UnityEvent calibrationMessageListener;
    private KalibratieUi _ui;

    #endregion

    #region calibration variables

    // Calibration states
    bool[] playerCalibratedArray = { false, false };
    bool calibrationChanged = false;
    bool isCalibrationStarted = false;
    bool isCalibrationFinished = false;
    bool isCalibrating = false;
    bool isListeningToSocket = false;
    private bool _shouldLoadScene = false;

    // Timing
    [Header("Timing variables")]
    [Space(10)]
    [SerializeField]
    [Tooltip("Delay between player detected and calibration finished")]
    [Range(0, 3)]
    float calibrationDelay = 1.5f;

    [SerializeField] [Tooltip("Delay between calibration finished and the next step")] [Range(1, 3)]
    float delayAfterCalibration = 2f;

    [SerializeField] [Tooltip("Inversed countdown speed before the game starts")] [Range(0.4f, 1f)]
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
        calibrationMessageListener ??= new UnityEvent();

        #endregion

        #region event listeners

        onCalibrationStarted.AddListener(StartCalibration);
        calibrationMessageListener.AddListener(ListenToCalibrationMessage);

        onCalibrationFinished.AddListener(CompleteCalibration);

        #endregion

        #region websocket

        // Get socket
        ws = GameObject.Find("SocketController").GetComponent<SocketEvents>().ws;

        if (!ws.IsAlive)
        {
            ws.Connect();
        }
        SendCalibrationMessage(CalibrationStatus.STARTED);


        // Subscribe to events
        ws.OnMessage += (sender, e) =>
        {
            // Deserialize message
            var message = JsonConvert.DeserializeObject<SocketOnMessage>(e.Data);
            // Debug.Log(message);
            
            if (!isListeningToSocket) return;
            calibrationChanged = true;
            if (message.Type != "kinect" || message.MessageData.Type != "calibration") return;
            if (message.MessageData.MessageData == null) return;
            
            if (message.MessageData.MessageData.Type == "players" && message.MessageData.MessageData.Value == 0)
            {
                Debug.Log("No players found");
                _ui.messageText = "Geen spelers gevonden";
                _ui.errorIconDisplayStyle = DisplayStyle.Flex;
                _ui.okIconDisplayStyle = DisplayStyle.None;
            }
            
            else if (message.MessageData.MessageData.Value == -1 && message.MessageData.MessageData.Type == "distance")
            {
                
                Debug.Log("to close together");
                _ui.messageText = "Geen spelers gevonden";
                _ui.errorIconDisplayStyle = DisplayStyle.Flex;
                _ui.okIconDisplayStyle = DisplayStyle.None;
                // to close together
            }
            else if (message.MessageData.MessageData.MiddleIndex >= 0)
            {
                _ui.messageText = "1 Speler gevonden, even wachten op de tweede speler";
                _ui.errorIconDisplayStyle = DisplayStyle.Flex;
                _ui.okIconDisplayStyle = DisplayStyle.None;
                // only one player
            }
            else if (message.MessageData.MessageData.LeftIndex >= 0 && message.MessageData.MessageData.RightIndex >= 0)
            {
                Debug.Log("Both players found");
                GameVariablesHolder.playerMapping[0] = message.MessageData.MessageData.LeftIndex;
                GameVariablesHolder.playerMapping[1] = message.MessageData.MessageData.RightIndex;
                _ui.okIconDisplayStyle = DisplayStyle.Flex;
                _ui.errorIconDisplayStyle = DisplayStyle.None;
                _ui.messageText = "2 Spelers gevonden";
                _ui.searchingText = "Het spel gaat zo beginnen";
                _shouldLoadScene = true;
                // two players
            } 
            else if (message.MessageData.MessageData.Type == "players")
            {
                // to many players
                _ui.messageText = "Te veel spelers gevonden, gelieve niet achter de trampoline te staan";
                _ui.errorIconDisplayStyle = DisplayStyle.Flex;
                _ui.okIconDisplayStyle = DisplayStyle.None;
            }
        };

        #endregion

        // start the sequence
        onCalibrationStarted.Invoke();
    }

    private void ListenToCalibrationMessage()
    {
        isListeningToSocket = true;
        
    }
    
    // main loop
    private void Update()
    {
        // check if End key is pressed
        if (Input.GetKeyDown(KeyCode.End))
        {
            CompleteCalibration();
        }
    
    
        if (!_shouldLoadScene) return;
        Debug.Log("loading scene, index 0: " + GameVariablesHolder.playerMapping[0] + " index 1: " + GameVariablesHolder.playerMapping[1]);
        CompleteCalibration();
        _shouldLoadScene = false;
    }

    #region calibration handling

    void SendCalibrationMessage(CalibrationStatus status)
    {
        // Send message: calibration started
        var calibrationMessage = new CalibrationMessage(status);

        // parse json to string with Newtonsoft.Json
        var calibrationString = JsonConvert.SerializeObject(calibrationMessage);
        // send message
        ws.Send(calibrationString);
    }

    private void StartCalibration()
    {
        {
            Debug.Log("Calibration started");

            SendCalibrationMessage(CalibrationStatus.STARTED);
            calibrationMessageListener.Invoke();
        }
    }
    
    


    private void CompleteCalibration()
    {
        isCalibrationFinished = true;
        Debug.Log("Calibration finished");

        SendCalibrationMessage(CalibrationStatus.FINISHED);
        // wait for 3 seconds
        StartCoroutine(LoadGameScene());

    }


    #endregion




    #region Load game scene

    private static IEnumerator LoadGameScene()
    {
        Debug.Log("Well done! The game starts in 3 seconds");

        yield return new WaitForSeconds(3);

        SceneManager.LoadScene("BoatGame2.0");
    }

    #endregion
}