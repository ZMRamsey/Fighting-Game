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

    [SerializeField] Animator _fadeWhite;


    FighterFilter _fighterOneFilter;
    FighterFilter _fighterTwoFilter;
    bool _canSelectSecondCharacter;
    bool _hasGameStated;

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
        if(!_hasGameStated && _fighterOne.IsReady && _fighterTwo.IsReady) {
            StartCoroutine(StartGame());
            _hasGameStated = true;
        }

        if (_hasGameStated) {
            return;
        }

        if (_canvas.activeSelf) {
            if (_currentPage == 1 && _type != GameType.watch) {
                if (!_fighterOne.IsReady && !_fighterTwo.IsReady) {
                    if (GlobalInputManager.Get().GetBackInput()) {
                        SetPage(0);
                        _inputSelectionSystem.OnPageOpened();
                    }
                }
            }
            else {
                if (GlobalInputManager.Get().GetBackInput()) {
                    LoadToMainMenu();
                }
            }

            if (_currentPage == 1) {
                if (!_fighterOne.IsReady) {
                    if (GlobalInputManager.Get().GetLeftInput(_fighterOneFilter)) {
                        _fighterOne.DecreaseCharacterIndex(_profiles.Length);
                        _fighterOne.Refresh(_profiles[_fighterOne.GetSelection()], FighterFilter.one);
                    }
                    if (GlobalInputManager.Get().GetRightInput(_fighterOneFilter)) {
                        _fighterOne.IncreaseCharacterIndex(_profiles.Length);
                        _fighterOne.Refresh(_profiles[_fighterOne.GetSelection()], FighterFilter.one);
                    }

                    if (GlobalInputManager.Get().GetSkinToggleLeft(_fighterOneFilter)) {
                        _fighterOne.DecreaseSkinIndex(_profiles[_fighterOne.GetSelection()]);
                        _fighterOne.Refresh(_profiles[_fighterOne.GetSelection()], FighterFilter.one);
                    }
                    if (GlobalInputManager.Get().GetSkinToggleRight(_fighterOneFilter)) {
                        _fighterOne.IncreaseSkinIndex(_profiles[_fighterOne.GetSelection()]);
                        _fighterOne.Refresh(_profiles[_fighterOne.GetSelection()], FighterFilter.one);
                    }

                    if (GlobalInputManager.Get().GetRandomInput(_fighterOneFilter)) {
                        _fighterOne.RandomCharacter(_profiles.Length);
                        _fighterOne.Refresh(_profiles[_fighterOne.GetSelection()], FighterFilter.one);
                        _fighterOne.SetAsReady();

                        if (_type != GameType.pvp) {
                            _canSelectSecondCharacter = true;
                            return;
                        }
                    }

                    if (GlobalInputManager.Get().GetSubmitInput(_fighterOneFilter)) {
                        _fighterOne.SetAsReady();
                        if (_type != GameType.pvp) {
                            _canSelectSecondCharacter = true;
                            return;
                        }
                    }
                }
                else {
                    if (GlobalInputManager.Get().GetBackInput(_fighterOneFilter)) {
                        _fighterOne.SetAsUnready();
                        _fighterOne.Refresh(_profiles[_fighterOne.GetSelection()], FighterFilter.one);
                        if(_type != GameType.pvp) {
                            _canSelectSecondCharacter = false;
                        }
                    }
                }

                //f2
                if (_canSelectSecondCharacter && !_fighterTwo.IsReady) {
                    if (GlobalInputManager.Get().GetLeftInput(_fighterTwoFilter)) {
                        _fighterTwo.DecreaseCharacterIndex(_profiles.Length);
                        _fighterTwo.Refresh(_profiles[_fighterTwo.GetSelection()], FighterFilter.two);
                    }
                    if (GlobalInputManager.Get().GetRightInput(_fighterTwoFilter)) {
                        _fighterTwo.IncreaseCharacterIndex(_profiles.Length);
                        _fighterTwo.Refresh(_profiles[_fighterTwo.GetSelection()], FighterFilter.two);
                    }

                    if (GlobalInputManager.Get().GetSkinToggleLeft(_fighterTwoFilter)) {
                        _fighterTwo.DecreaseSkinIndex(_profiles[_fighterTwo.GetSelection()]);
                        _fighterTwo.Refresh(_profiles[_fighterTwo.GetSelection()], FighterFilter.two);
                    }
                    if (GlobalInputManager.Get().GetSkinToggleRight(_fighterTwoFilter)) {
                        _fighterTwo.IncreaseSkinIndex(_profiles[_fighterTwo.GetSelection()]);
                        _fighterTwo.Refresh(_profiles[_fighterTwo.GetSelection()], FighterFilter.two);
                    }

                    if (GlobalInputManager.Get().GetRandomInput(_fighterTwoFilter)) {
                        _fighterTwo.RandomCharacter(_profiles.Length);
                        _fighterTwo.Refresh(_profiles[_fighterTwo.GetSelection()], FighterFilter.two);
                        _fighterTwo.SetAsReady();
                    }

                    if (GlobalInputManager.Get().GetSubmitInput(_fighterTwoFilter)) {
                        _fighterTwo.SetAsReady();
                    }
                }
                else {
                    if (GlobalInputManager.Get().GetBackInput(_fighterTwoFilter)) {
                        _fighterTwo.SetAsUnready();
                        _fighterTwo.Refresh(_profiles[_fighterTwo.GetSelection()], FighterFilter.one);
                    }
                }
            }
        }
    }

    public void RefreshCharacterSelect() {
        _fighterOne.Reset(2);
        _fighterTwo.Reset(3);

        _fighterOne.Refresh(_profiles[_fighterOne.GetSelection()], FighterFilter.one);
        _fighterTwo.IncreaseSkinIndex(_profiles[_fighterTwo.GetSelection()]);
        _fighterTwo.Refresh(_profiles[_fighterTwo.GetSelection()], FighterFilter.two);

        _canSelectSecondCharacter = false;
        _fighterOneFilter = FighterFilter.none;
        _fighterTwoFilter = FighterFilter.none;

        if (_type == GameType.pvp) {
            _canSelectSecondCharacter = true;
            _fighterOneFilter = FighterFilter.one;
            _fighterTwoFilter = FighterFilter.two;
        }

        if(_type == GameType.tutorial) {
            _fighterOne.SetAsReady();
            _fighterTwo.SetAsReady();
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
        RefreshCharacterSelect(); 
        _canvas.SetActive(true);
        SetPage(1);
    }

    public void DisableCanvas() {
        _canvas.SetActive(false);
    }

    public void SetPage(int ID) {
        _currentPage = ID;
        DisablePages();

        if (ID == 1) {
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
        GameLogic.Get()._type = type;
    }

    public GameType GetGameType() {
        return _type;
    }

    IEnumerator StartGame() {
        yield return new WaitForSeconds(1);
        _fadeWhite.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        GameLogic.Get().LoadScene("Base", "Menu");
    }
}

[System.Serializable]
public class CharacterSelectFighter
{
    [SerializeField] CharacterUI[] _characterVisuals;
    [SerializeField] TextMeshProUGUI _characterName;
    [SerializeField] TextMeshProUGUI _characterSkin;
    [SerializeField] Animator _characterPopin;
    [SerializeField] GameObject _readyObject;
    public bool IsReady;

    int _currentSelection;
    int _currentSkinSelection;

    public void Reset(int selection) {
        _currentSelection = selection;
        _currentSkinSelection = 0;
        IsReady = false;
        _readyObject.SetActive(false);
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
        foreach (CharacterUI character in _characterVisuals) {
            character.gameObject.SetActive(false);
        }
    }

    public void SetAsReady() {
        _characterPopin.SetTrigger("Select");
        _readyObject.SetActive(true);
        IsReady = true;
        _characterName.text = "";
        _characterSkin.text = "";
    }

    public void SetAsUnready() {
        IsReady = false;
        _readyObject.SetActive(false);
    }

    public void RandomCharacter(int length) {
        _currentSelection = Random.Range(0, length);
        _characterPopin.SetTrigger("Pop");

        if (CharacterSelectSystem.Get().IsSelectionSame()) {
            _currentSkinSelection = 1;
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
        if (_currentSkinSelection > current.GetPalleteLength() - 1) {
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

    public void Refresh(FighterProfile profile, FighterFilter filter) {
        EnableVisual();
        _characterName.text = profile.GetName();

        if (filter == FighterFilter.one) {
            GameLogic.Get().GetSettings().SetSkinOneID(_currentSkinSelection);
            GameLogic.Get().GetSettings().SetFighterOneProfile(profile);
        }
        else {
            GameLogic.Get().GetSettings().SetSkinTwoID(_currentSkinSelection);
            GameLogic.Get().GetSettings().SetFighterTwoProfile(profile);
        }

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
