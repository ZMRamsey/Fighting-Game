using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] Gamepad _controller;
    [SerializeField] Keyboard _keyboard;
    [SerializeField] InputState _inputState;

    public InputState GetInputState() {
        return _inputState;
    }

    public void SetInputState(InputState state) {
        _inputState = state;
    }

    public Gamepad GetGamepad() {
        return _controller;
    }

    public Keyboard GetKeyboard() {
        return _keyboard;
    }

    public void SetGamepad(Gamepad pad) {
        _controller = pad;
        _keyboard = null;
    }

    public void SetKeyboard(Keyboard board) {
        _keyboard = board;
        _controller = null;
    }
}
