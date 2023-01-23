using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Repository;
using System.Threading.Tasks;
using static UnityEditor.Progress;
using System.Collections.Generic;
using Models;
using System;
using System.Collections;
using UnityEngine.Networking;

public class scoreboard : MonoBehaviour
{
    ListView lvwScores;
    List<ScoreboardItem> list_Items;
    private UIDocument _document;
    [SerializeField] Material _material;
    Texture2D _texture;
    [MenuItem("Window/UI Toolkit/scoreboard")]

    public void Start()
    {
        _document = GetComponent<UIDocument>();
        // Each editor window contains a root VisualElement object
        VisualElement root = _document.rootVisualElement;
        FillList(root);
        
    }

    

    async void FillList(VisualElement root)
    {
        
        list_Items = await ScoreRepository.GetScoresAsync();
        foreach (ScoreboardItem item in list_Items)
        {
            item.Img = await GetRemoteTexture(item.ImgUrl);
        }
        list_Items.Sort();
        
        lvwScores = root.Q<ListView>("lvwScores");
        lvwScores.makeItem = MakeItem;
        lvwScores.bindItem = BindItem;
        lvwScores.itemsSource = list_Items;
    }

    private VisualElement MakeItem()
    {
        //Here we take the uxml and make a VisualElement
        VisualElement listItem = new VisualElement();
        var l = new Label();
        l.name = "score-label";
        l.AddToClassList("c-score-label");
        listItem.Add(l);
        
        var i = new Image();
        i.name = "score-image";
        i.AddToClassList("c-score-image");
        listItem.Add(i);
        return listItem;

    }

    private void BindItem(VisualElement e, int i)
    {
        //We add the game name to the label of the list item
        e.Q<Label>("score-label").text = $"{list_Items[i].Username} - {list_Items[i].Score}";
        

        _texture = await GetRemoteTexture(list_Items[i].ImgUrl);
        //Here we create a call back for clicking on the list item and provide data to a function
        e.Q<Image>("score-image").image = _texture;

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
            //if (www.isNetworkError || www.isHttpError)
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
    //void OnDestroy() => Dispose();
    //public void Dispose() => Destroy(_texture);
}