using UnityEngine;
using UnityEngine.UIElements;

namespace UiScripts
{
    public class KalibratieUi : MonoBehaviour
    {
        // Start is called before the first frame update
        private UIDocument _document;
        private Label _leftPlayerLabel;
        private Label _rightPlayerLabel;
        public string leftPlayerText;
        public string rightPlayerText;

        void Start()
        {
            _document = GetComponent<UIDocument>();
            _leftPlayerLabel = _document.rootVisualElement.Q<Label>("leftPlayerLabel");
            _rightPlayerLabel = _document.rootVisualElement.Q<Label>("rightPlayerLabel");
        }

        // Update is called once per frame
        void Update()
        {
            _leftPlayerLabel.text = leftPlayerText;
            _rightPlayerLabel.text = rightPlayerText;
        }
    }
}