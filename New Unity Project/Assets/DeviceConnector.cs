using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeviceConnector : MonoBehaviour
{
    Keyboard _keyboard;
    Gamepad _gamepad;
    InputTest _test;
    bool _assignedPlayer;
    public void AddDevice(Keyboard keyboard, Gamepad gamepad, InputTest test) {
        _keyboard = keyboard;
        _gamepad = gamepad;
        _test = test;
    }

    void Update()
    {
        if (_assignedPlayer) {
            return;
        }

        if(_keyboard != null) {
            if (_keyboard.anyKey.wasPressedThisFrame) {
                _test.SetPlayer(_keyboard);
                _assignedPlayer = true;
                return;
            }
        }

        if(_gamepad != null) {
            if (_gamepad.aButton.wasPressedThisFrame) {
                _test.SetPlayer(_gamepad);
                _assignedPlayer = true;
                return;
            }
        }
    }
}
