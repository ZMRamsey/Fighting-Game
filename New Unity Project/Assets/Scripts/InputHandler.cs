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
    public bool _jumpExtraInput;
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

    public bool _crouchInput;
    public bool _crouchFrame;

    Gamepad _gamepad;
    Keyboard _keyboard;
    private bool _pauseHeld;

    public void SetDevice(PlayerDevice device) {
        //_playerDevice = device;
        if (device.GetDeviceType() == InputDeviceType.keyboard) {
            _keyboard = Keyboard.current;
        }

        if (device.GetDeviceType() == InputDeviceType.gamepad) {
           foreach(Gamepad pad in Gamepad.all) {
                if(pad.deviceId == device.GetDeviceID()) {
                    _gamepad = pad;
                }
            }
        }
    }

    void Update() {
        if (Time.timeScale <= 0 || !GameManager.Get().IsGameActive()) {
            _inputX = 0.0f;
            _jumpInput = false;
            _jumpHeld = false;
            _crouchInput = false;
            _smashInput = false;
            _smashInput = false;
            _driveInput = false;
            _dropInput = false;
            return;
        }

        if (_keyboard != null) {

            _pauseHeld = _keyboard.escapeKey.IsPressed();

            _inputX = 0.0f;

            if (_keyboard.aKey.IsPressed()) {
                _inputX = 1;
            }
            if (_keyboard.dKey.IsPressed()) {
                _inputX = -1;
            }

            _jumpHeld = _keyboard.wKey.isPressed;


            if (_keyboard.wKey.isPressed) {
                _jumpInput = true;
                _crouchInput = false;
            }
            else {
                _jumpInput = false;
            }

            if (_keyboard.wKey.wasPressedThisFrame) {
                _jumpExtraInput = true;
            }

            _crouchInput = _keyboard.sKey.isPressed;

            if (_keyboard.sKey.wasPressedThisFrame)
            {
                _crouchFrame = true;
            }

            if (_keyboard.shiftKey.wasPressedThisFrame) {
                _dashInput = true;
            }

            if (_keyboard.rightArrowKey.wasPressedThisFrame || _keyboard.lKey.wasPressedThisFrame) {
                _driveInput = true;
            }

            if (_keyboard.upArrowKey.wasPressedThisFrame || _keyboard.iKey.wasPressedThisFrame) {
                _dropInput = true;
            }

            if (_keyboard.leftArrowKey.wasPressedThisFrame || _keyboard.jKey.wasPressedThisFrame) {
                _smashInput = true;
            }

            if (_keyboard.spaceKey.wasPressedThisFrame) {
                _specialInput = true;
            }

            if (_keyboard.rightArrowKey.isPressed || _keyboard.leftArrowKey.isPressed || _keyboard.upArrowKey.isPressed || _keyboard.lKey.isPressed
                || _keyboard.iKey.isPressed || _keyboard.jKey.isPressed)
            {
                _chargeInput = true;
            }
            else
            {
                _chargeInput = false;
            }

            _chipHeld = _keyboard.downArrowKey.isPressed || _keyboard.kKey.isPressed;

            if (_keyboard.downArrowKey.wasPressedThisFrame || _keyboard.kKey.wasPressedThisFrame)
            {
                _chipInput = true;
            }
        }

        if (_gamepad != null)
        {
            _pauseHeld = _gamepad.startButton.IsPressed();

            _inputX = 0.0f;
            if (_gamepad.leftStick.left.ReadValue() > 0.5 || _gamepad.dpad.left.isPressed)
            {
                _inputX = 1;
            }
            if (_gamepad.leftStick.right.ReadValue() > 0.5 || _gamepad.dpad.right.isPressed)
            {
                _inputX = -1;
            }

            _jumpHeld = _gamepad.leftStick.up.ReadValue() > 0.5 || _gamepad.dpad.up.isPressed;

            if (_gamepad.leftStick.up.ReadValue() > 0.5 || _gamepad.dpad.up.isPressed)
            {
                _jumpInput = true;
                _crouchInput = false;
            }
            else
            {
                _jumpInput = false;
            }

            if (_gamepad.leftStick.up.wasPressedThisFrame || _gamepad.dpad.up.wasPressedThisFrame) {
                _jumpExtraInput = true;
            }

            _crouchInput = _gamepad.leftStick.down.isPressed || _gamepad.dpad.down.isPressed;

            if (_gamepad.leftStick.down.wasPressedThisFrame || _gamepad.dpad.down.wasPressedThisFrame)
            {
                _crouchFrame = true;
            }

            if (_gamepad.leftTrigger.wasPressedThisFrame) {
                _dashInput = true;
            }

            if (_gamepad.buttonEast.wasPressedThisFrame)
            {
                _driveInput = true;
            }

            if (_gamepad.buttonNorth.wasPressedThisFrame)
            {
                _dropInput = true;
            }

            if (_gamepad.buttonWest.wasPressedThisFrame)
            {
                _smashInput = true;
            }

            if (_gamepad.buttonSouth.wasPressedThisFrame)
            {
                _chipInput = true;
            }

            if (_gamepad.rightTrigger.wasPressedThisFrame)
            {
                _specialInput = true;
            }

            //if (_playerDevice.GetGamepad().buttonEast.isPressed || _playerDevice.GetGamepad().buttonNorth.isPressed || _playerDevice.GetGamepad().buttonWest.isPressed || _playerDevice.GetGamepad().buttonSouth.isPressed)
            if (_gamepad.buttonEast.isPressed || _gamepad.buttonNorth.isPressed || _gamepad.buttonWest.isPressed)
            {
                _chargeInput = true;
            }
            else
            {
                _chargeInput = false;
            }

            if (_gamepad.buttonSouth.isPressed)
            {
                _chipHeld = true;
            }
            else
            {
                _chipHeld = false;
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

    public bool GetDoubleJump(bool can) {
        if (!can) {
            _jumpExtraInput = false;
            return false;
        }

        var temp = _jumpExtraInput;
        _jumpExtraInput = false;
        return temp;
    }

    public bool GetCrouch()
    {
        return _crouchInput;
    }

    public bool GetCrouchFrame()
    {
        var temp = _crouchFrame;
        _crouchFrame = false;
        return temp;
    }

    public bool GetSmash() {
        var temp = _smashInput;
        _smashInput = false;
        return temp;
    }

    public bool GetLift() {
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

    public bool GetSuper() {
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

    public bool GetPause() {
        return _pauseHeld;
    }
}
