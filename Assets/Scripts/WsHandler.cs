using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class WsHandler : MonoBehaviour
{

    WebSocket ws;

    void Start()
    {
        Debug.Log("Script started");

        ws = new WebSocket("ws://172.30.248.72:3000");
        ws.Connect();
        
        ws.OnMessage += (sender, e) =>
        {
            //Debug.Log($"Message received from {((WebSocket)sender).Url}");
            Debug.Log("Message received: " + e.Data);            
        };
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
