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
    private int _counter = -1;
    private bool _yellowReleasedFirst = false;

    void Start()
    {
        GenerateNewName();
        _document = GetComponent<UIDocument>();
        _userNameLabel = _document.rootVisualElement.Q<Label>("generatedname");
        var score = _document.rootVisualElement.Q<Label>("score");
        var minuuten = GameVariablesHolder.Score / 60;
        var seconden = GameVariablesHolder.Score % 60;
        score.text = $"{minuuten}:{seconden}";
        var btnYellow = _document.rootVisualElement.Q("yellowButton");
        _btnYellowTop = btnYellow.Q("buttonTop");
        var btnBlue = _document.rootVisualElement.Q("blueButton");
        _btnBlueTop = btnBlue.Q("buttonTop");
        ButtonListener.ListenToButtons();
        Debug.Log("Turning LED's on");
        ButtonListener.UpdateLed(LedType.Left, LedValue.On);
        ButtonListener.UpdateLed(LedType.Right, LedValue.On);
    }

    private async void GenerateNewName()
    {
        _gebruikersNaam = await ScoreRepository.UserNameGeneration();
        _userNameLabel.text = _gebruikersNaam.Trim('\"');
    }

    // Update is called once per frame
    void Update()
    {
        if (ButtonListener.BtnUpdate > _counter)
        {
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
                    if (_yellowReleasedFirst) NextScene();
                    break;
                case BtnValue.Released:
                    _btnYellowTop.RemoveFromClassList("move-down");
                    _yellowReleasedFirst = true;
                    break;
            }
        }
    }

    private void NextScene()
    {
        Debug.Log("going to next scene and turning LED's off");
        GameVariablesHolder.Username = _gebruikersNaam;
        ButtonListener.UpdateLed(LedType.Left, LedValue.Off);
        ButtonListener.UpdateLed(LedType.Right, LedValue.Off);
        // Todo: change name of next scene
        // SceneManager.LoadScene("scenes/");
    }
}