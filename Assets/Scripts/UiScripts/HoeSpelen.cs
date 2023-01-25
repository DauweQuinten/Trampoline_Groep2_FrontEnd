using System.Collections;
using Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UiScripts
{
    public class HoeSpelen : MonoBehaviour
    {
        // Start is called before the first frame update
        private UIDocument _document;
        private int _buttonCounter = -1;
        private VisualElement _yellowButtonTop;
        private bool _rightButtonHasBeenReleased;

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
            switch (ButtonListener.Right)
            {
                case BtnValue.Pressed:
                    _yellowButtonTop.AddToClassList("move-down");
                    if (!_rightButtonHasBeenReleased) break;
                    StartCoroutine(GoToNextScene(0.3f));
                    break;
                case BtnValue.Released:
                    _yellowButtonTop.RemoveFromClassList("move-down");
                    _rightButtonHasBeenReleased = true;
                    break;
            }

            _buttonCounter = ButtonListener.BtnUpdate;
        }

        private IEnumerator GoToNextScene(float delay)
        {
            ButtonListener.UpdateLed(LedType.Right, LedValue.Off);
            ButtonListener.UpdateLed(LedType.Left, LedValue.Off);
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene("Instructies2");
        }
    }
}