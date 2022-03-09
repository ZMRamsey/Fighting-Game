using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class InputTest : MonoBehaviour
{
    public rotaterCircle rot;

    PlayerDevice _playerOne;
    Gamepad _playerOneGamepad;
    Keyboard _playerOneKeyboard;
    PlayerDevice _playerTwo;
    Gamepad _playerTwoGamepad;
    Keyboard _playerTwoKeyboard;
    bool setPlayerOne;
    bool setPlayerTwo;
    int _f1Index;
    int _f2Index;

    [SerializeField] GameSettings _settings;
    [SerializeField] FighterProfile[] _profiles;


    // Start is called before the first frame update
    void Start()
    {
        //_f1Char.color = Color.grey;
        //_f2Char.color = Color.grey;

        //_f1Char.text = _profiles[0].GetName();
        //_f2Char.text = _profiles[0].GetName();

        _settings.SetFighterOneProfile(_profiles[0]);
        _settings.SetFighterTwoProfile(_profiles[0]);

        if(rot._PlayerOptionSelected == 0)
        {
            SetPVAI();
            Debug.Log("Player VS AI");
        }
    }

    void Update() {
        var allGamepads = Gamepad.all;

        foreach (Gamepad device in allGamepads) {
            if (device.aButton.wasPressedThisFrame) {
                SetPlayer(device);
            }
        }

        if (Keyboard.current != null) {
            if (Keyboard.current.enterKey.wasPressedThisFrame) {
                SetPlayer(Keyboard.current);
            }
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame || Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame) {
            if (!setPlayerOne && !setPlayerTwo) {
                SetAIVAI();
            }
            else if (setPlayerOne && setPlayerTwo) {
                SetPVP();
                print("PVP");
            }
            else if (setPlayerOne) {
                SetPVAI();
            }
        }

        //DONT NEED TO REFERENCE THIS, TEMPORARY CHARACTER SELECTION
        //    if (_playerOneKeyboard != null) {
        //        if (_playerOneKeyboard.leftArrowKey.wasPressedThisFrame || _playerOneKeyboard.aKey.wasPressedThisFrame) {
        //            _f1Index++;
        //            if (_f1Index > _profiles.Length - 1) {
        //                _f1Index = 0;
        //            }
        //            _f1Char.text = _profiles[_f1Index].GetName();
        //        }

        //        if (_playerOneKeyboard.rightArrowKey.wasPressedThisFrame || _playerOneKeyboard.dKey.wasPressedThisFrame) {
        //            _f1Index--;
        //            if (_f1Index < 0) {
        //                _f1Index = _profiles.Length - 1;
        //            }
        //            _f1Char.text = _profiles[_f1Index].GetName();
        //            _settings.SetFighterOneProfile(_profiles[_f1Index]);
        //        }
        //    }

        //    if (_playerTwoKeyboard != null) {
        //        if (_playerTwoKeyboard.leftArrowKey.wasPressedThisFrame || _playerTwoKeyboard.aKey.wasPressedThisFrame) {
        //            _f2Index++;
        //            if (_f2Index > _profiles.Length - 1) {
        //                _f2Index = 0;
        //            }
        //            _f2Char.text = _profiles[_f2Index].GetName();
        //        }

        //        if (_playerTwoKeyboard.rightArrowKey.wasPressedThisFrame || _playerTwoKeyboard.dKey.wasPressedThisFrame) {
        //            _f2Index--;
        //            if (_f2Index < 0) {
        //                _f2Index = _profiles.Length - 1;
        //            }
        //            _f2Char.text = _profiles[_f2Index].GetName();
        //            _settings.SetFighterTwoProfile(_profiles[_f2Index]);
        //        }
        //    }

        //    if (_playerOneGamepad != null) {
        //        if (_playerOneGamepad.dpad.left.wasPressedThisFrame || _playerOneGamepad.leftStick.left.wasPressedThisFrame) {
        //            _f1Index++;
        //            if (_f1Index > _profiles.Length - 1) {
        //                _f1Index = 0;
        //            }
        //            _f1Char.text = _profiles[_f1Index].GetName();
        //        }

        //        if (_playerOneGamepad.dpad.right.wasPressedThisFrame || _playerOneGamepad.leftStick.right.wasPressedThisFrame) {
        //            _f1Index--;
        //            if (_f1Index < 0) {
        //                _f1Index = _profiles.Length - 1;
        //            }
        //            _f1Char.text = _profiles[_f1Index].GetName();
        //            _settings.SetFighterOneProfile(_profiles[_f1Index]);
        //        }
        //    }

        //    if (_playerTwoGamepad!= null) {
        //        if (_playerTwoGamepad.dpad.left.wasPressedThisFrame || _playerTwoGamepad.leftStick.left.wasPressedThisFrame) {
        //            _f2Index++;
        //            if (_f2Index > _profiles.Length - 1) {
        //                _f2Index = 0;
        //            }
        //            _f2Char.text = _profiles[_f2Index].GetName();
        //        }

        //        if (_playerTwoGamepad.dpad.right.wasPressedThisFrame || _playerTwoGamepad.leftStick.right.wasPressedThisFrame) {
        //            _f2Index--;
        //            if (_f2Index < 0) {
        //                _f2Index = _profiles.Length - 1;
        //            }
        //            _f2Char.text = _profiles[_f2Index].GetName();
        //            _settings.SetFighterTwoProfile(_profiles[_f2Index]);
        //        }
        //    }
    }

    public void SetAIVAI() {
        _settings.GetFighterOneDevice().SetInputState(InputState.ai);
        _settings.GetFighterTwoDevice().SetInputState(InputState.ai);
    }

    public void SetPVAI() {
        _settings.GetFighterTwoDevice().SetInputState(InputState.ai);
    }

    public void SetPVP() {

    }

    public void SetPlayer(Keyboard keyboard) {
        if(setPlayerOne && _playerOne.GetDeviceID() == keyboard.deviceId) {
            return;
        }

        if (!setPlayerOne) {
            //_f1Char.color = Color.white;
            //_f1Text.text = "KEYBOARD";
            _playerOne = new PlayerDevice(InputDeviceType.keyboard, keyboard.deviceId);
            _settings.GetFighterOneDevice().SetDevice(_playerOne);
            _settings.GetFighterOneDevice().SetInputState(InputState.player);
            setPlayerOne = true;
            _playerOneKeyboard = keyboard;
            return;
        }

        if (!setPlayerTwo) {
            //_f2Char.color = Color.white;
            //_f2Text.text = "KEYBOARD";
            _playerTwo = new PlayerDevice(InputDeviceType.keyboard, keyboard.deviceId);
            _settings.GetFighterTwoDevice().SetDevice(_playerTwo);
            _settings.GetFighterTwoDevice().SetInputState(InputState.player);
            setPlayerTwo = true;
            _playerTwoKeyboard = keyboard;
            //Application.LoadLevel("Base");
            return;
        }
    }

    public void SetPlayer(Gamepad gamepad) {
        if (setPlayerOne && _playerOne.GetDeviceID() == gamepad.deviceId) {
            return;
        }

        if (!setPlayerOne) {
            //_f1Char.color = Color.white;
            //_f1Text.text = "GAMEPAD";
            _playerOne = new PlayerDevice(InputDeviceType.gamepad, gamepad.deviceId);
            _settings.GetFighterOneDevice().SetDevice(_playerOne) ;
            _settings.GetFighterOneDevice().SetInputState(InputState.player);
            setPlayerOne = true;
            _playerOneGamepad = gamepad;
            return;
        }

        if (!setPlayerTwo) {
            //_f2Char.color = Color.white;
            //_f2Text.text = "GAMEPAD";
            _playerTwo = new PlayerDevice(InputDeviceType.gamepad, gamepad.deviceId);
            _settings.GetFighterTwoDevice().SetDevice(_playerTwo);
            _settings.GetFighterTwoDevice().SetInputState(InputState.player);
            setPlayerTwo = true;
            _playerTwoGamepad = gamepad;
            //Application.LoadLevel("Base");
            return;
        }
    }
}
