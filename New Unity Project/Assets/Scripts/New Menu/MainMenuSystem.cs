using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSystem : MonoBehaviour
{
    public static MainMenuSystem _instance;
    [SerializeField] MainMenuPage[] _mainMenuPages;
    public Button[] _mainMenuButtons;
    [SerializeField] PlayMenuPage[] _playMenuPages;
    public Button[] _playMenuButtons;
    [SerializeField] GameObject _canvas;
    [SerializeField] GameObject[] _canvasArray;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        SetPage(0);
        for (int i = 0; i < _mainMenuButtons.Length; i++)
        {
            int steve = i + 1;
            _mainMenuButtons[i].onClick.AddListener(() => SetPage(steve));
        }

        for (int i = 0; i < _playMenuButtons.Length; i++)
        {
            int steve = i;
            _playMenuButtons[i].onClick.AddListener(() => LoadToCharacterSelect());
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

    public void LoadToCharacterSelect()
    {
        DisableCanvas();
        CharacterSelectSystem.Get().SetCanvas();
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
