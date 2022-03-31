using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSystem : MonoBehaviour
{
    public static MainMenuSystem _instance;
    [SerializeField] MainMenuPage[] _mainMenuPages;
    [SerializeField] Button[] _mainMenuButtons;
    [SerializeField] PlayMenuPage[] _playMenuPages;
    [SerializeField] Button[] _playMenuButtons;
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

        //for (int i = 0; i < _playMenuButtons.Length; i++)
        //{
        //    int steve = i + 1;
        //    _playMenuButtons[i].onClick.AddListener(() => SetPage(steve));
        //}
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

    }

    public static MainMenuSystem Get()
    {
        return _instance;
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
