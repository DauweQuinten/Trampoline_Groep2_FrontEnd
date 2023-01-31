using System.Collections.Generic;
using System.Linq;
using Models;
using Repository;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UiScripts
{
    public class scoreboard : MonoBehaviour
    {
        private List<ScoreboardItem> _listItems;
        private ScoreboardItem _userItem;
        private UIDocument _document;
        private List<VisualElement> _listviewItems = new();

        private VisualElement _btnYellowTop;
        private VisualElement _btnBlueTop;

        private VisualElement _root;
        private int _previousUpdateCount = -1;
        private bool _isEnabled;
        private bool _buttonLeftIngedrukt;
        private bool _buttonRightIngedrukt;


        public void Start()
        {
            if (GameVariablesHolder.Id < 2) GameVariablesHolder.Id = 21;
            _isEnabled = true;
            _document = GetComponent<UIDocument>();
            // Each editor window contains a root VisualElement object
            var btnYellow = _document.rootVisualElement.Q("yellowButton");
            _btnYellowTop = btnYellow.Q("buttonTop");
            var btnBlue = _document.rootVisualElement.Q("blueButton");
            _btnBlueTop = btnBlue.Q("buttonTop");
            ButtonListener.ListenToButtons();
            ButtonListener.UpdateLed(LedType.Left, LedValue.On);
            ButtonListener.UpdateLed(LedType.Right, LedValue.On);
            _root = _document.rootVisualElement;
            FillBoard();
            // reset kinect
            var socketObject = GameObject.Find("SocketController");
            var socket = socketObject.GetComponent<SocketEvents>();
            socket.ResetKinect();
        }


        private async void FillBoard()
        {
            _listItems = await ScoreRepository.GetScoresAsync();
            _userItem = await ScoreRepository.GetScoreAsync(GameVariablesHolder.Id);
            _listItems.Sort();

            foreach (ScoreboardItem item in _listItems.Take(9).ToList())
            {
                var imgByteArrayScoreList = await ScoreRepository.GetAvatar(item.Id);
                var textureList = new Texture2D(1, 1);
                textureList.LoadImage(imgByteArrayScoreList);
                item.Img = textureList;
            }

            var imgByteArray = await ScoreRepository.GetAvatar(GameVariablesHolder.Id);
            var texture = new Texture2D(1, 1);
            texture.LoadImage(imgByteArray);
            _userItem.Img = texture;

            // _userItem.Img = await GetRemoteTexture(_userItem.ImgUrl);
            ListView lvwScores = _root.Q<ListView>("lvwScores");

            FillList(lvwScores, _listItems.Take(9).ToList());

            var scoreInSeconds = _userItem.Score / 10;
            var seconds = scoreInSeconds % 60;
            var minutes = scoreInSeconds / 60;
            var milliSeconds = _userItem.Score % 10;

            _root.Q<Image>("lblUserImage").style.backgroundImage = texture;
            _root.Q<Label>("thisUserScore").text = $"{minutes:D}:{seconds:D2}.{milliSeconds:D1}";
            _root.Q<Label>("lblUserScore").text = _userItem.Username;

            var userNumber = _listItems.IndexOf(_userItem) + 1;
            var userPosition = $"{userNumber}e plaats";

            _root.Q<Label>("lblUserPosition").text = userPosition;

            foreach (var item in _listviewItems)
            {
                if (item.ClassListContains($"Id{_userItem.Id}"))
                {
                    item.AddToClassList("c-user-list");
                }
            }
        }

        private void FillList(ListView list, List<ScoreboardItem> items)
        {
            list.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;
            list.fixedItemHeight = 62;
            list.makeItem = MakeItem;
            list.bindItem = BindItem;
            list.itemsSource = items;
        }

        private VisualElement MakeItem()
        {
            //Here we take the uxml and make a VisualElement
            VisualElement listItem = new VisualElement();
            listItem.AddToClassList("c-score-list-item");

            var leftAlignement = new VisualElement
            {
                name = "leftAlignment"
            };
            leftAlignement.AddToClassList("row");
            listItem.Add(leftAlignement);

            var number = new Label { name = "score-number" };
            number.style.width = 35;
            number.AddToClassList("outline-text");
            number.AddToClassList("outline-text-sm");
            leftAlignement.Add(number);

            var i = new Image { name = "score-image" };
            i.AddToClassList("c-score-image");
            i.AddToClassList("u-list-img");
            leftAlignement.Add(i);

            var l = new Label { name = "score-name-label" };
            l.style.maxWidth = 200;
            l.AddToClassList("outline-text");
            l.AddToClassList("outline-text-sm");
            l.AddToClassList("u-list-label");
            leftAlignement.Add(l);

            var s = new Label { name = "score-number-label" };
            s.style.maxWidth = 200;
            s.AddToClassList("outline-text");
            s.AddToClassList("outline-text-sm");
            listItem.Add(s);
            _listviewItems.Add(listItem);
            return listItem;
        }

        private void BindItem(VisualElement e, int i)
        {
            e.AddToClassList($"Id{_listItems[i].Id}");

            e.Q<Label>("score-number").text = $"{i + 1}.";

            e.Q<Label>("score-name-label").text = $"{_listItems[i].Username}";
            var scoreInSeconds = _listItems[i].Score / 10;
            var seconds = scoreInSeconds % 60;
            var minutes = scoreInSeconds / 60;
            var milliSeconds = _listItems[i].Score % 10;

            e.Q<Label>("score-number-label").text = $"{minutes:D}:{seconds:D2}.{milliSeconds:D1}";

            e.Q<Image>("score-image").style.backgroundImage = _listItems[i].Img;
        }


        private void Update()
        {
            if (!_isEnabled || ButtonListener.BtnUpdate <= _previousUpdateCount) return;
            if (ButtonListener.Both == BtnValue.Pressed)
            {
                _document.rootVisualElement.Clear();
                SceneManager.LoadScene("Startscherm");
            }

            switch (ButtonListener.Left)
            {
                case BtnValue.Pressed:
                    _buttonLeftIngedrukt = true;
                    _btnBlueTop.AddToClassList("move-down");
                    break;
                case BtnValue.Released:
                    _btnBlueTop.RemoveFromClassList("move-down");
                    // if (_buttonLeftIngedrukt) StartCoroutine(GoToStartScene(0.3f));
                    if (_buttonLeftIngedrukt) FlowHandler.LoadNextSceneInstantly("CalibrationScene");
                    break;
            }

            switch (ButtonListener.Right)
            {
                case BtnValue.Pressed:
                    _buttonRightIngedrukt = true;
                    _btnYellowTop.AddToClassList("move-down");
                    break;
                case BtnValue.Released:
                    _btnYellowTop.RemoveFromClassList("move-down");
                    // if (_buttonRightIngedrukt) StartCoroutine(PlayAgain());
                    if (_buttonRightIngedrukt) FlowHandler.LoadNextSceneInstantly("Startscherm");
                    break;
            }

            _previousUpdateCount = ButtonListener.BtnUpdate;
        }
    }
}