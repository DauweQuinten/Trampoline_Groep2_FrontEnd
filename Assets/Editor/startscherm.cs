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

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/startscherm.uss");
        root.styleSheets.Add(styleSheet);
        
        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/startscherm.uxml");
        VisualElement labelFromUxml = visualTree.Instantiate();
        root.Add(labelFromUxml);
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