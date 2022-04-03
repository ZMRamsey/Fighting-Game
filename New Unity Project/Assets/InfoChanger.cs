using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;
using UnityEngine.UI;

public class InfoChanger : MonoBehaviour
{
    [SerializeField] Sprite _iconWinController, _iconPSController, _iconKeyboard;
    [SerializeField] Image _image;

    void Update()
    {
        var gamepadButtonPressed = Gamepad.current != null && Gamepad.current.allControls.Any(x => x is UnityEngine.InputSystem.Controls.ButtonControl button && x.IsPressed() && !x.synthetic) 
            || Gamepad.current.leftStick.left.wasPressedThisFrame || Gamepad.current.leftStick.right.wasPressedThisFrame;
        var keyboardPressed = Keyboard.current.anyKey.wasPressedThisFrame;

        print(gamepadButtonPressed);
        print(keyboardPressed);

        if (gamepadButtonPressed) {
            if (Gamepad.current is DualShockGamepad) {
                _image.sprite = _iconPSController;
            }
            else {
                _image.sprite = _iconWinController;
            }
        }
        if (keyboardPressed) {
            _image.sprite = _iconKeyboard;
        }
    }
}
