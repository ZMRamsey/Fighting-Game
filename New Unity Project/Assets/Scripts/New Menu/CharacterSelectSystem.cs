using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterSelectSystem : MonoBehaviour
{
    public static CharacterSelectSystem _instance;
    [SerializeField] CharacterSelectPage[] _characterSelectPages;
    [SerializeField] GameObject _canvas;

    int _numberOfPlayers;

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            LoadToMainMenu();
        }
    }

    public static CharacterSelectSystem Get()
    {
        return _instance;
    }

    public void LoadToMainMenu()
    {
        DisableCanvas();
        MainMenuSystem.Get().SetCanvas();
    }

    public void SetCanvas()
    {
        _canvas.SetActive(true);
    }

    public void DisableCanvas()
    {
        _canvas.SetActive(false);
    }

    public void SetPage(int ID)
    {
        DisablePages();

        _characterSelectPages[ID].EnablePage();
    }

    void DisablePages()
    {
        foreach (CharacterSelectPage page in _characterSelectPages)
        {
            if (page.isActiveAndEnabled)
            {
                page.DisablePage();
            }
        }
    }

    public void SetNumberOfPlayers(int players)
    {
        _numberOfPlayers = players;
    }
}