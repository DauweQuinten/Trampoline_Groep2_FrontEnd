using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Repository;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

namespace UiScripts
{
    public class scoreboard : EditorWindow
    {
        ListView lvwScores;
        List<ScoreboardItem> list_Items;

        [MenuItem("Window/UI Toolkit/scoreboard")]
        public static void ShowExample()
        {
            scoreboard wnd = GetWindow<scoreboard>();
            wnd.titleContent = new GUIContent("scoreboard");
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // VisualElements objects can contain other VisualElement following a tree hierarchy.
            //VisualElement label = new Label("Hello World! From C#");
            //root.Add(label);

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/scoreboard/scoreboard.uxml");
            VisualElement labelFromUXML = visualTree.Instantiate();
            root.Add(labelFromUXML);

            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/scoreboard/scoreboard.uss");
            root.styleSheets.Add(styleSheet);

            FillList(root);
        }

        async Task FillList(VisualElement root)
        {
            list_Items = await ScoreRepository.GetScoresAsync();
            list_Items.Sort();
            Action<VisualElement, int> bindItem = (e, i) =>
                (e as Label).text = $"{list_Items[i].Username} - {list_Items[i].Score}";
            lvwScores = root.Q<ListView>("lvwScores");

            Func<VisualElement> makeItem = () =>
            {
                var v = new VisualElement();
                var i = new Image();
                var l = new Label();
                l.style.width = 64;
                l.style.color = Color.white;
                v.Add(i);
                v.Add(l);
                return l;
            };

            lvwScores.makeItem = makeItem;
            lvwScores.bindItem = bindItem;
            lvwScores.itemsSource = list_Items;
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
                //if (www.isNetworkError || www.isHttpError)
                if (www.result != UnityWebRequest.Result.Success) // for Unity >= 2020.1
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
    }
}