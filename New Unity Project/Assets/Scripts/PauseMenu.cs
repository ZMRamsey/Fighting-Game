using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool _active;
    [SerializeField] GameObject _pausePanel;
    [SerializeField] GameObject _movelistPanel;
    [SerializeField] GameObject _selector;
    private int _index;
    private int _pauseLayer;
    protected FighterFilter _pauseOwner;

    //continue, movelist, characterSelect, returnToMenu
    [SerializeField] TextMeshProUGUI[] _options;

    [SerializeField] Vector3[] _targetPoints;

    [Header("Movelist Left Side")]
    [SerializeField] GameObject[] _movelistOptions;
    [SerializeField] Color[] _colorSet;
    [SerializeField] GameObject _universalArrows;
    [SerializeField] GameObject _characterArrows;
    private int _mIndex;

    [SerializeField] MoveListOption[] _topSideStrikes;
    [SerializeField] MoveListOption[] _topSideUniversal;

    [Header("Movelist Right Side")]
    [SerializeField] GameObject[] _movelistRightSide;
    [SerializeField] Sprite[] _rightSideSprites;
    [SerializeField] string[] _rightSideDescriptions;
    [SerializeField] FighterProfile[] _fighterProfiles;
    private int _rIndex;

    private void Awake()
    {
    }
    
    void Update()
    {
        if (IsActive())
        {
            switch (ReturnLayer())
            {
                case (0):
                    OnMainPause();
                    break;
                case (1):
                    _pauseLayer = 0;
                    break;
                case (2):
                    OnMoveList();
                    break;
                default:
                    break;
            }
        }
    }

    public void OnMainPause()
    {
        if (GlobalInputManager.Get().GetDownInput(_pauseOwner))
        {
            _options[_index].color = Color.black;
            _index++;
            if (_index == 4)
            {
                _index = 0;
            }
            _options[_index].color = Color.white;
            _selector.transform.localPosition = _targetPoints[_index];
        }

        if (GlobalInputManager.Get().GetUpInput(_pauseOwner))
        {
            _options[_index].color = Color.black;
            _index--;
            if (_index == -1)
            {
                _index = 3;
            }
            _options[_index].color = Color.white;
            _selector.transform.localPosition = _targetPoints[_index];
        }

        if (GlobalInputManager.Get().GetPauseInput(_pauseOwner))
        {
            GameManager.Get().Resume();
        }

        if (GlobalInputManager.Get().GetSubmitInput(_pauseOwner))
        {
            switch (_index)
            {
                case (0):
                    Resume();
                    break;

                case (1):
                    SetMovelist();
                    break;

                case (2):
                    //SceneManager.LoadScene(sceneName: "WinScreenTest");
                    break;

                case (3):
                    //SceneManager.LoadScene(sceneName: "MenuTest");
                    Resume();
                    GameManager.Get().KillSwitch();
                    GameLogic.Get().LoadScene("MenuTest", "Base");
                    break;

                default:
                    break;
            }
        }
    }

    public void OnMoveList()
    {
        if (GlobalInputManager.Get().GetPauseInput(_pauseOwner) || GlobalInputManager.Get().GetBackInput())
        {
            DeselectLeftSide();
            SetMainPause();
        }

        if (GlobalInputManager.Get().GetDownInput(_pauseOwner))
        {
            DeselectLeftSide();
            _mIndex++;
            if (_mIndex == 8)
            {
                _mIndex = 0;
            }
            SelectLeftSide();
        }

        if (GlobalInputManager.Get().GetUpInput(_pauseOwner))
        {
            DeselectLeftSide();
            _mIndex--;
            if (_mIndex == -1)
            {
                _mIndex = 7;
            }
            SelectLeftSide();
        }


        if ((GlobalInputManager.Get().GetLeftInput(_pauseOwner) || GlobalInputManager.Get().GetRightInput(_pauseOwner)) && _mIndex == 0)
        {
            if(_movelistOptions[0].GetComponentInChildren<TextMeshProUGUI>().text == "Strike")
            {
                SetTopSet(_topSideUniversal);
            }
            else
            {
                SetTopSet(_topSideStrikes);
            }
        }

        if (GlobalInputManager.Get().GetRightInput(_pauseOwner) && _mIndex == 5)
        {
            _rIndex++;
            if (_rIndex == 8)
            {
                _rIndex = 0;
            }
            ChangeSpecialCharacter();
        }

        if (GlobalInputManager.Get().GetLeftInput(_pauseOwner) && _mIndex == 5)
        {
            _rIndex--;
            if (_rIndex == -1)
            {
                _rIndex = 7;
            }
            ChangeSpecialCharacter();
        }
    }

    void Resume() {
        Disable();
        GameManager.Get().Resume();
        GameManager.Get().EnableUI();
    }

    public void Enable() 
    {
        GameManager.Get().SetSounds(0, 424);
        _active = true;
        _options[_index].color = Color.black;
        _index = 0;
        _pauseLayer = 0;
        _selector.transform.localPosition = _targetPoints[_index];
        _options[_index].color = Color.white;
        _pausePanel.SetActive(true);
    }
    public void Disable()
    {
        _active = false;
        _pausePanel.SetActive(false);
    }

    public bool IsActive()
    {
        return _active;
    }

    public void SetPauseOwner(FighterFilter owner)
    {
        _pauseOwner = owner;
    }

    public int ReturnLayer()
    {
        return _pauseLayer;
    }

    public void SetMainPause()
    {
        _pauseLayer = 1;
        _movelistPanel.SetActive(false);
        ResetLeftSide();
        _pausePanel.SetActive(true);
    }

    public void SetMovelist()
    {
        _rIndex = 0;
        foreach (FighterProfile profile in _fighterProfiles)
        {
            if (GameManager.Get().GetGameSettings().GetFighterProfile(_pauseOwner) == profile)
            {
                break;
            }
            _rIndex++;
        }
        _mIndex = 0;
        _pauseLayer = 2;
        ChangeSpecialCharacter();
        _movelistPanel.SetActive(true);
        _pausePanel.SetActive(false);
    }

    public void ResetLeftSide()
    {
        _universalArrows.GetComponent<Image>().color = Color.white;
        _characterArrows.GetComponent<Image>().color = _colorSet[1];
        _movelistOptions[0].GetComponent<Image>().color = _colorSet[1];
        _movelistOptions[0].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
    }

    public void DeselectLeftSide()
    {
        _movelistOptions[_mIndex].GetComponent<Image>().color = _colorSet[2];
        foreach (TextMeshProUGUI textbox in _movelistOptions[_mIndex].GetComponentsInChildren<TextMeshProUGUI>())
        {
            textbox.color = _colorSet[0];
        }
        if (_mIndex == 0)
        {
            _universalArrows.GetComponent<Image>().color = _colorSet[1];
            _universalArrows.GetComponent<Animator>().SetBool("Idle", true);
            UpdateRightSide();
        }
        else if (_mIndex == 5)
        {
            _characterArrows.GetComponent<Image>().color = _colorSet[1];
            _characterArrows.GetComponent<Animator>().SetBool("Idle", true);
        }
    }

    public void SelectLeftSide()
    {
        _movelistOptions[_mIndex].GetComponent<Image>().color = _colorSet[1];
        foreach (TextMeshProUGUI textbox in _movelistOptions[_mIndex].GetComponentsInChildren<TextMeshProUGUI>())
        {
            textbox.color = Color.white;
        }
        if (_mIndex == 0)
        {
            _universalArrows.GetComponent<Image>().color = Color.white;
            _universalArrows.GetComponent<Animator>().SetBool("Idle", false);
            UpdateRightSide();
        }
        else if (_mIndex == 5)
        {
            _characterArrows.GetComponent<Image>().color = Color.white;
            _characterArrows.GetComponent<Animator>().SetBool("Idle", false);
            ChangeSpecialCharacter();
        }
        else
        {
            UpdateRightSide();
        }
    }

    public void UpdateRightSide()
    {
        //Final
        _movelistRightSide[0].GetComponent<TextMeshProUGUI>().text = _movelistOptions[_mIndex].GetComponentInChildren<TextMeshProUGUI>().text;
        //Temp
        _movelistRightSide[1].GetComponent<Image>().sprite = _rightSideSprites[_mIndex];
        //Temp?
        _movelistRightSide[2].GetComponent<TextMeshProUGUI>().text = _rightSideDescriptions[_mIndex];
    }

    public void ChangeSpecialCharacter()
    {
        _movelistOptions[5].GetComponentInChildren<TextMeshProUGUI>().text = _fighterProfiles[_rIndex].GetName();
        _movelistOptions[6].GetComponentInChildren<TextMeshProUGUI>().text = _fighterProfiles[_rIndex].GetSuperName();
        _movelistOptions[7].GetComponentInChildren<TextMeshProUGUI>().text = _fighterProfiles[_rIndex].GetGimmickName();

        _rightSideSprites[5] = _fighterProfiles[_rIndex].GetIconSprite();
        _rightSideSprites[6] = _fighterProfiles[_rIndex].GetIconSprite();
        _rightSideSprites[7] = _fighterProfiles[_rIndex].GetIconSprite();

        _rightSideDescriptions[5] = _fighterProfiles[_rIndex].GetFighterDesc();
        _rightSideDescriptions[6] = _fighterProfiles[_rIndex].GetSuperDesc();
        _rightSideDescriptions[7] = _fighterProfiles[_rIndex].GetGimmickDesc();

        UpdateRightSide();
    }
    public void SetTopSet(MoveListOption[] set)
    {
        for (int i = 0; i < 5; i++)
        {
            _movelistOptions[i].GetComponentInChildren<TextMeshProUGUI>().text = set[i].GetOptionName();
            //_rightSideSprites[0] = _fighterProfiles[_rIndex].GetIconSprite();
            _rightSideDescriptions[i] = set[i].GetOptionDesc();
        }
        UpdateRightSide();
    }
}
