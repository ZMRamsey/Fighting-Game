using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum ConnectorPos { left, middle, right };
public class DeviceConnector : MonoBehaviour
{
    Keyboard _keyboard;
    Gamepad _gamepad;

    InputSelectionSystem _root;
    bool _isConfirmed;

    Transform _homeTarget;
    Vector3 _currentTarget;

    [SerializeField] Sprite _keyboardSprite, _controllerSprite;
    [SerializeField] Image _renderer;

    [SerializeField] Image _leftDirection;
    [SerializeField] Image _rightDirection;

    ConnectorPos _pos;

    PlayerDevice _device;

    public void AddDevice(Keyboard keyboard, Gamepad gamepad, Transform homeTarget, InputSelectionSystem root) {
        _keyboard = keyboard;
        _gamepad = gamepad;
        _root = root;
        _homeTarget = homeTarget;

        _pos = ConnectorPos.middle;

        if (keyboard != null) {
            _renderer.sprite = _keyboardSprite;
            _device = new PlayerDevice(InputDeviceType.keyboard, keyboard.deviceId);
        }

        if (gamepad != null) {
            _device = new PlayerDevice(InputDeviceType.gamepad, gamepad.deviceId);
            _renderer.sprite = _controllerSprite;
        }

    }

    void Update() {
        if (_homeTarget != null) {
            transform.position = Vector3.MoveTowards(transform.position, _currentTarget, Time.deltaTime * 1000);
        }

        if(_pos == ConnectorPos.middle) {
            _currentTarget = _homeTarget.transform.position;

            _leftDirection.enabled = !_root.HasLeft();
            _rightDirection.enabled = !_root.HasRight();

            if (GetLeftInput() && !_root.HasLeft()) {
                _root.SetLeft(true, _device);
                _pos = ConnectorPos.left;
            }

            if (GetRightInput() && !_root.HasRight()) {
                _root.SetRight(true, _device);
                _pos = ConnectorPos.right;
            }
        }

        if (_pos == ConnectorPos.left) {
            _currentTarget = _root.GetLeftPosition();

            _leftDirection.enabled = false;
            _rightDirection.enabled = true;

            if (GetRightInput()) {
                _root.SetLeft(false, _device);
                _pos = ConnectorPos.middle;
            }
        }

        if (_pos == ConnectorPos.right) {
            _currentTarget = _root.GetRightPosition();

            _leftDirection.enabled = true;
            _rightDirection.enabled = false;

            if (GetLeftInput()) {
                _root.SetRight(false, _device);
                _pos = ConnectorPos.middle;
            }
        }
    }

    public bool GetLeftInput() {
        if(_gamepad != null) {
            return _gamepad.leftStick.left.wasPressedThisFrame || _gamepad.dpad.left.wasPressedThisFrame;
        }
        if (_keyboard != null) {
            return _keyboard.leftArrowKey.wasPressedThisFrame || _keyboard.aKey.wasPressedThisFrame;
        }

        return false;
    }

    public bool GetRightInput() {
        if (_gamepad != null) {
            return _gamepad.leftStick.right.wasPressedThisFrame || _gamepad.dpad.right.wasPressedThisFrame;
        }
        if (_keyboard != null) {
            return _keyboard.rightArrowKey.wasPressedThisFrame || _keyboard.dKey.wasPressedThisFrame;
        }

        return false;
    }

    public ConnectorPos GetConnectorPos() {
        return _pos;
    }

}

