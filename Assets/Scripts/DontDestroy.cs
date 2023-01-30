using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    private void Awake()
    {
        GameObject socketController = GameObject.Find("SocketController");
        DontDestroyOnLoad(socketController);
    }
}
