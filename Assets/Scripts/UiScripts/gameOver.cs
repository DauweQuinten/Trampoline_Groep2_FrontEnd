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

        // Start is called before the first frame update
        void Start()
        {
            _document = GetComponent<UIDocument>();
            var labelScore = _document.rootVisualElement.Q<Label>("score");
            labelScore.text = "1:51";
            // labelScore.text = "Score: " + ScoreModel.Score;

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
                    ButtonListener.UpdateLed(LedType.Right, LedValue.Off);
                    // wait for .3s
                    StartCoroutine(LoadSceneAfterDelay(0.3f));
                    // move to next screen after animation
                    break;
                case BtnValue.Released:
                    _btnYellowTop.RemoveFromClassList("move-down");
                    break;
            }
        }

        private static IEnumerator LoadSceneAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene("username-kiezen");
        }
    }
}