using UnityEngine;
using UnityEngine.UIElements;

namespace UiScripts
{
    public class UiOverlay : MonoBehaviour
    {
        private LevelController _levelController;
        private UIDocument _document;

        private VisualElement _shark;

        private Label _scoreLabel;

        // Start is called before the first frame update
        void Start()
        {
            // reference to LevelController
            _levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
            _document = GetComponent<UIDocument>();
            // get the shark class
            _shark = _document.rootVisualElement.Q<VisualElement>("shark");
            _scoreLabel = _document.rootVisualElement.Q<Label>("score");
        }

        // Update is called once per frame
        void Update()
        {
            // update the distance height
            UpdateDistanceToSharkGui(_levelController.distancePercentage);
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
            var height = 760 - distance * 700;
            if (height > 700) height = 700;
            _shark.style.height = height;
        }
    }
}