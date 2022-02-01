using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputState { none, player, ai};

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

            if (Keyboard.current.wKey.wasPressedThisFrame) {
                _jumpInput = true;
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
    }

    public InputState GetState() {
        return _state;
    }

    public void SetInputState(InputState state) {
        _state = state;
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
