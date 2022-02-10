using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputState { none, player, ai, controller};

public class InputHandler : MonoBehaviour
{
    [SerializeField] InputState _state;
    public float _inputX;

    public bool _jumpInput;
    public bool _jumpHeld;
    public bool _smashInput;
    public bool _dropInput;
    public bool _driveInput;
    public bool _chipInput;
    public bool _specialInput;

    void Update() {
        if (_state == InputState.player) {
            _inputX = 0.0f;
            if (Keyboard.current.aKey.IsPressed()) {
                _inputX = 1;
            }
            if (Keyboard.current.dKey.IsPressed()) {
                _inputX = -1;
            }

            _jumpHeld = Keyboard.current.wKey.isPressed;

            if (Keyboard.current.wKey.isPressed) {
                _jumpInput = true;
            }
            else {
                _jumpInput = false;
            }

            if (Keyboard.current.rightArrowKey.wasPressedThisFrame) {
                _driveInput = true;
            }

            if (Keyboard.current.upArrowKey.wasPressedThisFrame) {
                _dropInput = true;
            }

            if (Keyboard.current.leftArrowKey.wasPressedThisFrame) {
                _smashInput = true;
            }

            if (Keyboard.current.downArrowKey.wasPressedThisFrame) {
                _chipInput = true;
            }

            if (Keyboard.current.spaceKey.wasPressedThisFrame) {
                _specialInput = true;
            }
        }

        if (_state == InputState.controller)
        {
            _inputX = 0.0f;
            if (Gamepad.current.leftStick.left.ReadValue() > 0 || Gamepad.current.dpad.left.isPressed)
            {
                _inputX = 1;
            }
            if (Gamepad.current.leftStick.right.ReadValue() > 0 || Gamepad.current.dpad.right.isPressed)
            {
                _inputX = -1;
            }

            _jumpHeld = Gamepad.current.leftStick.up.ReadValue() > 0 || Gamepad.current.dpad.up.isPressed;

            if (Gamepad.current.leftStick.up.ReadValue() > 0 || Gamepad.current.dpad.up.isPressed)
            {
                _jumpInput = true;
            }
            else
            {
                _jumpInput = false;
            }

            if (Gamepad.current.buttonEast.wasPressedThisFrame)
            {
                _driveInput = true;
            }

            if (Gamepad.current.buttonNorth.wasPressedThisFrame)
            {
                _dropInput = true;
            }

            if (Gamepad.current.buttonWest.wasPressedThisFrame)
            {
                _smashInput = true;
            }

            if (Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                _chipInput = true;
            }

            if (Gamepad.current.rightTrigger.wasPressedThisFrame)
            {
                _specialInput = true;
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
}
