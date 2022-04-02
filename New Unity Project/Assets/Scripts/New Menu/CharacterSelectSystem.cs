using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class CharacterSelectSystem : MonoBehaviour
{
    public static CharacterSelectSystem _instance;
    [SerializeField] InputSelectionSystem _inputSelectionSystem;
    [SerializeField] CharacterSelectPage[] _characterSelectPages;
    [SerializeField] GameObject _canvas;

    [Header("Character Select")]
    [SerializeField] FighterProfile[] _profiles;
    [SerializeField] CharacterSelectFighter _fighterOne;
    [SerializeField] CharacterSelectFighter _fighterTwo;


    FighterFilter _fighterOneFilter;
    FighterFilter _fighterTwoFilter;
    bool _canSelectSecondCharacter;

    GameType _type;
    int _currentPage = 0;

    private void Awake() {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        RefreshCharacterSelect();
    }

    public bool IsSelectionSame() {
        return (_fighterOne.GetSelection() == _fighterTwo.GetSelection()) && (_fighterOne.GetSkinSelection() == _fighterTwo.GetSkinSelection());
    }

    // Update is called once per frame
    void Update() {
        if (_canvas.activeSelf) {
            if (_currentPage == 1 && _type != GameType.watch) {
                if (GlobalInputManager.Get().GetBackInput()) {
                    SetPage(0);
                    _inputSelectionSystem.OnPageOpened();
                }
            }
            else {
                if (GlobalInputManager.Get().GetBackInput()) {
                    LoadToMainMenu();
                }
            }

            if(_currentPage == 1) {
                if (!_fighterOne.IsReady) {
                    if (GlobalInputManager.Get().GetLeftInput(_fighterOneFilter)) {
                        _fighterOne.DecreaseCharacterIndex(_profiles.Length);
                        GameLogic.Get().GetSettings().SetFighterOneProfile(_profiles[_fighterOne.GetSelection()]);
                        _fighterOne.Refresh(_profiles[_fighterOne.GetSelection()]);
                    }
                    if (GlobalInputManager.Get().GetRightInput(_fighterOneFilter)) {
                        _fighterOne.IncreaseCharacterIndex(_profiles.Length);
                        GameLogic.Get().GetSettings().SetFighterOneProfile(_profiles[_fighterOne.GetSelection()]);
                        _fighterOne.Refresh(_profiles[_fighterOne.GetSelection()]);
                    }

                    if (GlobalInputManager.Get().GetSkinToggleLeft(_fighterOneFilter)) {
                        _fighterOne.DecreaseSkinIndex(_profiles[_fighterOne.GetSelection()]);
                        GameLogic.Get().GetSettings().SetSkinOneID(_fighterOne.GetSkinSelection());
                        _fighterOne.Refresh(_profiles[_fighterOne.GetSelection()]);
                    }
                    if (GlobalInputManager.Get().GetSkinToggleRight(_fighterOneFilter)) {
                        _fighterOne.IncreaseSkinIndex(_profiles[_fighterOne.GetSelection()]);
                        GameLogic.Get().GetSettings().SetSkinOneID(_fighterOne.GetSkinSelection());
                        _fighterOne.Refresh(_profiles[_fighterOne.GetSelection()]);
                    }


                    if (GlobalInputManager.Get().GetSubmitInput(_fighterOneFilter)) {
                        _fighterOne.IsReady = true;
                        if (!_canSelectSecondCharacter) {
                            _canSelectSecondCharacter = true;
                            return;
                        }
                    }
                }

                //f2
                if (_canSelectSecondCharacter && !_fighterTwo.IsReady) {
                    if (GlobalInputManager.Get().GetLeftInput(_fighterTwoFilter)) {
                        _fighterTwo.DecreaseCharacterIndex(_profiles.Length);
                        GameLogic.Get().GetSettings().SetFighterTwoProfile(_profiles[_fighterTwo.GetSelection()]);
                        _fighterTwo.Refresh(_profiles[_fighterTwo.GetSelection()]);
                    }
                    if (GlobalInputManager.Get().GetRightInput(_fighterTwoFilter)) {
                        _fighterTwo.IncreaseCharacterIndex(_profiles.Length);
                        GameLogic.Get().GetSettings().SetFighterTwoProfile(_profiles[_fighterTwo.GetSelection()]);
                        _fighterTwo.Refresh(_profiles[_fighterTwo.GetSelection()]);
                    }

                    if (GlobalInputManager.Get().GetSkinToggleLeft(_fighterTwoFilter)) {
                        _fighterTwo.DecreaseSkinIndex(_profiles[_fighterTwo.GetSelection()]);
                        GameLogic.Get().GetSettings().SetSkinTwoID(_fighterTwo.GetSkinSelection());
                        _fighterTwo.Refresh(_profiles[_fighterTwo.GetSelection()]);
                    }
                    if (GlobalInputManager.Get().GetSkinToggleRight(_fighterTwoFilter)) {
                        _fighterTwo.IncreaseSkinIndex(_profiles[_fighterTwo.GetSelection()]);
                        GameLogic.Get().GetSettings().SetSkinTwoID(_fighterTwo.GetSkinSelection());
                        _fighterTwo.Refresh(_profiles[_fighterTwo.GetSelection()]);
                    }

                    if (GlobalInputManager.Get().GetSubmitInput(_fighterTwoFilter)) {
                        _fighterTwo.IsReady = true;
                    }
                }

                //
                if (GlobalInputManager.Get().GetPauseInput()) {
                    GameLogic.Get().LoadScene("Base", "Menu");
                }
            }
        }
    }

    public void RefreshCharacterSelect() {
        _fighterOne.Reset();
        _fighterTwo.Reset();

        _fighterOne.Refresh(_profiles[_fighterOne.GetSelection()]);
        _fighterTwo.IncreaseSkinIndex(_profiles[_fighterTwo.GetSelection()]);
        _fighterTwo.Refresh(_profiles[_fighterTwo.GetSelection()]);

        _canSelectSecondCharacter = false;
        _fighterOneFilter = FighterFilter.one;
        _fighterTwoFilter = FighterFilter.one;

        if (_type == GameType.pvp) {
            _canSelectSecondCharacter = true;
            _fighterOneFilter = FighterFilter.one;
            _fighterTwoFilter = FighterFilter.two;
        }
    }

    public static CharacterSelectSystem Get() {
        return _instance;
    }

    public void LoadToMainMenu() {
        DisableCanvas();
        MainMenuSystem.Get().SetCanvas();
    }

    public void SetCanvas() {
        SetPage(0);
        _canvas.SetActive(true);
        _inputSelectionSystem.OnPageOpened();
    }

    public void SkipToCharacterSelect() {
        _canvas.SetActive(true);
        SetPage(1);
    }

    public void DisableCanvas() {
        _canvas.SetActive(false);
    }

    public void SetPage(int ID) {
        _currentPage = ID;
        DisablePages();

        if(ID == 1) {
            RefreshCharacterSelect();
        }

        _characterSelectPages[ID].EnablePage();
    }

    void DisablePages() {
        foreach (CharacterSelectPage page in _characterSelectPages) {
            page.DisablePage();
        }
    }

    public void SetGameType(GameType type) {
        _type = type;
    }

    public GameType GetGameType() {
        return _type;
    }
}

[System.Serializable]
public class CharacterSelectFighter
{
    [SerializeField] CharacterUI[] _characterVisuals;
    [SerializeField] TextMeshProUGUI _characterName;
    [SerializeField] TextMeshProUGUI _characterSkin;
    [SerializeField] Animator _characterPopin;
    public bool IsReady;

    int _currentSelection;
    int _currentSkinSelection;

    public void Reset() {
        _currentSelection = 0;
        _currentSkinSelection = 0;
        IsReady = false;
    }

    public int GetSelection() {
        return _currentSelection;
    }

    public int GetSkinSelection() {
        return _currentSkinSelection;
    }

    public void EnableVisual() {
        DisableVisuals();
        _characterVisuals[_currentSelection].gameObject.SetActive(true);
    }

    public void DisableVisuals() {
        foreach(CharacterUI character in _characterVisuals) {
            character.gameObject.SetActive(false);
        }
    }

    public void IncreaseCharacterIndex(int length) {
        _currentSelection++;
        _characterPopin.SetTrigger("Pop");
        _currentSkinSelection = 0;

        if (_currentSelection > length - 1) {
            _currentSelection = 0;
        }

        if (CharacterSelectSystem.Get().IsSelectionSame()) {
            _currentSkinSelection = 1;
        }
    }

    public void DecreaseCharacterIndex(int length) {
        _currentSelection--;
        _characterPopin.SetTrigger("Pop");
        _currentSkinSelection = 0;

        if (_currentSelection < 0) {
            _currentSelection = length - 1;
        }

        if (CharacterSelectSystem.Get().IsSelectionSame()) {
            _currentSkinSelection = 1;
        }
    }

    public void IncreaseSkinIndex(FighterProfile current) {
        _currentSkinSelection++;
        if(_currentSkinSelection > current.GetPalleteLength() - 1) {
            _currentSkinSelection = 0;
        }

        if (CharacterSelectSystem.Get().IsSelectionSame()) {
            IncreaseSkinIndex(current);
        }
    }

    public void DecreaseSkinIndex(FighterProfile current) {
        _currentSkinSelection--;
        if (_currentSkinSelection < 0) {
            _currentSkinSelection = current.GetPalleteLength() - 1;
        }

        if (CharacterSelectSystem.Get().IsSelectionSame()) {
            DecreaseSkinIndex(current);
        }
    }

    public CharacterUI GetCharacter() {
        return _characterVisuals[_currentSelection];
    }

    public void Refresh(FighterProfile profile) {
        EnableVisual();
        _characterName.text = profile.GetName();

        if (_currentSkinSelection == 0) {
            GetCharacter().SetState(false);
            _characterSkin.text = $"<DEFAULT>";
        }
        else {
            GetCharacter().SetState(true);
            GetCharacter().UpdatePallete(profile.GetPallete(_currentSkinSelection));
            _characterSkin.text = $"<{profile.GetPallete(_currentSkinSelection).name}>";
        }
    }
}
