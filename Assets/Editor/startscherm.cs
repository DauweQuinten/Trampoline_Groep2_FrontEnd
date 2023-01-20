using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class startscherm : EditorWindow
{
    [MenuItem("Window/UI Toolkit/startscherm")]
    public static void ShowExample()
    {
        startscherm wnd = GetWindow<startscherm>();
        wnd.titleContent = new GUIContent("startscherm");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        var root = rootVisualElement;
        // Import UXML
        var tvisualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/startscherm.uxml");
        VisualElement labelFromUxml = tvisualTree.Instantiate();
        root.Add(labelFromUxml);
        ListenToButtons();
    }

    private void ListenToButtons()
    {
        var gameObject = GameObject.Find("SocketController");
        var events = gameObject.GetComponent<SocketEvents>();
        events.btnPressedLeft.AddListener(LeftMoveDown);
        events.btnPressedRight.AddListener(RightMoveDown);
        events.btnPressedBoth.AddListener(BothCloseUI);
        // events.btnReleasedLeft.AddListener(LeftMoveUp);
        // events.btnReleasedRight.AddListener(RightMoveUp);
    }

    private void BothCloseUI()
    {
        rootVisualElement.Clear();
    }

    private void RightMoveDown()
    {
        // select right button from visualtree
        rootVisualElement.Query("yellowButton").First().AddToClassList("move-down");
    }

    private void LeftMoveDown(Color arg0)
    {
        // throw new System.NotImplementedException();
        rootVisualElement.Query("blueButton").First().AddToClassList("move-down");
    }

    private void LeftMoveUp()
    {
        rootVisualElement.Query("blueButton").First().RemoveFromClassList("move-down");
    }

    private void RightMoveUp()
    {
        rootVisualElement.Query("yellowButton").First().RemoveFromClassList("move-down");
    }

    void OnEnable()
    {
        CreateGUI();
    }

    void OnDisable()
    {
        rootVisualElement.Clear();
    }
}