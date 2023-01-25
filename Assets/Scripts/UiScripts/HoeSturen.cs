using System.Collections;
using Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UiScripts
{
    public class HoeSturen : MonoBehaviour
    {
        // Start is called before the first frame update
        private UIDocument _document;
        private int _buttonCounter = -1;
        private VisualElement _yellowButtonTop;
        private bool _rightButtonWasPressed = false;

        void Start()
        {
            _document = GetComponent<UIDocument>();
            _yellowButtonTop = _document.rootVisualElement.Q("yellowButton").Q<VisualElement>("buttonTop");
            ButtonListener.ListenToButtons();
            ButtonListener.UpdateLed(LedType.Right, LedValue.On);
            ButtonListener.UpdateLed(LedType.Left, LedValue.Off);
        }

        // Update is called once per frame
        void Update()
        {
            if (_buttonCounter >= ButtonListener.BtnUpdate) return;
            _buttonCounter = ButtonListener.BtnUpdate;
            switch (ButtonListener.Right)
            {
                case BtnValue.Pressed:
                    _rightButtonWasPressed = true;
                    _yellowButtonTop.AddToClassList("move-down");
                    break;
                case BtnValue.Released:
                    _yellowButtonTop.RemoveFromClassList("move-down");
                    if (_rightButtonWasPressed) StartCoroutine(GoToNextScene(0.3f));
                    break;
            }
        }

        private IEnumerator GoToNextScene(float delay)
        {
            ButtonListener.UpdateLed(LedType.Right, LedValue.Off);
            ButtonListener.UpdateLed(LedType.Left, LedValue.Off);
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene("CalibrationScene");
        }
    }
}