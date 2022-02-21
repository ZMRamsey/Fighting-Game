using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class OptionsMenuManager : MonoBehaviour
{
    public int _selectionIndex = 0;
    public int _confirmedIndex = 0;
    bool _hasGoneRight = false;
    bool _hasGoneLeft = false;
    int _randSelection = 0;

    public bool _isReady = false;
    public bool _isSelectingMap = false;

    //public GameObject[] characters;
    public GameObject[] buttons;
    public GameObject[] characters;
    public Sprite[] sprites;

    public MapMenuManager man;

    public GameObject[] ConfirmedCharacters;
    public GameObject[] readyButtons;

    public GameObject RuleSetMenu;

    public Sprite spriteDeselected;
    public Sprite spriteSelected;
    public Sprite randSelected;
    public Sprite randDeSelected;

    public int objNum;
    public int objCount;

    public int _maxCharacters = 6;

    void SelectHighlightedCharacter()
    {
        if(_selectionIndex == 3)
        {
            _randSelection = Random.Range(0, 7);
            ConfirmedCharacters[_confirmedIndex].GetComponent<Image>().sprite = sprites[_randSelection];
            ConfirmedCharacters[_confirmedIndex].SetActive(true);
        }
        else
        {
            ConfirmedCharacters[_confirmedIndex].GetComponent<Image>().sprite = sprites[_selectionIndex];
            ConfirmedCharacters[_confirmedIndex].SetActive(true);
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
        man._moveAside = true;
        characters[_selectionIndex].SetActive(false);
        _isSelectingMap = true;
        if(_selectionIndex != 3)
        {
            buttons[_selectionIndex].GetComponent<Image>().sprite = spriteDeselected;
        }
        else
        {
            buttons[_selectionIndex].GetComponent<Image>().sprite = randDeSelected;
        }

        
    }

    void DeselectCharacter()
    {
        ConfirmedCharacters[_confirmedIndex].SetActive(false);
        readyButtons[_confirmedIndex].SetActive(false);
        _confirmedIndex--;
        if (_confirmedIndex < 0)
        {
            _confirmedIndex = 0;
        }
    }

    public void ControlCharacterSelection()
    {
        _isSelectingMap = false;
        _isReady = false;
        _confirmedIndex = 0;
        _hasGoneLeft = false;
        _hasGoneRight = false;

        //WIP
        for(int i = 0; i < 2; i++)
        {
            ConfirmedCharacters[i].SetActive(false);
            readyButtons[i].SetActive(false);
        }
    }

    void HighlightSelectedCharacter()
    {
        //characters[_selectionIndex].SetActive(true);
        if(_hasGoneLeft == true)
        {
            characters[_selectionIndex+1].SetActive(false);
            buttons[_selectionIndex+1].GetComponent<Image>().sprite = spriteDeselected;
        }
        else if(_hasGoneRight == true)
        {
            characters[_selectionIndex-1].SetActive(false);
            buttons[_selectionIndex-1].GetComponent<Image>().sprite = spriteDeselected;
        }
        else
        {
            buttons[_selectionIndex].GetComponent<Image>().sprite = spriteSelected;
        }

        characters[_selectionIndex].SetActive(true);
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

    // Update is called once per frame
    void Update()
    {
        if (!_isSelectingMap)
        {
            if (Keyboard.current.aKey.wasPressedThisFrame)
            {
                _selectionIndex -= 1;
                MoveLeft();
                if (_selectionIndex < 0)
                {
                    _selectionIndex = 0;
                    NotMove();
                }
            }
            else if (Keyboard.current.dKey.wasPressedThisFrame)
            {
                _selectionIndex += 1;
                MoveRight();
                if (_selectionIndex > _maxCharacters)
                {
                    _selectionIndex = _maxCharacters;
                    NotMove();
                }
            }
            else if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                SelectHighlightedCharacter();
            }
            else if (Keyboard.current.zKey.wasPressedThisFrame)
            {
                Debug.Log("" + _confirmedIndex);
                DeselectCharacter();
            }
            else if (!_isReady)
            {
                HighlightSelectedCharacter();
            }

            if (_isReady)
            {
                LoadRuleSetSelect();
            }
        }
        
    }
}
