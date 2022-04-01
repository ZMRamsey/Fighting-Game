using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

/// <summary>
/// Global Input Manger to sync menu controls so everythings consistent.
/// You can get any device by using GetUpInput(); 
/// You can get player ones device by GetUpInput(FighterFilter.one);
/// You can get player twos device by GetUpInput(FighterFilter.two);
/// If it can't get access to player ones or twos specific devices it'll default to any device. 
/// </summary>

public class GlobalInputManager : MonoBehaviour
{
    static GlobalInputManager _manager;
    Gamepad _controller;
    Keyboard _keyboard;

    public static GlobalInputManager Get() {
        return _manager;
    }

    void Awake() {
        _manager = this;
    }

    public bool GetUpInput() {
        return GetUpInput(FighterFilter.none);
    }

    public bool GetDownInput() {
        return GetDownInput(FighterFilter.none);
    }

    public bool GetLeftInput() {
        return GetLeftInput(FighterFilter.none);
    }

    public bool GetRightInput() {
        return GetRightInput(FighterFilter.none);
    }

    public bool GetSubmitInput() {
        return GetSubmitInput(FighterFilter.none);
    }

    public bool GetBackInput() {
        return GetBackInput(FighterFilter.none);
    }

    public bool GetPauseHeldInput() {
        return GetPauseHeldInput(FighterFilter.none);
    }

    public bool GetPauseInput() {
        return GetPauseInput(FighterFilter.none);
    }

    public bool GetAnyButton() {
        RefreshDevices(FighterFilter.none);
        var gamepadButtonPressed = HasController() && _controller.allControls.Any(x => x is ButtonControl button && x.IsPressed() && !x.synthetic);
        return gamepadButtonPressed || (HasKeyboard() && _keyboard.anyKey.wasPressedThisFrame);
    }

    public bool GetUpInput(FighterFilter filter) {
        RefreshDevices(filter);
        return (HasController() && (_controller.dpad.up.wasPressedThisFrame || _controller.leftStick.up.wasPressedThisFrame))
            || (HasKeyboard() && (_keyboard.wKey.wasPressedThisFrame || _keyboard.upArrowKey.wasPressedThisFrame));
    }

    public bool GetDownInput(FighterFilter filter) {
        RefreshDevices(filter);
        return (HasController() && (_controller.dpad.down.wasPressedThisFrame || _controller.leftStick.down.wasPressedThisFrame))
            || (HasKeyboard() && (_keyboard.sKey.wasPressedThisFrame || _keyboard.downArrowKey.wasPressedThisFrame));
    }

    public bool GetLeftInput(FighterFilter filter) {
        RefreshDevices(filter);
        return (HasController() && (_controller.dpad.left.wasPressedThisFrame || _controller.leftStick.left.wasPressedThisFrame))
            || (HasKeyboard() && (_keyboard.aKey.wasPressedThisFrame || _keyboard.leftArrowKey.wasPressedThisFrame));
    }

    public bool GetRightInput(FighterFilter filter) {
        RefreshDevices(filter);
        return (HasController() && (_controller.dpad.right.wasPressedThisFrame || _controller.leftStick.right.wasPressedThisFrame))
            || (HasKeyboard() && (_keyboard.dKey.wasPressedThisFrame || _keyboard.rightArrowKey.wasPressedThisFrame));
    }

    public bool GetSubmitInput(FighterFilter filter) {
        RefreshDevices(filter);
        return (HasController() && (_controller.aButton.wasPressedThisFrame)
            || (HasKeyboard() && (_keyboard.spaceKey.wasPressedThisFrame || _keyboard.enterKey.wasPressedThisFrame)));
    }

    public bool GetBackInput(FighterFilter filter) {
        RefreshDevices(filter);
        return (HasController() && (_controller.bButton.wasPressedThisFrame)
            || (HasKeyboard() && (_keyboard.escapeKey.wasPressedThisFrame)));
    }
    public bool GetPauseHeldInput(FighterFilter filter) {
        RefreshDevices(filter);
        return (HasController() && (_controller.startButton.isPressed)
            || (HasKeyboard() && (_keyboard.escapeKey.isPressed)));
    }

    public bool GetPauseInput(FighterFilter filter) {
        RefreshDevices(filter);
        return (HasController() && (_controller.startButton.wasPressedThisFrame)
            || (HasKeyboard() && (_keyboard.escapeKey.wasPressedThisFrame)));
    }

    public void RefreshDevices(FighterFilter filter) {
        _controller = null;
        _keyboard = null;
        PlayerDevice device = null;

        if(filter == FighterFilter.one) {
            device = GameLogic.Get().GetSettings().GetFighterOneDevice();
        }
        else if (filter == FighterFilter.two) {
            device = GameLogic.Get().GetSettings().GetFighterTwoDevice();
        }
        else {
            _controller = Gamepad.current;
            _keyboard = Keyboard.current;
        }

        if (device != null) {
            if (device.GetDeviceType() == InputDeviceType.keyboard) {
                _keyboard = Keyboard.current;
            }

            if (device.GetDeviceType() == InputDeviceType.gamepad) {
                print("PAD");
                foreach (Gamepad pad in Gamepad.all) {
                    if (pad.deviceId == device.GetDeviceID()) {
                        _controller = pad;
                        print("PAD");
                    }
                }
            }
        }
    }

    public bool ControllerOrKeyboardInUse() {
        return (HasController() && _controller.lastUpdateTime < 5.0f) || (HasKeyboard() && _keyboard.lastUpdateTime < 5.0f);
    }

    bool HasController() {
        return _controller != null;
    }

    bool HasKeyboard() {
        return _keyboard != null;
    }
}
