using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class OptionsMenuManager : MonoBehaviour
{

    private void Start()
    {
        characterSelectPanel.GetComponent<Image>().sprite = characterSelectPanelSprite;
    }

    public GameObject MMenuScript;

    public int _selectionIndex = 0;
    public int _confirmedIndex = 0;
    bool _hasGoneRight = false;
    bool _hasGoneLeft = false;
    int _randSelection = 0;

    [SerializeField] GameSettings _settings;
    [SerializeField] FighterProfile[] _profiles;

    public int player1Char;
    public int player2Char;

    string _selectedCharacter;

    public bool _isReady = false;
    public bool _isSelectingMap = false;

    public MapMenuManager man;

    public GameObject characterSelectPanel;
    public Sprite characterSelectPanelSprite;

    public GameObject[] readyButtons;
    public GameObject[] fighters;

    public GameObject RuleSetMenu;

    public GameObject opMenu;
    public GameObject mMenu;
    public GameObject bg;

    //public GameObject[] characters;
    public GameObject[] buttons;
    public GameObject[] characters;
    public Sprite[] sprites;

    public Sprite[] esme;

    int[] _selectedChar = new int[2];

    public GameObject[] ConfirmedCharacters;
    public GameObject[] animatedCharactersLeft;
    public GameObject[] animatedCharactersRight;
    public GameObject[] animatedCharactersCentre;

    public Sprite spriteDeselected;
    public Sprite spriteSelected;
    public Sprite randSelected;
    public Sprite randDeSelected;
    public Sprite mBG;

    public int objNum;
    public int objCount;

    public int _maxCharacters = 6;

    void SelectHighlightedCharacter()
    {
        if(_confirmedIndex == 0)
        {
            if (_selectionIndex == 3)
            {
                _randSelection = Random.Range(0, 7);
                _selectionIndex = _randSelection;
                animatedCharactersLeft[_selectionIndex].SetActive(true);
            }
            else
            {
                animatedCharactersLeft[_selectionIndex].SetActive(true);
            }
            _settings.SetFighterOneProfile(_profiles[_selectionIndex]);
        }
        else
        {
            if (_selectionIndex == 3)
            {
                _randSelection = Random.Range(0, 7);
                _selectionIndex = _randSelection;
                animatedCharactersRight[_selectionIndex].SetActive(true);
            }
            else
            {
                animatedCharactersRight[_selectionIndex].SetActive(true);
            }
            _settings.SetFighterTwoProfile(_profiles[_selectionIndex]);
        }
        

        readyButtons[_confirmedIndex].SetActive(true);
        _confirmedIndex++;

        if (_confirmedIndex > 1)
        {
            _isReady = true;
            _confirmedIndex = 1;
        }
    }

    void LoadRuleSetSelect()
    {

        animatedCharactersCentre[_selectionIndex].SetActive(false);
        Debug.Log("Selection Index: " + _selectionIndex);
        man._moveAside = true;
        _isSelectingMap = true;
        if (_selectionIndex != 3)
        {
            buttons[_selectionIndex].GetComponent<Image>().sprite = spriteDeselected;
        }
        else
        {
            buttons[_selectionIndex].GetComponent<Image>().sprite = randDeSelected;
        }
        


    }

    public void ControlCharacterSelection()
    {
        _isSelectingMap = false;
        _isReady = false;
        _confirmedIndex = 1;
        _hasGoneLeft = false;
        _hasGoneRight = false;

        //WIP
        //for (int i = 0; i < 2; i++)
        //{
        //    ConfirmedCharacters[i].SetActive(false);
        //    readyButtons[i].SetActive(false);
        //}
    }

    void DeselectCharacter()
    {
        if(_confirmedIndex == 0)
        {
            for (int i = 0; i < 7; i++)
            {
                animatedCharactersLeft[i].SetActive(false);
            }
        }
        else if(_confirmedIndex == 1)
        {
            for (int i = 0; i < 7; i++)
            {
                animatedCharactersRight[i].SetActive(false);
            }
        }

        for (int i = 0; i < 7; i++)
        {
            animatedCharactersRight[i].SetActive(false);
            animatedCharactersLeft[i].SetActive(false);
        }

        for (int i = 0; i < 2; i++)
        {
            readyButtons[i].SetActive(false);
        }
        readyButtons[_confirmedIndex].SetActive(false);
        _confirmedIndex--;
        if (_confirmedIndex < 0)
        {
            _confirmedIndex = 0;
        }
    }

    void HighlightSelectedCharacter()
    {
        //characters[_selectionIndex].SetActive(true);
        if(_hasGoneLeft == true)
        {
            animatedCharactersCentre[_selectionIndex+1].SetActive(false);
            buttons[_selectionIndex+1].GetComponent<Image>().sprite = spriteDeselected;
        }
        else if(_hasGoneRight == true)
        {
            animatedCharactersCentre[_selectionIndex-1].SetActive(false);
            buttons[_selectionIndex-1].GetComponent<Image>().sprite = spriteDeselected;
        }

        animatedCharactersCentre[_selectionIndex].SetActive(true);
        buttons[_selectionIndex].GetComponent<Image>().sprite = spriteSelected;

        if (_selectionIndex == 3)
        {
            buttons[3].GetComponent<Image>().sprite = randSelected;
        }
        else
        {
            buttons[3].GetComponent<Image>().sprite = randDeSelected;
        }
        
    }

    void MoveLeft()
    {
        _hasGoneLeft = true;
        _hasGoneRight = false;
    }
    void MoveRight()
    {
        _hasGoneLeft = false;
        _hasGoneRight = true;
    }
    void NotMove()
    {
        _hasGoneLeft = false;
        _hasGoneRight = false;
    }

    void ReturnToMain()
    {
        opMenu.SetActive(false);
        mMenu.SetActive(true);
        bg.GetComponent<Image>().sprite = mBG;
    }

    void ResetCharacterSelection()
    {
        _isSelectingMap = false;
        for(int i = 0; i < 7; i++)
        {
            animatedCharactersLeft[i].SetActive(false);
            animatedCharactersRight[i].SetActive(false);
        }
        for (int i = 0; i < 2; i++)
        {
            readyButtons[i].SetActive(false);
        }
        _selectionIndex = 0;
        _hasGoneLeft = false;
        _hasGoneRight = false;
        _isReady = false;
        MMenuScript.GetComponent<rotaterCircle>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isSelectingMap)
        {
            
            if (Keyboard.current.aKey.wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.leftStick.left.wasPressedThisFrame))
            {
                _selectionIndex -= 1;
                MoveLeft();
                if (_selectionIndex < 0)
                {
                    _selectionIndex = 0;
                    NotMove();
                }
            }
            else if (Keyboard.current.dKey.wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.leftStick.right.wasPressedThisFrame))
            {
                _selectionIndex += 1;
                MoveRight();
                if (_selectionIndex > _maxCharacters)
                {
                    _selectionIndex = _maxCharacters;
                    NotMove();
                }
            }
            else if (Keyboard.current.spaceKey.wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame))
            {
                SelectHighlightedCharacter();
            }
            else if (Keyboard.current.zKey.wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame))
            {
                if(_confirmedIndex == 0)
                {
                    ReturnToMain();
                }
                else
                {
                    DeselectCharacter();
                }
                
            }
            
            if (!_isReady)
            {
                HighlightSelectedCharacter();
            }

            if (_isReady)
            {
                ResetCharacterSelection();
                LoadRuleSetSelect();
            }
            //HighlightSelectedCharacter();

            
        }
        else
        {
            characters[_selectionIndex].SetActive(false);
        }

        //if (_selectedChar[0] == 1)
        //{
        //    for (int i = 0; i < 10; i++)
        //    {
        //        Debug.Log(i);
        //        ConfirmedCharacters[0].GetComponent<Image>().sprite = esme[i/2];
        //    }
        //}
        //Debug.Log(_confirmedIndex);
    }
}
