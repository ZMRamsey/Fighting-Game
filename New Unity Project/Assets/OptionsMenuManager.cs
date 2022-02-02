using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class OptionsMenuManager : MonoBehaviour
{
    int _selectionIndex = 0;
    bool _hasGoneRight = false;
    bool _hasGoneLeft = false;

    //public GameObject[] characters;
    public GameObject[] buttons;
    public GameObject[] characters;

    public Sprite spriteDeselected;
    public Sprite spriteSelected;
    public Sprite randSelected;
    public Sprite randDeSelected;

    public int objNum;
    public int objCount;

    public int _maxCharacters = 4;

    void HighlightSelectedCharacter()
    {
        //characters[_selectionIndex].SetActive(true);
        if(_hasGoneLeft == true)
        {
            characters[_selectionIndex].SetActive(true);
            characters[_selectionIndex+1].SetActive(false);
            buttons[_selectionIndex].GetComponent<Image>().sprite = spriteSelected;
            buttons[_selectionIndex+1].GetComponent<Image>().sprite = spriteDeselected;
        }
        else if(_hasGoneRight == true)
        {
            characters[_selectionIndex].SetActive(true);
            characters[_selectionIndex-1].SetActive(false);
            buttons[_selectionIndex].GetComponent<Image>().sprite = spriteSelected;
            buttons[_selectionIndex-1].GetComponent<Image>().sprite = spriteDeselected;
        }

        if(_selectionIndex == 2)
        {
            buttons[2].GetComponent<Image>().sprite = randSelected;
        }
        else
        {
            buttons[2].GetComponent<Image>().sprite = randDeSelected;
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
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            _selectionIndex -= 1;
            MoveLeft();
            if(_selectionIndex < 0)
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
        HighlightSelectedCharacter();
    }
}
