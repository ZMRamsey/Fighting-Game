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
    [SerializeField] GameObject _selector;
    private int _index;
    protected FighterController _pauseOwner;

    //[SerializeField] TextMeshProUGUI _continue;
    //[SerializeField] TextMeshProUGUI _movelist;
    //[SerializeField] TextMeshProUGUI _characterSelect;
    //[SerializeField] TextMeshProUGUI _returnToMenu;

    //continue, movelist, characterSelect, returnToMenu
    [SerializeField] TextMeshProUGUI[] _options;

    [SerializeField] Vector3[] _targetPoints;

    private void Awake()
    {
        //SetPauseOwner(GameManager.Get().GetFighterOne());
        //_index = 0;
        //_selector.transform.localPosition = _targetPoints[_index];
    }
    // Update is called once per frame
    void Update()
    {
        //if ((Gamepad.current != null && Gamepad.current.leftStick.down.wasPressedThisFrame || Gamepad.current.dpad.down.wasPressedThisFrame) || Keyboard.current.sKey.wasPressedThisFrame)
        //if (_pauseOwner.GetHandler().GetCrouchFrame())
        if (Keyboard.current.sKey.wasPressedThisFrame)
            {
            _options[_index].color = Color.black;
            _index++;
            if(_index == 4)
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
                    
                    break;

                case (2):
                    //SceneManager.LoadScene(sceneName: "WinScreenTest");
                    break;

                case (3):
                    SceneManager.LoadScene(sceneName: "MenuTest");
                    break;

                default:
                    break;
            }

        }
    }

    public void Enable() 
    { 
        _active = true;
        _pausePanel.SetActive(true);
    }
    public void Disable()
    {
        _active = false;
        _pausePanel.SetActive(false);
        _options[_index].color = Color.black;
        _index = 0;
        _selector.transform.localPosition = _targetPoints[_index];
        _options[_index].color = Color.white;
    }

    public bool IsActive()
    {
        return _active;
    }

    public void SetPauseOwner(FighterController owner)
    {
        _pauseOwner = owner;
    }
}
