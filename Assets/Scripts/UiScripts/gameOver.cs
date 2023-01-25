using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UiScripts
{
    public class gameOver : MonoBehaviour
    {
        private UIDocument _document;
        private VisualElement _btnYellowTop;

        private int _updateCount;
        private bool _rightHasBeenPressed = false;

        // Start is called before the first frame update
        void Start()
        {
            _document = GetComponent<UIDocument>();
            var labelScore = _document.rootVisualElement.Q<Label>("score");
            var scoreInSeconden = GameVariablesHolder.Score / 10;
            var minuuten = scoreInSeconden / 60;
            var seconden = scoreInSeconden % 60;
            labelScore.text = $"{minuuten}:{seconden}";

            // handle button
            var btnYellow = _document.rootVisualElement.Q("yellowButton");
            _btnYellowTop = btnYellow.Q("buttonTop");
            ButtonListener.ListenToButtons();
            ButtonListener.UpdateLed(LedType.Right, LedValue.On);
            ButtonListener.UpdateLed(LedType.Left, LedValue.Off);
        }

        // Update is called once per frame
        void Update()
        {
            if (ButtonListener.BtnUpdate <= _updateCount) return;
            switch (ButtonListener.Right)
            {
                case BtnValue.Pressed:
                    _btnYellowTop.AddToClassList("move-down");
                    _rightHasBeenPressed = true;
                    break;
                case BtnValue.Released:
                    _btnYellowTop.RemoveFromClassList("move-down");
                    if (_rightHasBeenPressed) StartCoroutine(LoadSceneAfterDelay(0.3f));
                    break;
            }
        }

        private static IEnumerator LoadSceneAfterDelay(float delay)
        {
            ButtonListener.UpdateLed(LedType.Right, LedValue.Off);
            ButtonListener.UpdateLed(LedType.Left, LedValue.Off);
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene("username-kiezen");
        }
    }
}