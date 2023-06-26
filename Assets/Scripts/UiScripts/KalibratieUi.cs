using UnityEngine;
using UnityEngine.UIElements;

namespace UiScripts
{
    public class KalibratieUi : MonoBehaviour
    {
        // Start is called before the first frame update
        private UIDocument _document;
        private Label _messageLabel;
        private Label _searchingLabel;
        private VisualElement _okIcon;
        private VisualElement _errorIcon;
        public DisplayStyle okIconDisplayStyle = DisplayStyle.None;
        public DisplayStyle errorIconDisplayStyle = DisplayStyle.None;
        public string messageText;
        public string searchingText = "Zoeken naar spelers...";

        private void Start()
        {
            _document = GetComponent<UIDocument>();
            _messageLabel = _document.rootVisualElement.Q<Label>("messageLabel");
            _okIcon = _document.rootVisualElement.Q<VisualElement>("okayIcon");
            _errorIcon = _document.rootVisualElement.Q<VisualElement>("errorIcon");
            _searchingLabel = _document.rootVisualElement.Q<Label>("searchingLabel");
            _okIcon.style.display = DisplayStyle.None;
            _errorIcon.style.display = DisplayStyle.None;
        }

        // Update is called once per frame
        private void Update()
        {
            if (_messageLabel.text == messageText && _okIcon.style.display == okIconDisplayStyle &&
                _errorIcon.style.display == errorIconDisplayStyle && _searchingLabel.text == searchingText) return;
            _messageLabel.text = messageText;
            _okIcon.style.display = okIconDisplayStyle;
            _errorIcon.style.display = errorIconDisplayStyle;
            _searchingLabel.text = searchingText;
        }
    }
}