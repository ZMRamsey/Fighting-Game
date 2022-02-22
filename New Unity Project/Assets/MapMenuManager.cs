using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MapMenuManager : MonoBehaviour
{
    public GameObject C1;
    public GameObject C2;
    public GameObject CSButtons;
    public GameObject RButtons;
    public GameObject RMapPreview;
    public GameObject RMap;
    public GameObject RReady;

    public OptionsMenuManager opMan;

    public GameObject[] buttons;
    public Sprite[] maps;

    public Sprite randSelected;
    public Sprite randDeselected;
    public Sprite spriteSelected;
    public Sprite spriteDeselected;

    public Sprite leftMap;
    public Sprite rightMap;

    public bool _moveAside = false;
    public bool _moveToInit = false;
    bool _hasGoneLeft = false;
    bool _hasGoneRight = false;

    bool _mapSelected = false;

    int _rSelectionIndex = 0;
    int _confirmedIndex = 0;
    int _maxMaps = 2;
    int _randSelection = 0;
    int _selectedMap = 0;

    Vector3 _C1Init = new Vector3(213.7393f, 429.3754f, 90f);
    Vector3 _C2Init = new Vector3(1033.264f, 429.3754f, 90f);

    Vector3 _C1Final = new Vector3(100.9844f, 215.1673f, 90f);
    Vector3 _C2Final = new Vector3(1146.0189f, 215.1673f, 90f);

    Vector3 _CBTNSInit = new Vector3(620.8455f, 310.5f, 90f);
    Vector3 _CBTNSFinal = new Vector3(620.8455f, -500f, 90f);

    Vector3 _RuleBTNSInit = new Vector3(620.8455f, -500f, 90f);
    Vector3 _RuleBTNSFinal = new Vector3(620.8455f, 318.6927f, 90f);

    Vector3 _MapPreviewInit = new Vector3(608.9289f, 1000.5408f, 0f);
    Vector3 _MapPreviewFinal = new Vector3(608.9289f, 447.5408f, 0f);


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


    void MoveAside()
    {
        C1.transform.position = Vector3.MoveTowards(C1.transform.position,_C1Final, 1600f*Time.deltaTime);
        C2.transform.position = Vector3.MoveTowards(C2.transform.position, _C2Final, 1600f * Time.deltaTime);
        CSButtons.transform.position = Vector3.MoveTowards(CSButtons.transform.position, _CBTNSFinal, 5000f * Time.deltaTime);
        RButtons.transform.position = Vector3.MoveTowards(RButtons.transform.position, _RuleBTNSFinal, 5000f * Time.deltaTime);
        RMapPreview.transform.position = Vector3.MoveTowards(RMapPreview.transform.position, _MapPreviewFinal, 5000f * Time.deltaTime);

    }
    void MoveToInit()
    {
        C1.transform.position = Vector3.MoveTowards(C1.transform.position, _C1Init, 1600f * Time.deltaTime);
        C2.transform.position = Vector3.MoveTowards(C2.transform.position, _C2Init, 1600f * Time.deltaTime);
        CSButtons.transform.position = Vector3.MoveTowards(CSButtons.transform.position, _CBTNSInit, 5000f * Time.deltaTime);
        RButtons.transform.position = Vector3.MoveTowards(RButtons.transform.position, _RuleBTNSInit, 5000f * Time.deltaTime);
        RMapPreview.transform.position = Vector3.MoveTowards(RMapPreview.transform.position, _MapPreviewInit, 5000f * Time.deltaTime);
    }

    void HighlightSelectedMap()
    {
        if (_hasGoneLeft)
        {
            buttons[_rSelectionIndex + 1].GetComponent<Image>().sprite = spriteDeselected;
        }
        else if (_hasGoneRight)
        {
            buttons[_rSelectionIndex - 1].GetComponent<Image>().sprite = spriteDeselected;
        }

        buttons[_rSelectionIndex].GetComponent<Image>().sprite = spriteSelected;

        if(_rSelectionIndex == 1)
        {
            buttons[1].GetComponent<Image>().sprite = randSelected;
        }
        else
        {
            buttons[1].GetComponent<Image>().sprite = randDeselected;
        }

        if(_rSelectionIndex == 0)
        {
            RMap.GetComponent<Image>().sprite = maps[0];
        }
        else if(_rSelectionIndex == 2)
        {
            RMap.GetComponent<Image>().sprite = maps[1];
        }
    }

    void SelectHighlightedMap()
    {
        if(_rSelectionIndex == 1)
        {
            _randSelection = Random.Range(0, _maxMaps);
            _selectedMap = _randSelection;

        }
        else
        {
            if(_rSelectionIndex == 2)
            {
                _selectedMap = 1;
            }
            else
            {
                _selectedMap = _rSelectionIndex;
            }
            
        }
        RMap.GetComponent<Image>().sprite = maps[_selectedMap];
        ReadyUp();
        _mapSelected = true;
    }

    void ReadyUp()
    {
        RReady.SetActive(true);
        _mapSelected = true;
    }

    void DeselectHighlightedMenu()
    {
        RReady.SetActive(false);
        _mapSelected = false;
    }
    void Update()
    {
        if (_moveAside)
        {
            MoveAside();
        }
        else if (!_moveAside)
        {
            MoveToInit();
        }

        if (RButtons.transform.position == _RuleBTNSFinal)
        {
            if (!_mapSelected)
            {
                if (Keyboard.current.aKey.wasPressedThisFrame || Gamepad.current.leftStick.left.wasPressedThisFrame)
                {
                    _rSelectionIndex -= 1;
                    MoveLeft();
                    if (_rSelectionIndex < 0)
                    {
                        _rSelectionIndex = 0;
                        NotMove();
                    }
                }
                else if (Keyboard.current.dKey.wasPressedThisFrame || Gamepad.current.leftStick.right.wasPressedThisFrame)
                {
                    _rSelectionIndex += 1;
                    MoveRight();
                    if (_rSelectionIndex > _maxMaps)
                    {
                        _rSelectionIndex = _maxMaps;
                        NotMove();
                    }
                }
                HighlightSelectedMap();

                if (Keyboard.current.zKey.wasPressedThisFrame || Gamepad.current.buttonEast.wasPressedThisFrame)
                {
                    _moveAside = false;
                    opMan.ControlCharacterSelection();
                }
            }
            if (Keyboard.current.spaceKey.wasPressedThisFrame || Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                SelectHighlightedMap();
            }
            else if (Keyboard.current.zKey.wasPressedThisFrame || Gamepad.current.buttonEast.wasPressedThisFrame)
            {
                DeselectHighlightedMenu();
            }
            
            
        }
        
    }
}
