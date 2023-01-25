using Models;
using UnityEngine;
using UnityEngine.UIElements;

namespace UiScripts
{
    public class HoeSturen : MonoBehaviour
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
        }
    }
}