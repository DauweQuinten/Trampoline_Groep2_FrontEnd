using System.Collections;
using Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UiScripts
{
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
            ButtonListener.UpdateLed(LedType.Left, LedValue.On);
            ButtonListener.UpdateLed(LedType.Right, LedValue.On);
        }

        private void Update()
        {
            if (!_isEnabled || ButtonListener.BtnUpdate <= _previousUpdateCount) return;
            if (ButtonListener.Both == BtnValue.Pressed)
            {
                ButtonListener.UpdateLed(LedType.Left, LedValue.Off);
                ButtonListener.UpdateLed(LedType.Right, LedValue.Off);
                StartCoroutine(LoadSceneAfterDelay(0.3f));
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

        private static IEnumerator LoadSceneAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene("CalibrationScene");
        }
    }
}