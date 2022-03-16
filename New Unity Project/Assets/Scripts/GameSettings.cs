using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputDeviceType { keyboard, gamepad };

[CreateAssetMenu(fileName = "GameSettings", menuName = "Badminton/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    [SerializeField] FighterProfile _fighterOne;
    [SerializeField] FighterProfile _fighterTwo;
    [SerializeField] PlayerDevice _fighterOneDevice;
    [SerializeField] PlayerDevice _fighterTwoDevice;

    public FighterProfile GetFighterOneProfile() {
        return _fighterOne;
    }
    public FighterProfile GetFighterTwoProfile() {
        return _fighterTwo;
    }

    public FighterProfile GetFighterProfile(FighterFilter filter)
    {
        FighterProfile profile = _fighterOne;
        if (filter == FighterFilter.two)
        {
            profile = _fighterTwo;
        }
        return profile;
    }

    public void SetFighterOneProfile(FighterProfile profile) {
        _fighterOne = profile;
    }

    public void SetFighterTwoProfile(FighterProfile profile) {
        _fighterTwo = profile;
    }

    public InputState GetFighterOneState() {
        return _fighterOneDevice.GetInputState();
    }

    public InputState GetFighterTwoState() {
        return _fighterTwoDevice.GetInputState();
    }


    public PlayerDevice GetFighterOneDevice() {
        return _fighterOneDevice;
    }

    public PlayerDevice GetFighterTwoDevice() {
        return _fighterTwoDevice;
    }
}
[System.Serializable]
public class PlayerDevice
{
    [SerializeField] int _id;
    [SerializeField] InputDeviceType _device;
    [SerializeField] InputState _inputState;

    public PlayerDevice(InputDeviceType device, int id) {
        _id = id;
        _device = device;
    }

    public void SetInputState(InputState state) {
        _inputState = state;
    }

    public InputState GetInputState() {
        return _inputState;
    }

    public void SetDevice(PlayerDevice device) {
        _id = device.GetDeviceID();
        _device = device.GetDeviceType();
    }

    public int GetDeviceID() {
        return _id;
    }

    public InputDeviceType GetDeviceType() {
        return _device;
    }
}


//[System.Serializable]
//public class PlayerDevice
//{
//    [SerializeField] Gamepad _controller;
//    [SerializeField] Keyboard _keyboard;
//    [SerializeField] InputState _inputState;

//    public InputState GetInputState() {
//        return _inputState;
//    }

//    public void SetInputState(InputState state) {
//        _inputState = state;
//    }

//    public Gamepad GetGamepad() {
//        return _controller;
//    }

//    public Keyboard GetKeyboard() {
//        return _keyboard;
//    }

//    public void SetGamepad(Gamepad pad) {
//        _controller = pad;
//        _keyboard = null;
//    }

//    public void SetKeyboard(Keyboard board) {
//        _keyboard = board;
//        _controller = null;
//    }
//}
