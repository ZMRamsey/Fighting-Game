using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class CharacterSelectSystem : MonoBehaviour
{
    public static CharacterSelectSystem _instance;
    [SerializeField] InputSelectionSystem _inputSelectionSystem;
    [SerializeField] CharacterSelectPage[] _characterSelectPages;
    [SerializeField] GameObject _canvas;
    private bool _mainLoadFlag = false;
    private int _selectedIndex = 0;

    [Header("Character Select")]
    [SerializeField] FighterProfile[] _profiles;
    [SerializeField] CharacterSelectFighter _fighterOne;
    [SerializeField] CharacterSelectFighter _fighterTwo;

    [SerializeField] Animator _fadeWhite;

    [Header("Selectors")]
    [SerializeField] Transform _selectorOne;
    [SerializeField] Transform _selectorTwo;
    [SerializeField] Transform _selectorJoint;
    [SerializeField] Vector2[] _selectorPositions;
    [SerializeField] GameObject[] _borders;

    [Header("SFX")]
    [SerializeField] AudioClip _readySFX;
    [SerializeField] AudioClip _toggleLeftSFX, _toggleRightSFX;
    [SerializeField] AudioClip _skinToggleSFX;
    [SerializeField] AudioClip _transitionSFX;


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

        if (_mainLoadFlag)
        {
            _mainLoadFlag = false;
            LoadToMainMenu();
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
                    _mainLoadFlag = true;
                }
            }

            if (_currentPage == 1) {
                if (!_fighterOne.IsReady) {
                    if (GlobalInputManager.Get().GetLeftInput(_fighterOneFilter)) {
                        MainMenuSystem.Get().PlaySFX(_toggleLeftSFX);
                        _fighterOne.DecreaseCharacterIndex(_profiles.Length);
                        _fighterOne.Refresh(_profiles[_fighterOne.GetSelection()], FighterFilter.one);
                    }
                    if (GlobalInputManager.Get().GetRightInput(_fighterOneFilter)) {
                        MainMenuSystem.Get().PlaySFX(_toggleRightSFX);
                        _fighterOne.IncreaseCharacterIndex(_profiles.Length);
                        _fighterOne.Refresh(_profiles[_fighterOne.GetSelection()], FighterFilter.one);
                    }

                    if (GlobalInputManager.Get().GetSkinToggleLeft(_fighterOneFilter)) {
                        MainMenuSystem.Get().PlaySFX(_skinToggleSFX);
                        _fighterOne.DecreaseSkinIndex(_profiles[_fighterOne.GetSelection()]);
                        _fighterOne.Refresh(_profiles[_fighterOne.GetSelection()], FighterFilter.one);
                    }
                    if (GlobalInputManager.Get().GetSkinToggleRight(_fighterOneFilter)) {
                        MainMenuSystem.Get().PlaySFX(_skinToggleSFX);
                        _fighterOne.IncreaseSkinIndex(_profiles[_fighterOne.GetSelection()]);
                        _fighterOne.Refresh(_profiles[_fighterOne.GetSelection()], FighterFilter.one);
                    }

                    if (GlobalInputManager.Get().GetRandomInput(_fighterOneFilter)) {
                        _fighterOne.RandomCharacter(_profiles.Length);
                        _fighterOne.Refresh(_profiles[_fighterOne.GetSelection()], FighterFilter.one);
                        _fighterOne.SetAsReady();

                        MainMenuSystem.Get().PlaySFXOverlap(_readySFX);

                        if (_type != GameType.pvp) {
                            _canSelectSecondCharacter = true;
                            return;
                        }
                    }

                    if (GlobalInputManager.Get().GetSubmitInput(_fighterOneFilter)) {
                        _fighterOne.SetAsReady();

                        MainMenuSystem.Get().PlaySFXOverlap(_readySFX);

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
                        MainMenuSystem.Get().PlaySFX(_toggleLeftSFX);
                        _fighterTwo.DecreaseCharacterIndex(_profiles.Length);
                        _fighterTwo.Refresh(_profiles[_fighterTwo.GetSelection()], FighterFilter.two);
                    }
                    if (GlobalInputManager.Get().GetRightInput(_fighterTwoFilter)) {
                        MainMenuSystem.Get().PlaySFX(_toggleRightSFX);
                        _fighterTwo.IncreaseCharacterIndex(_profiles.Length);
                        _fighterTwo.Refresh(_profiles[_fighterTwo.GetSelection()], FighterFilter.two);
                    }

                    if (GlobalInputManager.Get().GetSkinToggleLeft(_fighterTwoFilter)) {
                        MainMenuSystem.Get().PlaySFX(_skinToggleSFX);
                        _fighterTwo.DecreaseSkinIndex(_profiles[_fighterTwo.GetSelection()]);
                        _fighterTwo.Refresh(_profiles[_fighterTwo.GetSelection()], FighterFilter.two);
                    }
                    if (GlobalInputManager.Get().GetSkinToggleRight(_fighterTwoFilter)) {
                        MainMenuSystem.Get().PlaySFX(_skinToggleSFX);
                        _fighterTwo.IncreaseSkinIndex(_profiles[_fighterTwo.GetSelection()]);
                        _fighterTwo.Refresh(_profiles[_fighterTwo.GetSelection()], FighterFilter.two);
                    }

                    if (GlobalInputManager.Get().GetRandomInput(_fighterTwoFilter)) {
                        _fighterTwo.RandomCharacter(_profiles.Length);
                        _fighterTwo.Refresh(_profiles[_fighterTwo.GetSelection()], FighterFilter.two);
                        _fighterTwo.SetAsReady();
                        MainMenuSystem.Get().PlaySFXOverlap(_readySFX);
                    }

                    if (GlobalInputManager.Get().GetSubmitInput(_fighterTwoFilter)) {
                        _fighterTwo.SetAsReady();
                        MainMenuSystem.Get().PlaySFXOverlap(_readySFX);
                    }
                }
                else {
                    if (GlobalInputManager.Get().GetBackInput(_fighterTwoFilter)) {
                        _fighterTwo.SetAsUnready();
                        _fighterTwo.Refresh(_profiles[_fighterTwo.GetSelection()], FighterFilter.one);
                    }
                }
            }

            RefreshSelectors();
            RefreshBorders();
        }
    }

    public void RefreshSelectors()
    {
        _selectorOne.localPosition = _selectorPositions[_fighterOne.GetSelection()];
        _selectorTwo.localPosition = _selectorPositions[_fighterTwo.GetSelection()];
        _selectorJoint.localPosition = _selectorPositions[_fighterOne.GetSelection()];
        if (_fighterOne.GetSelection() == _fighterTwo.GetSelection())
        {
            _selectorJoint.gameObject.SetActive(true);
            _selectorOne.gameObject.SetActive(false);
            _selectorTwo.gameObject.SetActive(false);
        }
        else
        {
            _selectorJoint.gameObject.SetActive(false);
            _selectorOne.gameObject.SetActive(true);
            _selectorTwo.gameObject.SetActive(true);
        }
    }

    public void RefreshBorders()
    {
        for (int i = 0; i < _borders.Length; i++)
        {
            _borders[i].SetActive(true);
            if (i == _fighterOne.GetSelection() || i == _fighterTwo.GetSelection())
            {
                _borders[i].SetActive(false);
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
        MainMenuSystem.Get()._playMenuButtons[_selectedIndex].OnFocus();
        MainMenuSystem.Get().SetPage(1);
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

    public void SetSelectedIndex(int index)
    {
        _selectedIndex = index;
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
    [SerializeField] TextMeshProUGUI _characterTagLine;
    [SerializeField] Image _characterIcon;
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
        _characterTagLine.text = "";
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
        _characterTagLine.text = profile.GetTagLine();
        _characterIcon.sprite = profile.GetCircleSprite();

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
            _characterSkin.text = $"<{profile.GetPallete(_currentSkinSelection).name.Split('_')[2]}>";
        }
    }
}
