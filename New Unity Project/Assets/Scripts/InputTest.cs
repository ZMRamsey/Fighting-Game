using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class InputTest : MonoBehaviour
{
    [SerializeField] GameSettings _settings;
    [SerializeField] TextMeshProUGUI _f1Text;
    [SerializeField] TextMeshProUGUI _f1Char;
    int _f1Index;
    int _f2Index;
    [SerializeField] TextMeshProUGUI _f2Text;
    [SerializeField] TextMeshProUGUI _f2Char;
    [SerializeField] FighterProfile[] _profiles;
    bool setPlayerOne;
    bool setPlayerTwo;
    // Start is called before the first frame update
    void Start()
    {
        _f1Char.color = Color.grey;
        _f2Char.color = Color.grey;

        _f1Char.text = _profiles[0].GetName();
        _f2Char.text = _profiles[0].GetName();

        _settings.SetFighterOneProfile(_profiles[0]);
        _settings.SetFighterTwoProfile(_profiles[0]);

        var allGamepads = Gamepad.all;

        foreach (Gamepad device in allGamepads) {
            var c = gameObject.AddComponent<DeviceConnector>();
            c.AddDevice(null, device, this);
        }

        if(Keyboard.current != null) {
            var c = gameObject.AddComponent<DeviceConnector>();
            c.AddDevice(Keyboard.current, null, this);
        }

    }

    void Update() {
        //DONT NEED TO REFERENCE THIS
        if (_settings.GetFighterOneDevice().GetKeyboard() != null) {
            if (_settings.GetFighterOneDevice().GetKeyboard().leftArrowKey.wasPressedThisFrame || _settings.GetFighterOneDevice().GetKeyboard().aKey.wasPressedThisFrame) {
                _f1Index++;
                if (_f1Index > _profiles.Length - 1) {
                    _f1Index = 0;
                }
                _f1Char.text = _profiles[_f1Index].GetName();
            }

            if (_settings.GetFighterOneDevice().GetKeyboard().rightArrowKey.wasPressedThisFrame || _settings.GetFighterOneDevice().GetKeyboard().dKey.wasPressedThisFrame) {
                _f1Index--;
                if (_f1Index < 0) {
                    _f1Index = _profiles.Length - 1;
                }
                _f1Char.text = _profiles[_f1Index].GetName();
                _settings.SetFighterOneProfile(_profiles[_f1Index]);
            }
        }

        if (_settings.GetFighterTwoDevice().GetKeyboard() != null) {
            if (_settings.GetFighterTwoDevice().GetKeyboard().leftArrowKey.wasPressedThisFrame || _settings.GetFighterTwoDevice().GetKeyboard().aKey.wasPressedThisFrame) {
                _f2Index++;
                if (_f2Index > _profiles.Length - 1) {
                    _f2Index = 0;
                }
                _f2Char.text = _profiles[_f2Index].GetName();
            }

            if (_settings.GetFighterTwoDevice().GetKeyboard().rightArrowKey.wasPressedThisFrame || _settings.GetFighterTwoDevice().GetKeyboard().dKey.wasPressedThisFrame) {
                _f2Index--;
                if (_f2Index < 0) {
                    _f2Index = _profiles.Length - 1;
                }
                _f2Char.text = _profiles[_f2Index].GetName();
                _settings.SetFighterTwoProfile(_profiles[_f2Index]);
            }
        }

        if (_settings.GetFighterOneDevice().GetGamepad() != null) {
            if (_settings.GetFighterOneDevice().GetGamepad().dpad.left.wasPressedThisFrame || _settings.GetFighterOneDevice().GetGamepad().leftStick.left.wasPressedThisFrame) {
                _f1Index++;
                if (_f1Index > _profiles.Length - 1) {
                    _f1Index = 0;
                }
                _f1Char.text = _profiles[_f1Index].GetName();
            }

            if (_settings.GetFighterOneDevice().GetGamepad().dpad.right.wasPressedThisFrame || _settings.GetFighterOneDevice().GetGamepad().leftStick.right.wasPressedThisFrame) {
                _f1Index--;
                if (_f1Index < 0) {
                    _f1Index = _profiles.Length - 1;
                }
                _f1Char.text = _profiles[_f1Index].GetName();
                _settings.SetFighterOneProfile(_profiles[_f1Index]);
            }
        }

        if (_settings.GetFighterTwoDevice().GetGamepad() != null) {
            if (_settings.GetFighterTwoDevice().GetGamepad().dpad.left.wasPressedThisFrame || _settings.GetFighterTwoDevice().GetGamepad().leftStick.left.wasPressedThisFrame) {
                _f2Index++;
                if (_f2Index > _profiles.Length - 1) {
                    _f2Index = 0;
                }
                _f1Char.text = _profiles[_f2Index].GetName();
            }

            if (_settings.GetFighterTwoDevice().GetGamepad().dpad.right.wasPressedThisFrame || _settings.GetFighterTwoDevice().GetGamepad().leftStick.right.wasPressedThisFrame) {
                _f2Index--;
                if (_f2Index < 0) {
                    _f2Index = _profiles.Length - 1;
                }
                _f1Char.text = _profiles[_f2Index].GetName();
                _settings.SetFighterTwoProfile(_profiles[_f2Index]);
            }
        }
    }

    public void SetPVAI() {
        _settings.GetFighterTwoDevice().SetInputState(InputState.ai);
        Application.LoadLevel("Base");
    }

    public void SetPlayer(Keyboard keyboard) {
        if (!setPlayerOne) {
            _f1Char.color = Color.white;
            _f1Text.text = "KEYBOARD";
            _settings.GetFighterOneDevice().SetKeyboard(keyboard);
            _settings.GetFighterOneDevice().SetInputState(InputState.player);
            setPlayerOne = true;
            return;
        }

        if (!setPlayerTwo) {
            _f2Char.color = Color.white;
            _f2Text.text = "KEYBOARD";
            _settings.GetFighterTwoDevice().SetKeyboard(keyboard);
            _settings.GetFighterTwoDevice().SetInputState(InputState.player);
            setPlayerTwo = true;
            Application.LoadLevel("Base");
            return;
        }
    }

    public void SetPlayer(Gamepad gamepad) {
        if (!setPlayerOne) {
            _f1Char.color = Color.white;
            _f1Text.text = "GAMEPAD";
            _settings.GetFighterOneDevice().SetGamepad(gamepad);
            print(_settings.GetFighterOneDevice().GetGamepad().name);
            _settings.GetFighterOneDevice().SetInputState(InputState.player);
            setPlayerOne = true;
            return;
        }

        if (!setPlayerTwo) {
            _f2Char.color = Color.white;
            _f2Text.text = "GAMEPAD";
            _settings.GetFighterTwoDevice().SetGamepad(gamepad);
            _settings.GetFighterTwoDevice().SetInputState(InputState.player);
            setPlayerTwo = true;
            Application.LoadLevel("Base");
            return;
        }
    }
}
