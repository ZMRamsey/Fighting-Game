using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputState { none, player, ai};

public class InputHandler : MonoBehaviour
{
    [SerializeField] InputState _state;
    float _inputX;

    bool _jumpInput;
    bool _smashInput;
    bool _dropInput;

    void Update() {
        if (_state == InputState.player) {
            _inputX = 0.0f;
            if (Keyboard.current.aKey.IsPressed()) {
                _inputX = 1;
            }
            if (Keyboard.current.dKey.IsPressed()) {
                _inputX = -1;
            }

            if (Keyboard.current.wKey.wasPressedThisFrame) {
                _jumpInput = true;
            }

            if (Keyboard.current.spaceKey.wasPressedThisFrame) {
                _smashInput = true;
            }
        }
    }

    public float GetInputX() {
        return _inputX;
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
}
