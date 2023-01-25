using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Repository;
using System.Threading.Tasks;
using System.Collections.Generic;
using Models;
using UnityEngine.Networking;
using UiScripts;
using UnityEngine.SceneManagement;
using System.Linq;

public class scoreboard : MonoBehaviour
{
    List<ScoreboardItem> list_Items;
    private UIDocument _document;


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
        
        list_Items = await ScoreRepository.GetScoresAsync();
        foreach (ScoreboardItem item in list_Items)
        {
            item.Img = await GetRemoteTexture(item.ImgUrl);
        }
        list_Items.Sort();
        ListView lvwScores = _root.Q<ListView>("lvwScores");
        
        FillList(lvwScores, list_Items.Take(10).ToList());

    }
    void FillList(ListView list, List<ScoreboardItem> items)
    {
        list.AddToClassList("c-score-list");
        list.Q<ScrollView>().verticalScrollerVisibility = ScrollerVisibility.Hidden;
        list.fixedItemHeight = 65;
        list.makeItem = MakeItem;
        list.bindItem = BindItem;
        list.itemsSource = items;
    }
    private VisualElement MakeItem()
    {
        //Here we take the uxml and make a VisualElement
        VisualElement listItem = new VisualElement();
        listItem.AddToClassList("c-score-list-item");

        var number = new Label{name = "score-number"};
        number.AddToClassList("c-score-number");
        listItem.Add(number);

        var i = new Image{name = "score-image"};
        i.AddToClassList("c-score-image");
        listItem.Add(i);

        var l = new Label { name = "score-label" };
        l.AddToClassList("c-score-label");
        listItem.Add(l);

        return listItem;

    }
    
    private void BindItem(VisualElement e, int i)
    {
        e.Q<Label>("score-number").text = $"{i + 1}.";

        e.Q<Label>("score-label").text = $"{list_Items[i].Username} - {list_Items[i].Score}";
        
        e.Q<Image>("score-image").image = list_Items[i].Img;

    }
    
    public static async Task<Texture2D> GetRemoteTexture(string url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            // begin request:
            var asyncOp = www.SendWebRequest();

            // await until it's done: 
            while (asyncOp.isDone == false)
                await Task.Delay(1000 / 30);//30 hertz

            // read results:
            if( www.result!=UnityWebRequest.Result.Success )// for Unity >= 2020.1
            {
                // log error:
                #if DEBUG
                Debug.Log($"{www.error}, URL:{www.url}");
                #endif

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
            SceneManager.LoadScene("StartScene");
        }
        if (ButtonListener.Left == BtnValue.Pressed)
        {
            SceneManager.LoadScene("StartScene");
        }
        if (ButtonListener.Right == BtnValue.Pressed)
        {
            SceneManager.LoadScene("StartScene");
        }
        _previousUpdateCount = ButtonListener.BtnUpdate;
    }
}
