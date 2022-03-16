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
    protected FighterController _pauseOwner;

    //continue, movelist, characterSelect, returnToMenu
    [SerializeField] TextMeshProUGUI[] _options;

    [SerializeField] Vector3[] _targetPoints;

    [Header("Movelist Left Side")]
    [SerializeField] GameObject[] _movelistOptions;
    [SerializeField] Color[] _colorSet;
    private int _mIndex;

    [Header("Movelist Right Side")]
    [SerializeField] GameObject[] _movelistRightSide;

    private void Awake()
    {
        //SetPauseOwner(GameManager.Get().GetFighterOne());
        //_index = 0;
        //_selector.transform.localPosition = _targetPoints[_index];
    }
    // Update is called once per frame
    void Update()
    {
        if (IsActive())
        {
            if (ReturnLayer() == 1)
            {
                _pauseLayer = 0;
            }
            else if (ReturnLayer() == 0)
            {
                //if ((Gamepad.current != null && Gamepad.current.leftStick.down.wasPressedThisFrame || Gamepad.current.dpad.down.wasPressedThisFrame) || Keyboard.current.sKey.wasPressedThisFrame)
                //if (_pauseOwner.GetHandler().GetCrouchFrame())
                if (Keyboard.current.sKey.wasPressedThisFrame)
                {
                    _options[_index].color = Color.black;
                    _index++;
                    if (_index == 4)
                    {
                        _index = 0;
                    }
                    _options[_index].color = Color.white;
                }

                if (Keyboard.current.wKey.wasPressedThisFrame)
                {
                    _options[_index].color = Color.black;
                    _index--;
                    if (_index == -1)
                    {
                        _index = 3;
                    }
                    _options[_index].color = Color.white;
                }

                _selector.transform.localPosition = _targetPoints[_index];

                if (Keyboard.current.enterKey.wasPressedThisFrame)
                {
                    switch (_index)
                    {
                        case (0):
                            Time.timeScale = 1;
                            Disable();
                            GameManager.Get().EnableUI();
                            break;

                        case (1):
                            SetMovelist();
                            break;

                        case (2):
                            //SceneManager.LoadScene(sceneName: "WinScreenTest");
                            break;

                        case (3):
                            //SceneManager.LoadScene(sceneName: "MenuTest");
                            break;

                        default:
                            break;
                    }

                }
            }

            if (ReturnLayer() == 2)
            {
                if (Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    SetMainPause();
                    _movelistOptions[_mIndex].GetComponent<Image>().color = _colorSet[2];
                    foreach (TextMeshProUGUI textbox in _movelistOptions[_mIndex].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        textbox.color = _colorSet[0];
                    }
                }

                if (Keyboard.current.sKey.wasPressedThisFrame)
                {
                    _movelistOptions[_mIndex].GetComponent<Image>().color = _colorSet[2];
                    foreach (TextMeshProUGUI textbox in _movelistOptions[_mIndex].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        textbox.color = _colorSet[0];
                    }
                    _mIndex++;
                    if (_mIndex == 7)
                    {
                        _mIndex = 0;
                    }
                    _movelistOptions[_mIndex].GetComponent<Image>().color = _colorSet[1];
                    foreach (TextMeshProUGUI textbox in _movelistOptions[_mIndex].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        textbox.color = Color.black;
                    }
                    UpdateRightSide();
                }

                if (Keyboard.current.wKey.wasPressedThisFrame)
                {
                    _movelistOptions[_mIndex].GetComponent<Image>().color = _colorSet[2];
                    foreach (TextMeshProUGUI textbox in _movelistOptions[_mIndex].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        textbox.color = _colorSet[0];
                    }
                    _mIndex--;
                    if (_mIndex == -1)
                    {
                        _mIndex = 6;
                    }
                    _movelistOptions[_mIndex].GetComponent<Image>().color = _colorSet[1];
                    foreach (TextMeshProUGUI textbox in _movelistOptions[_mIndex].GetComponentsInChildren<TextMeshProUGUI>())
                    {
                        textbox.color = Color.black;
                    }
                }
                UpdateRightSide();
            }       
        }      
    }

    public void Enable() 
    { 
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

    public void SetPauseOwner(FighterController owner)
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
        _pausePanel.SetActive(true);
    }

    public void SetMovelist()
    {
        _mIndex = 0;
        _pauseLayer = 2;
        _movelistPanel.SetActive(true);
        _pausePanel.SetActive(false);
        foreach (GameObject option in _movelistOptions)
        {
            option.GetComponent<Image>().color = _colorSet[2];
            foreach (TextMeshProUGUI textbox in option.GetComponentsInChildren<TextMeshProUGUI>())
            {
                textbox.color = _colorSet[0];
            }
        }
        _movelistOptions[0].GetComponent<Image>().color = _colorSet[1];
        _movelistOptions[0].GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
    }

    public void UpdateRightSide()
    {
        _movelistRightSide[0].GetComponent<TextMeshProUGUI>().text = _movelistOptions[_mIndex].GetComponentInChildren<TextMeshProUGUI>().text;
        //_movelistRightSide[1].GetComponent<Image>().sprite;
        //_movelistRightSide[2].GetComponent<TextMeshProUGUI>().text;
    }
}
