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
    [SerializeField] TextMeshProUGUI _f2Text;
    bool setPlayerOne;
    bool setPlayerTwo;
    // Start is called before the first frame update
    void Start()
    {
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

    public void SetPlayer(Keyboard keyboard) {
        if (!setPlayerOne) {
            _f1Text.text = "KEYBOARD";
            _settings.GetFighterOneDevice().SetKeyboard(keyboard);
            setPlayerOne = true;
            return;
        }

        if (!setPlayerTwo) {
            _f2Text.text = "KEYBOARD";
            _settings.GetFighterTwoDevice().SetKeyboard(keyboard);
            setPlayerTwo = true;
            Application.LoadLevel(1);
            return;
        }
    }

    public void SetPlayer(Gamepad gamepad) {
        if (!setPlayerOne) {
            _f1Text.text = "GAMEPAD";
            _settings.GetFighterOneDevice().SetGamepad(gamepad);
            print(_settings.GetFighterOneDevice().GetGamepad().name);
            setPlayerOne = true;
            return;
        }

        if (!setPlayerTwo) {
            _f2Text.text = "GAMEPAD";
            _settings.GetFighterTwoDevice().SetGamepad(gamepad);
            setPlayerTwo = true;
            Application.LoadLevel(1);
            return;
        }
    }
}
