using System.Collections;
using Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UiScripts
{
    public class StartschermManager : MonoBehaviour
    {
// Start is called before the first frame update
        private UIDocument _document;
        private VisualElement _btnYellowTop;
        private VisualElement _btnBlueTop;

        private int _previousUpdateCount = -1;
        private bool _isEnabled;
        private bool _btnsHaveBeenPressed;


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
            ButtonListener.UpdateLed(LedType.Left, LedValue.On);
            ButtonListener.UpdateLed(LedType.Right, LedValue.On);
        }

        private void Update()
        {
            if (!_isEnabled || ButtonListener.BtnUpdate <= _previousUpdateCount) return;
            switch (ButtonListener.Both)
            {
                case BtnValue.Pressed:
                    _btnsHaveBeenPressed = true;
                    break;
                case BtnValue.Released:
                    // if (_btnsHaveBeenPressed) StartCoroutine(GoToNextScene(0.3f));
                    if (_btnsHaveBeenPressed) FlowHandler.LoadNextSceneInstantly("InstructionScene");
                    break;
            }

            switch (ButtonListener.Left)
            {
                case BtnValue.Pressed:
                    _btnBlueTop.AddToClassList("move-down");
                    break;
                case BtnValue.Released:
                    _btnBlueTop.RemoveFromClassList("move-down");
                    break;
            }

            switch (ButtonListener.Right)
            {
                case BtnValue.Pressed:
                    _btnYellowTop.AddToClassList("move-down");
                    break;
                case BtnValue.Released:
                    _btnYellowTop.RemoveFromClassList("move-down");
                    break;
            }

            _previousUpdateCount = ButtonListener.BtnUpdate;
        }
    }
}