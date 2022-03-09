using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputState { none, player, ai};

public class InputHandler : MonoBehaviour
{
    [SerializeField] InputState _state;
    PlayerDevice _playerDevice;
    public float _inputX;
    public float _grabHoldTime;

    public bool _jumpInput;
    public bool _jumpHeld;
    public bool _smashInput;
    public bool _dropInput;
    public bool _driveInput;
    public bool _chipInput;
    public bool _specialInput;
    public bool _dashInput;
    public bool _chargeInput;
    public bool _grabInput;
    public bool _chipHeld;

    public void SetDevice(PlayerDevice device) {
        _playerDevice = device;
    }

    void Update() {
        if (_playerDevice != null && _playerDevice.GetKeyboard() != null) {
            
            _inputX = 0.0f;
            if (_playerDevice.GetKeyboard().aKey.IsPressed()) {
                _inputX = 1;
            }
            if (_playerDevice.GetKeyboard().dKey.IsPressed()) {
                _inputX = -1;
            }

            _jumpHeld = _playerDevice.GetKeyboard().wKey.isPressed;


            if (_playerDevice.GetKeyboard().wKey.isPressed) {
                _jumpInput = true;
            }
            else {
                _jumpInput = false;
            }

            if (_playerDevice.GetKeyboard().shiftKey.wasPressedThisFrame) {
                _dashInput = true;
            }

            if (_playerDevice.GetKeyboard().rightArrowKey.wasPressedThisFrame || _playerDevice.GetKeyboard().lKey.wasPressedThisFrame) {
                _driveInput = true;
            }

            if (_playerDevice.GetKeyboard().upArrowKey.wasPressedThisFrame || _playerDevice.GetKeyboard().iKey.wasPressedThisFrame) {
                _dropInput = true;
            }

            if (_playerDevice.GetKeyboard().leftArrowKey.wasPressedThisFrame || _playerDevice.GetKeyboard().jKey.wasPressedThisFrame) {
                _smashInput = true;
            }

            if (_playerDevice.GetKeyboard().spaceKey.wasPressedThisFrame) {
                _specialInput = true;
            }

            if (_playerDevice.GetKeyboard().rightArrowKey.isPressed || _playerDevice.GetKeyboard().leftArrowKey.isPressed || _playerDevice.GetKeyboard().upArrowKey.isPressed || _playerDevice.GetKeyboard().lKey.isPressed
                || _playerDevice.GetKeyboard().iKey.isPressed || _playerDevice.GetKeyboard().jKey.isPressed)
            {
                _chargeInput = true;
            }
            else
            {
                _chargeInput = false;
            }

            _chipHeld = _playerDevice.GetKeyboard().downArrowKey.isPressed || _playerDevice.GetKeyboard().kKey.isPressed;

            if (_playerDevice.GetKeyboard().downArrowKey.isPressed || _playerDevice.GetKeyboard().kKey.isPressed)
            {
                _chipInput = true;
            }
            else
            {
                _chipInput = false;
            }
        }

        if (_playerDevice != null && _playerDevice.GetGamepad() != null)
        {
            _inputX = 0.0f;
            if (_playerDevice.GetGamepad().leftStick.left.ReadValue() > 0.5 || _playerDevice.GetGamepad().dpad.left.isPressed)
            {
                _inputX = 1;
            }
            if (_playerDevice.GetGamepad().leftStick.right.ReadValue() > 0.5 || _playerDevice.GetGamepad().dpad.right.isPressed)
            {
                _inputX = -1;
            }

            _jumpHeld = _playerDevice.GetGamepad().leftStick.up.ReadValue() > 0.5 || _playerDevice.GetGamepad().dpad.up.isPressed;

            if (_playerDevice.GetGamepad().leftStick.up.ReadValue() > 0.5 || _playerDevice.GetGamepad().dpad.up.isPressed)
            {
                _jumpInput = true;
            }
            else
            {
                _jumpInput = false;
            }

            if (_playerDevice.GetGamepad().leftTrigger.wasPressedThisFrame) {
                _dashInput = true;
            }

            if (_playerDevice.GetGamepad().buttonEast.wasPressedThisFrame)
            {
                _driveInput = true;
            }

            if (_playerDevice.GetGamepad().buttonNorth.wasPressedThisFrame)
            {
                _dropInput = true;
            }

            if (_playerDevice.GetGamepad().buttonWest.wasPressedThisFrame)
            {
                _smashInput = true;
            }

            if (_playerDevice.GetGamepad().buttonSouth.wasPressedThisFrame)
            {
                _chipInput = true;
            }

            if (_playerDevice.GetGamepad().rightTrigger.wasPressedThisFrame)
            {
                _specialInput = true;
            }

            //if (_playerDevice.GetGamepad().buttonEast.isPressed || _playerDevice.GetGamepad().buttonNorth.isPressed || _playerDevice.GetGamepad().buttonWest.isPressed || _playerDevice.GetGamepad().buttonSouth.isPressed)
            if (_playerDevice.GetGamepad().buttonEast.isPressed || _playerDevice.GetGamepad().buttonNorth.isPressed || _playerDevice.GetGamepad().buttonWest.isPressed)
            {
                _chargeInput = true;
            }
            else
            {
                _chargeInput = false;
            }

            if (_playerDevice.GetGamepad().buttonSouth.isPressed)
            {
                _grabInput = true;
            }
            else
            {
                _grabInput = false;
            }
        }
    }

    public InputState GetState() {
        return _state;
    }

    public void SetInputState(InputState state) {
        _state = state;
    }

    public Vector3 GetInput() {
        float y = 0;
        if (_jumpHeld) {
            y = 1;
        }

        return new Vector3(GetInputX(), y, 0);
    }

    public float GetInputX() {
        return _inputX;
    }

    public bool GetJumpHeld() {
        return _jumpHeld;
    }

    public bool GetJump(bool can) {
        if (!can) {
            return false;
        }

        var temp = _jumpInput;
        _jumpInput = false;
        return temp;
    }

    public bool GetSmash() {
        var temp = _smashInput;
        _smashInput = false;
        return temp;
    }

    public bool GetDrop() {
        var temp = _dropInput;
        _dropInput = false;
        return temp;
    }

    public bool GetDrive() {
        var temp = _driveInput;
        _driveInput = false;
        return temp;
    }

    public bool GetChip() {
        var temp = _chipInput;
        _chipInput = false;
        return temp;
    }

    public bool GetSpecial() {
        var temp = _specialInput;
        _specialInput = false;
        return temp;
    }

    public bool GetDash() {
        var temp = _dashInput;
        _dashInput = false;
        return temp;
    }

    public bool GetCharge()
    {
        return _chargeInput;
    }

    public bool GetGrab()
    {
        return _chipHeld;
    }
}
