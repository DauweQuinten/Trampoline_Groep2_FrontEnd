using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class startschermManager : MonoBehaviour
{
// Start is called before the first frame update
    private UIDocument _document;
    private VisualElement _btnYellowTop;
    private VisualElement _btnBlueTop;

    private bool _leftDown;
    private bool _rightDown;
    private bool _destroy;
    private bool _btnUpdate;
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
        ListenToButtons();
        
    }

    private void Update()
    {
        if (_btnUpdate && _isEnabled)
        {
            _btnUpdate = false;
            if (_leftDown)
            {
                _btnBlueTop.AddToClassList("move-down");
            }
            else
            {
                _btnBlueTop.RemoveFromClassList("move-down");
            }

            if (_rightDown)
            {
                _btnYellowTop.AddToClassList("move-down");
            }
            else
            {
                _btnYellowTop.RemoveFromClassList("move-down");
            }

            if (_destroy)
            {
                _document.rootVisualElement.Clear();
                SceneManager.LoadScene("BoatGame");
            }
        }
    }

    private void ListenToButtons()
    {
        // link to socketController Events
        var socketObject = GameObject.Find("SocketController");
        var events = socketObject.GetComponent<SocketEvents>();
        events.btnPressedLeft2.AddListener(LeftMoveDown);
        events.btnPressedLeft.AddListener(LeftMoveDown2);
        events.btnPressedRight.AddListener(RightMoveDown);
        events.btnReleasedLeft.AddListener(LeftMoveUp);
        events.btnReleasedRight.AddListener(RightMoveUp);
        events.btnPressedBoth.AddListener(BothButtonsOnClicked);
    }

    private void LeftMoveDown2(Color arg0)
    {
        _leftDown = true;
        _btnUpdate = true;
    }

    private void RightMoveUp()
    {
        _rightDown = false;
        _btnUpdate = true;
    }

    private void LeftMoveUp()
    {
        _leftDown = false;
        _btnUpdate = true;

    }

    private void RightMoveDown()
    {
        _rightDown = true;
        _btnUpdate = true;

    }

    private void LeftMoveDown()
    {
        _leftDown = true;
        _btnUpdate = true;

    }

    private void BothButtonsOnClicked()
    {
        _destroy = true;
        _btnUpdate = true;

    }
}
