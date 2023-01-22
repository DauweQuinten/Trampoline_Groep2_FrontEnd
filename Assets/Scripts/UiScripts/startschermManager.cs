using System.Collections;
using System.Collections.Generic;
using Models;
using UiScripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class startschermManager : MonoBehaviour
{
// Start is called before the first frame update
    private UIDocument _document;
    private VisualElement _btnYellowTop;
    private VisualElement _btnBlueTop;

    private int _previousUpdateCount = -1;
    private bool _isEnabled;


    private void Start()
    {
        _isEnabled = true;
        Debug.Log("start");
        _document = GetComponent<UIDocument>();
        var btnYellow = _document.rootVisualElement.Q("yellowButton");
        _btnYellowTop = btnYellow.Q("buttonTop");
        var btnBlue = _document.rootVisualElement.Q("blueButton");
        _btnBlueTop = btnBlue.Q("buttonTop");
        ButtonListener.ListenToButtons();
        
    }

    private void Update()
    {
        if (!_isEnabled || ButtonListener.BtnUpdate <= _previousUpdateCount) return;
        if (ButtonListener.Both == BtnValue.Pressed)
        {
            _document.rootVisualElement.Clear();
            SceneManager.LoadScene("BoatGame");
        }
        if (ButtonListener.Left == BtnValue.Pressed)
        {
            _btnBlueTop.AddToClassList("move-down");
        }
        if (ButtonListener.Right == BtnValue.Pressed)
        {
            _btnYellowTop.AddToClassList("move-down");
        }
        if (ButtonListener.Left == BtnValue.Released)
        {
            _btnBlueTop.RemoveFromClassList("move-down");
        }
        if (ButtonListener.Right == BtnValue.Released)
        {
            _btnYellowTop.RemoveFromClassList("move-down");
        }
        _previousUpdateCount = ButtonListener.BtnUpdate;
    }
}