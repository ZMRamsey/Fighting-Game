using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MainMenuSystem : MonoBehaviour
{
    public static MainMenuSystem _instance;
    [SerializeField] MainMenuPage[] _mainMenuPages;
    public UIButton[] _mainMenuButtons;
    [SerializeField] PlayMenuPage[] _playMenuPages;
    public UIButton[] _playMenuButtons;
    [SerializeField] GameObject _canvas;
    [SerializeField] GameObject[] _canvasArray;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        SetPage(0);
        //for (int i = 0; i < _mainMenuButtons.Length; i++)
        //{
        //    int steve = i + 1;
        //    _mainMenuButtons[i].onClick.AddListener(() => SetPage(steve));
        //}

        //for (int i = 0; i < _playMenuButtons.Length; i++)
        //{
        //    int steve = i;
        //    _playMenuButtons[i].onClick.AddListener(() => LoadToCharacterSelect());
        //}

    }

    void Update() {
        for (int i = 0; i < _mainMenuButtons.Length; i++) {
            int steve = i + 1;
            if (_mainMenuButtons[i].OnClick()) {
                SetPage(steve);
            }
        }

        for (int i = 0; i < _playMenuButtons.Length; i++) {
            if (_playMenuButtons[i].OnClick()) {
                GameType type = GameType.pva;

                if(i == 1) {
                    type = GameType.pvp;
                }

                LoadToCharacterSelect(type);
            }
        }
    }

    public void SetPage(int ID)
    {
        DisablePages();

        _mainMenuPages[ID].EnablePage();
    }

    void DisablePages()
    {
        foreach (MainMenuPage page in _mainMenuPages)
        {
            if (page.isActiveAndEnabled)
            {
                page.DisablePage();
            }
        }
    }

    public void LoadToCharacterSelect(GameType type)
    {
        DisableCanvas();
        CharacterSelectSystem.Get().SetCanvas(type);
    }

    public static MainMenuSystem Get()
    {
        return _instance;
    }

    public void SetCanvas()
    {
        _canvas.SetActive(true);
    }

    public void DisableCanvas()
    {
        _canvas.SetActive(false);
    }

    public void SetCanvas(int ID)
    {
        DisableAllCanvas();
        _canvasArray[ID].SetActive(true);
    }

    public void DisableAllCanvas()
    {
        foreach (GameObject canvas in _canvasArray)
        {
            canvas.SetActive(false);
        }
    }
}
