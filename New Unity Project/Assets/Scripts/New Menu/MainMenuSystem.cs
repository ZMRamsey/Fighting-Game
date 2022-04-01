using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MainMenuSystem : MonoBehaviour
{
    public static MainMenuSystem _instance;
    [SerializeField] MainMenuPage[] _mainMenuPages;
    public UIButton[] _mainMenuButtons;
    [SerializeField] PlayMenuPage[] _playMenuPages;
    public UIButton[] _playMenuButtons;
    [SerializeField] GameObject _canvas;
    [SerializeField] GameObject[] _canvasArray;
    int _currentPage;
    int _mainSelectionIndex;
    int _playSelectionIndex;

    bool _usingMouse = true;

    private void Awake() {
        _instance = this;
    }

    void Start() {
        SetPage(0);
        _mainMenuButtons[_mainSelectionIndex].OnFocus();
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

                if (i == 1) {
                    type = GameType.pvp;
                }

                LoadToCharacterSelect(type);
            }
        }

        if (Mouse.current.leftButton.wasPressedThisFrame) {
            _usingMouse = true;
        }

        if (_currentPage != 0) {
            if (GlobalInputManager.Get().GetBackInput()) {
                if (_canvas.activeSelf) {
                    SetPage(0);
                }
            }
        }

        if (_canvas.activeInHierarchy) {
            if (_currentPage == 0) {
                MainMenuControls();
            }

            if (_currentPage == 1) {
                PlayMenuControls();
            }
        }
    }

    void MainMenuControls() {
        if (GlobalInputManager.Get().GetAnyButton()) {
            _usingMouse = false;
            _mainMenuButtons[_mainSelectionIndex].OnFocus();
        }

        if (GlobalInputManager.Get().GetLeftInput()) {
            _mainMenuButtons[_mainSelectionIndex].OnUnfocus();
            _mainSelectionIndex--;
            if (_mainSelectionIndex < 0) {
                _mainSelectionIndex = _mainMenuButtons.Length - 1;
            }
            _mainMenuButtons[_mainSelectionIndex].OnFocus();
        }

        if (GlobalInputManager.Get().GetRightInput()) {
            _mainMenuButtons[_mainSelectionIndex].OnUnfocus();
            _mainSelectionIndex++;
            if (_mainSelectionIndex > _mainMenuButtons.Length - 1) {
                _mainSelectionIndex = 0;
            }
            _mainMenuButtons[_mainSelectionIndex].OnFocus();
        }

        if (_mainMenuButtons[_mainSelectionIndex].IsFocused()) {
            if (GlobalInputManager.Get().GetSubmitInput()) {
                _mainMenuButtons[_mainSelectionIndex].OnSubmit();
            }
        }
    }

    void PlayMenuControls() {
        if (GlobalInputManager.Get().GetAnyButton()) {
            _usingMouse = false;
            _mainMenuButtons[_mainSelectionIndex].OnFocus();
        }

        if (GlobalInputManager.Get().GetLeftInput()) {
            _playMenuButtons[_playSelectionIndex].OnUnfocus();
            _playSelectionIndex--;
            if (_playSelectionIndex < 0) {
                _playSelectionIndex = _playMenuButtons.Length - 1;
            }
            _playMenuButtons[_playSelectionIndex].OnFocus();
        }

        if (GlobalInputManager.Get().GetRightInput()) {
            _playMenuButtons[_playSelectionIndex].OnUnfocus();
            _playSelectionIndex++;
            if (_playSelectionIndex > _playMenuButtons.Length - 1) {
                _playSelectionIndex = 0;
            }
            _playMenuButtons[_playSelectionIndex].OnFocus();
        }

        if (_playMenuButtons[_playSelectionIndex].IsFocused()) {
            if (GlobalInputManager.Get().GetSubmitInput()) {
                _playMenuButtons[_playSelectionIndex].OnSubmit();
            }
        }
    }

    public void SetPage(int ID) {
        _currentPage = ID;
        DisablePages();

        _mainMenuPages[ID].EnablePage();

        _playSelectionIndex = 0;

        if (!_usingMouse) {
            _mainMenuButtons[_mainSelectionIndex].OnFocus();
            _playMenuButtons[0].OnFocus();
        }
    }

    void DisablePages() {
        foreach (MainMenuPage page in _mainMenuPages) {
            if (page.isActiveAndEnabled) {
                page.DisablePage();
            }
        }
    }

    public void LoadToCharacterSelect(GameType type) {
        DisableCanvas();
        CharacterSelectSystem.Get().SetCanvas(type);
    }

    public static MainMenuSystem Get() {
        return _instance;
    }

    public void SetCanvas() {
        _canvas.SetActive(true);
    }

    public void DisableCanvas() {
        _canvas.SetActive(false);
    }

    public void SetCanvas(int ID) {
        DisableAllCanvas();
        _canvasArray[ID].SetActive(true);
    }

    public void DisableAllCanvas() {
        foreach (GameObject canvas in _canvasArray) {
            canvas.SetActive(false);
        }
    }
}
