using UnityEngine;
using UnityEngine.UIElements;

namespace UiScripts
{
    public class UiOverlay : MonoBehaviour
    {
        private LevelController _levelController;
        private PlayerControls _playerControls;
        private UIDocument _document;

        private VisualElement _shark;
        private VisualElement _shark2d;
        private VisualElement _boat2d;
        private Label _scoreLabel;
        private VisualElement _dial;

        // Start is called before the first frame update
        void Start()
        {
            // reference to LevelController
            _levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
            _playerControls = GameObject.Find("Boat").GetComponent<PlayerControls>();
            _document = GetComponent<UIDocument>();
            // get the shark class
            _shark = _document.rootVisualElement.Q<VisualElement>("shark");
            _scoreLabel = _document.rootVisualElement.Q<Label>("score");
            _boat2d = _document.rootVisualElement.Q<VisualElement>("boat2d");
            _shark2d = _document.rootVisualElement.Q<VisualElement>("shark2d");
            _dial = _document.rootVisualElement.Q<VisualElement>("wijzer");
        }

        // Update is called once per frame
        void Update()
        {
            // update the distance height
            UpdateDistanceToSharkGui(_levelController.distancePercentage);
            RotateDial(_playerControls.speed);
            UpdateScore();
        }

        private void UpdateScore()
        {
            var scoreSeconden = _levelController.score / 10;
            var minuuten = scoreSeconden / 60;
            var seconden = scoreSeconden % 60;
            _scoreLabel.text = $"{minuuten:D}:{seconden:D2}";
        }

        private void UpdateDistanceToSharkGui(float distance)
        {
            var height = 680 - distance * 600;
            if (height > 700) height = 700;
            _shark.style.height = height;
            _shark2d.style.top = (750 - (1 - distance) * 600);
        }

        private void RotateDial(float speed)
        {
            var angle = (speed * -2);
            Debug.Log(angle + "angle");
            _dial.style.rotate = new StyleRotate(new Rotate(angle));
        }
    }
}