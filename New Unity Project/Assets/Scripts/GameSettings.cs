using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Badminton/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
    [SerializeField] FighterProfile _fighterOne;
    [SerializeField] FighterProfile _fighterTwo;
    [SerializeField] InputState _fighterOneState;
    [SerializeField] InputState _fighterTwoState;

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

    public void SetPVP() {
        _fighterOneState = InputState.player;
        _fighterTwoState = InputState.player;
    }

    public void SetPVA() {
        _fighterOneState = InputState.player;
        _fighterTwoState = InputState.ai;
    }

    public void SetPVD() {
        _fighterOneState = InputState.player;
        _fighterTwoState = InputState.none;
    }

    public InputState GetFighterOneState() {
        return _fighterOneState;
    }

    public InputState GetFighterTwoState() {
        return _fighterTwoState;
    }
}
