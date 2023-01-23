using System.Collections;
using System.Collections.Generic;
using Models;
using Repository;
using UiScripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class kiezenGebruikersNaam : MonoBehaviour
{
    // Start is called before the first frame update
    private string _gebruikersNaam;
    private bool _naamIsIngevuld;
    private UIDocument _document;
    private VisualElement _btnYellowTop;
    private VisualElement _btnBlueTop;
    private Label _userNameLabel;
    private int _counter;

    void Start()
    {
        GenerateNewName();
        _document = GetComponent<UIDocument>();
        _userNameLabel = _document.rootVisualElement.Q<Label>("generatedname");
        var btnYellow = _document.rootVisualElement.Q("yellowButton");
        _btnYellowTop = btnYellow.Q("buttonTop");
        var btnBlue = _document.rootVisualElement.Q("blueButton");
        _btnBlueTop = btnBlue.Q("buttonTop");
        ButtonListener.ListenToButtons();
    }

    private async void GenerateNewName()
    {
        _gebruikersNaam = await ScoreRepository.UserNameGeneration();
        _userNameLabel.text = _gebruikersNaam;
    }

    // Update is called once per frame
    void Update()
    {
        if (_naamIsIngevuld)
        {
            _userNameLabel.text = _gebruikersNaam;
            _naamIsIngevuld = false;
        }

        if (_counter <= ButtonListener.BtnUpdate) return;
        _counter = ButtonListener.BtnUpdate;
        switch (ButtonListener.Left)
        {
            case BtnValue.Pressed:
                _btnBlueTop.AddToClassList("move-down");
                GenerateNewName();
                break;
            case BtnValue.Released:
                _btnBlueTop.RemoveFromClassList("move-down");
                break;
        }

        switch (ButtonListener.Right)
        {
            case BtnValue.Pressed:
                _btnYellowTop.AddToClassList("move-down");
                NextScene();
                break;
            case BtnValue.Released:
                _btnYellowTop.RemoveFromClassList("move-down");
                break;
        }
    }

    private void NextScene()
    {
        // Todo: change name of next scene
        // SceneManager.LoadScene("placeholder");
    }
}