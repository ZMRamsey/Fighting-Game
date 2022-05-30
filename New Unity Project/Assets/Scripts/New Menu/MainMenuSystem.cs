using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
using TMPro;

public class MainMenuSystem : MonoBehaviour
{
    public static MainMenuSystem _instance;
    [SerializeField] MainMenuPage[] _mainMenuPages;
    public UIButton[] _mainMenuButtons;
    public UIButton[] _playMenuButtons;
    public Slider[] _settingsMenuSliders;
    public UIButton[] _settingsMenuButtons;
    public UIButton[] _quitMenuButtons;
    [SerializeField] GameObject _canvas;
    [SerializeField] GameObject[] _canvasArray;
    [SerializeField] CanvasGroup _group;
    [SerializeField] AudioSource _sfxSource, _sfxMainSource;
    [SerializeField] GameObject _unlockable;
    int _currentPage;
    int _mainSelectionIndex;
    int _playSelectionIndex;
    int _settingsSelectionIndex;
    int _quitSelectionIndex;
    //0 for audio 1 for display
    int _selectedSettingsTab;

    bool _usingMouse = true;

    [Header("Sorry")]
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;

    public AudioMixer musicMixer;
    public AudioMixer sfxMixer;

    public GameObject _musicSource;
    private AudioSource _musicSrc;

    float _musicVol = 1.0f;
    float _sfxVol = 1.0f;

    public Vector2[] _resolutionOptions;
    public TextMeshProUGUI _resolutionText;
    int _resolutionIndex;
    Vector2 _savedResolution;

    private void Awake() {
        _instance = this;
    }

    void Start() {
        SetPage(0);
        _mainMenuButtons[_mainSelectionIndex].OnFocus();
        musicMixer.SetFloat("lowpass", 22000);
        _resolutionText.text = Screen.currentResolution.width + " x " + Screen.currentResolution.height;
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

    public void PlaySFX(AudioClip clip) {
        _sfxSource.Stop();
        _sfxSource.clip = clip;
        _sfxSource.Play();
    }

    public void PlaySFXOverlap(AudioClip clip) {
        _sfxMainSource.PlayOneShot(clip);
    }

    void Update() {
        if (!GameLogic.Get()._devLocked && _unlockable.activeSelf)
        {
            _unlockable.SetActive(false);
        }

        for (int i = 0; i < _mainMenuButtons.Length; i++) {
            int steve = i + 1;
            if (_mainMenuButtons[i].OnClick() && steve != 2) {
                SetPage(steve);
                if (steve == 3)
                {
                    SetUpSettings();
                }
            }
        }

        for (int i = 0; i < _playMenuButtons.Length; i++) {
            if (_playMenuButtons[i].OnClick()) {
                GameType type = GameType.pva;

                if (i == 1) {
                    type = GameType.pvp;
                }

                if (i == 2)
                {
                    //type = GameType.tutorial;
                    if (!GameLogic.Get()._devLocked)
                    {
                        type = GameType.arcade;
                    }
                    else
                    {
                        break;
                    }
                }

                if (i == 3) {
                    type = GameType.training;
                }

                if (i == 4) {
                    type = GameType.watch;
                }

                CharacterSelectSystem.Get().SetSelectedIndex(i);
                ProceedToInputSelection(type);
            }
        }

        for (int i = 0; i < _settingsMenuButtons.Length; i++)
        {
            if (_settingsMenuButtons[i].OnClick())
            {
                if (i == 0)
                {
                    FullScreenClick();
                }

                if (i == 1)
                {
                    ResolutionClick(-1);
                }

                if (i == 2)
                {
                    ResolutionClick(1);
                }
            }
        }

        for (int i = 0; i < _quitMenuButtons.Length; i++)
        {
            if (_quitMenuButtons[i].OnClick())
            {
                if (i == 0)
                {
                    SetPage(i);
                }

                if (i == 1)
                {
                    Application.Quit();
                }
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

            if (_currentPage == 3)
            {
                SettingsMenuControls();
            }

            if (_currentPage == 4)
            {
                QuitMenuControls();
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
                //original
                //_playSelectionIndex = _playMenuButtons.Length - 1;
                //shaun
                _playSelectionIndex = _playMenuButtons.Length - 2;
            }else if (_playSelectionIndex == 3)
            {
                _playSelectionIndex--;
            }
            _playMenuButtons[_playSelectionIndex].OnFocus();
        }

        if (GlobalInputManager.Get().GetRightInput()) {
            _playMenuButtons[_playSelectionIndex].OnUnfocus();
            _playSelectionIndex++;
            //original
            //if (_playSelectionIndex > _playMenuButtons.Length - 1) {
            //shaun
            if (_playSelectionIndex > _playMenuButtons.Length - 2) {
                _playSelectionIndex = 0;
            }
            _playMenuButtons[_playSelectionIndex].OnFocus();
        }

        if ((GlobalInputManager.Get().GetUpInput() || GlobalInputManager.Get().GetDownInput()) && (_playSelectionIndex == 3 || _playSelectionIndex == 4))
        {
            _playMenuButtons[_playSelectionIndex].OnUnfocus();
            if (_playSelectionIndex == 3)
            {
                _playSelectionIndex = 4;
            }
            else
            {
                _playSelectionIndex = 3;
            }
            _playMenuButtons[_playSelectionIndex].OnFocus();
        }

        if (_playMenuButtons[_playSelectionIndex].IsFocused()) {
            if (GlobalInputManager.Get().GetSubmitInput()) {
                _playMenuButtons[_playSelectionIndex].OnSubmit();
            }
        }
    }

    void SettingsMenuControls()
    {
        bool swapped = false;
        if (GlobalInputManager.Get().GetDashInput())
        {
            _selectedSettingsTab = Mathf.Abs(_selectedSettingsTab - 1);
            swapped = true;
        }
        if (_selectedSettingsTab == 0)
        {
            if (swapped){
                for (int i = 0; i < _settingsMenuButtons.Length; i++)
                {
                    _settingsMenuButtons[i].OnUnfocus();
                    _settingsMenuButtons[i].interactable = false;
                }
                for (int i = 0; i < _settingsMenuSliders.Length; i++)
                {
                    _settingsMenuSliders[i].interactable = true;

                }
                _settingsMenuSliders[0].Select();
            }
        }
        else if (_selectedSettingsTab == 1)
        {
            if (swapped)
            {
                for (int i = 0; i < _settingsMenuSliders.Length; i++)
                {
                    _settingsMenuSliders[i].interactable = false;

                }
                for (int i = 0; i < _settingsMenuButtons.Length; i++)
                {
                    _settingsMenuButtons[i].interactable = true;
                }
                _settingsSelectionIndex = 0;
                _settingsMenuButtons[0].OnFocus();
            }

            if (GlobalInputManager.Get().GetAnyButton())
            {
                _usingMouse = false;
                _mainMenuButtons[_mainSelectionIndex].OnFocus();
            }

            if ((GlobalInputManager.Get().GetLeftInput() || GlobalInputManager.Get().GetRightInput()) && _settingsSelectionIndex != 0)
            {
                _settingsMenuButtons[_settingsSelectionIndex].OnUnfocus();
                _settingsSelectionIndex = (2 / _settingsSelectionIndex);
                _settingsMenuButtons[_settingsSelectionIndex].OnFocus();
            }

            if (GlobalInputManager.Get().GetUpInput() || GlobalInputManager.Get().GetDownInput())
            {
                _settingsMenuButtons[_settingsSelectionIndex].OnUnfocus();
                if (_settingsSelectionIndex == 0)
                {
                    _settingsSelectionIndex = 1;
                }
                else
                {
                    _settingsSelectionIndex = 0;
                }
                _settingsMenuButtons[_settingsSelectionIndex].OnFocus();
            }

            if (_settingsMenuButtons[_settingsSelectionIndex].IsFocused())
            {
                if (GlobalInputManager.Get().GetSubmitInput())
                {
                    _settingsMenuButtons[_settingsSelectionIndex].OnSubmit();
                }
            }
        }
    }

    void QuitMenuControls()
    {
        if (GlobalInputManager.Get().GetAnyButton())
        {
            _usingMouse = false;
            _mainMenuButtons[_mainSelectionIndex].OnFocus();
        }
        if (GlobalInputManager.Get().GetLeftInput())
        {
            _quitMenuButtons[_quitSelectionIndex].OnUnfocus();
            _quitSelectionIndex--;
            if (_quitSelectionIndex < 0)
            {
                _quitSelectionIndex = _quitMenuButtons.Length - 1;
            }
            _quitMenuButtons[_quitSelectionIndex].OnFocus();
        }

        if (GlobalInputManager.Get().GetRightInput())
        {
            _quitMenuButtons[_quitSelectionIndex].OnUnfocus();
            _quitSelectionIndex++;
            if (_quitSelectionIndex > _quitMenuButtons.Length - 1)
            {
                _quitSelectionIndex = 0;
            }
            _quitMenuButtons[_quitSelectionIndex].OnFocus();
        }

        if (_quitMenuButtons[_quitSelectionIndex].IsFocused())
        {
            if (GlobalInputManager.Get().GetSubmitInput())
            {
                _quitMenuButtons[_quitSelectionIndex].OnSubmit();
            }
        }
    }

    public void SetPage(int ID) {
        _currentPage = ID;
        DisablePages();

        _mainMenuPages[ID].EnablePage();

        _playSelectionIndex = 0;
        _settingsSelectionIndex = 0;
        _quitSelectionIndex = 0;

        if (!_usingMouse) {
            _mainMenuButtons[_mainSelectionIndex].OnFocus();
            _playMenuButtons[0].OnFocus();
            _settingsMenuSliders[0].Select();
            //_settingsMenuButtons[0].OnFocus();
            _quitMenuButtons[0].OnFocus();
        }
    }

    void DisablePages() {
        foreach (MainMenuPage page in _mainMenuPages) {
            if (page.isActiveAndEnabled) {
                page.DisablePage();
            }
        }
    }

    public void ProceedToInputSelection(GameType type) {
        _group.alpha = 1;
        DisableCanvas();
        CharacterSelectSystem.Get().SetGameType(type);
        CharacterSelectSystem.Get().SetCanvas();
    }

    public void SkipToCharacterSelect() {
        _group.alpha = 1;
        _playMenuButtons[0].OnFocus();
        DisableCanvas();
        CharacterSelectSystem.Get().SkipToCharacterSelect();
    }

    public static MainMenuSystem Get() {
        return _instance;
    }

    public void SetCanvas() {
        _group.alpha = 1;
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

    public void SetMusicVolume()
    {
        musicMixer.SetFloat("musicVol", Mathf.Log(musicVolumeSlider.value) * 20);
    }

    public void SetSFXVolume()
    {
        sfxMixer.SetFloat("sfxVolume", (-80 + sfxVolumeSlider.value * 100));
    }

    public void SetUpSettings()
    {
        _resolutionText.text = GameLogic.Get().GetResolution().x + " x " + GameLogic.Get().GetResolution().y;
        UICheckBox checkbox = (UICheckBox)_settingsMenuButtons[0];
        _selectedSettingsTab = 0;
        for (int i = 0; i < _settingsMenuSliders.Length; i++)
        {
            _settingsMenuSliders[i].interactable = true;
        }
        for (int i = 0; i < _settingsMenuButtons.Length; i++)
        {
            _settingsMenuButtons[i].interactable = false;
        }
        if (!GameLogic.Get().GetFullScreen())
        {
            checkbox.UnCheck();
        }
        for (int i = 0; i < _resolutionOptions.Length; i++)
        {
            if (_resolutionOptions[i] == GameLogic.Get().GetResolution())
            {
                _resolutionIndex = i;
            }
        }
    }

    public void FullScreenClick()
    {
        //print("FSC");
        UICheckBox checkbox = (UICheckBox)_settingsMenuButtons[0];
        Screen.fullScreen = checkbox.GetChecked();
        GameLogic.Get().SetFullScreen(checkbox.GetChecked());
    }

    //+1 for right, -1 for left
    public void ResolutionClick(int side)
    {
        _resolutionIndex += side;
        if (_resolutionIndex < 0)
        {
            _resolutionIndex = _resolutionOptions.Length -1;
        }

        if (_resolutionIndex > _resolutionOptions.Length -1)
        {
            _resolutionIndex = 0;
        }

        _savedResolution = _resolutionOptions[_resolutionIndex];
        GameLogic.Get().SetResolution(_savedResolution);
        UICheckBox checkbox = (UICheckBox)_settingsMenuButtons[0];
        Screen.SetResolution((int)_savedResolution.x, (int)_savedResolution.y, checkbox.GetChecked());
        _resolutionText.text = _savedResolution.x + " x " + _savedResolution.y;
        //print("Resolution set to " + _savedResolution.x + " x " + _savedResolution.y);
    }

}
