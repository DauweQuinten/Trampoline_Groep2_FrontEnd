using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Repository;
using UnityEngine;
using UnityEngine.Networking;
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


        public void Start()
        {
            _isEnabled = true;
            _document = GetComponent<UIDocument>();
            // Each editor window contains a root VisualElement object
            var btnYellow = _document.rootVisualElement.Q("yellowButton");
            _btnYellowTop = btnYellow.Q("buttonTop");
            var btnBlue = _document.rootVisualElement.Q("blueButton");
            _btnBlueTop = btnBlue.Q("buttonTop");
            ButtonListener.ListenToButtons();
            _root = _document.rootVisualElement;
            FillBoard();
        }


        async void FillBoard()
        {
            _listItems = await ScoreRepository.GetScoresAsync();
            _userItem = await ScoreRepository.GetScoreAsync(GameVariablesHolder.Id);

            foreach (ScoreboardItem item in _listItems)
            {
                item.Img = await GetRemoteTexture(item.ImgUrl);
            }

            _userItem.Img = await GetRemoteTexture(_userItem.ImgUrl);
            _listItems.Sort();
            ListView lvwScores = _root.Q<ListView>("lvwScores");

            FillList(lvwScores, _listItems.Take(9).ToList());

            var scoreInSeconds = _userItem.Score / 10;
            var seconds = scoreInSeconds % 60;
            var minutes = scoreInSeconds / 60;
            var milliSeconds = _userItem.Score % 10;

            _root.Q<Image>("lblUserImage").image = _userItem.Img;
            _root.Q<Label>("thisUserScore").text = $"{minutes:D}:{seconds:D2}.{milliSeconds:D1}";
            _root.Q<Label>("lblUserScore").text = _userItem.Username;

            foreach (var item in _listviewItems)
            {
                if (item.ClassListContains($"Id{_userItem.Id}"))
                {
                    item.AddToClassList("c-user-list");
                }
            }
        }

        void FillList(ListView list, List<ScoreboardItem> items)
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
            number.AddToClassList("c-score-number");
            leftAlignement.Add(number);

            var i = new Image { name = "score-image" };
            i.AddToClassList("c-score-image");
            i.AddToClassList("u-list-img");
            leftAlignement.Add(i);

            var l = new Label { name = "score-name-label" };
            l.AddToClassList("c-score-label");
            l.AddToClassList("u-list-label");
            leftAlignement.Add(l);

            var s = new Label { name = "score-number-label" };
            s.AddToClassList("c-score-label");
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

            e.Q<Image>("score-image").image = _listItems[i].Img;
        }

        public static async Task<Texture2D> GetRemoteTexture(string url)
        {
            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
            {
                // begin request:
                var asyncOp = www.SendWebRequest();

                // await until it's done: 
                while (asyncOp.isDone == false)
                    await Task.Delay(1000 / 30); //30 hertz

                // read results:
                if (www.result != UnityWebRequest.Result.Success) // for Unity >= 2020.1
                {
                    // log error:
                    Debug.Log($"{www.error}, URL:{www.url}");

                    // nothing to return on error:
                    return null;
                }
                else
                {
                    // return valid results:
                    return DownloadHandlerTexture.GetContent(www);
                }
            }
        }


        private void Update()
        {
            if (!_isEnabled || ButtonListener.BtnUpdate <= _previousUpdateCount) return;
            if (ButtonListener.Both == BtnValue.Pressed)
            {
                _document.rootVisualElement.Clear();
                SceneManager.LoadScene("Startscherm");
            }

            if (ButtonListener.Left == BtnValue.Pressed)
            {
                SceneManager.LoadScene("Startscherm");
            }

            if (ButtonListener.Right == BtnValue.Pressed)
            {
                SceneManager.LoadScene("Startscherm");
            }

            _previousUpdateCount = ButtonListener.BtnUpdate;
        }
    }
}