using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using TMPro;

public enum GameType { pvp, pva, watch, tutorial, training, arcade};
public class InputSelectionSystem : MonoBehaviour
{
    [SerializeField] GameObject _connectorPrefab;

    List<GameObject> _currentControllerPrefabs = new List<GameObject>();
    List<GameObject> _currentHomeTargets = new List<GameObject>();

    [SerializeField] Transform _leftControllerTarget, _rightControllerTarget;
    [SerializeField] Transform _homeTargetHolder;
    [SerializeField] Transform _controllerTab;

    [SerializeField] TextMeshProUGUI _informative;
    [SerializeField] Transform _panel;
    GameType _type;

    bool _hasLeft;
    bool _hasRight;

    // Start is called before the first frame update
    void Start()
    {
        OnPageOpened();
    }

    public bool CanControl() {
        return _panel.gameObject.activeSelf;
    }

    void Update()
    {
        if (!CanControl()) {
            return;
        }

        if (_type == GameType.pvp) {
            if (!HasLeft() || !HasRight()) {
                _informative.text = "WAITING...";
            }
            else {
                _informative.text = "PRESS TO CONTINUE";
                if (GlobalInputManager.Get().GetSubmitInput()) {
                    GameLogic.Get().GetSettings().GetFighterOneDevice().SetInputState(InputState.player);
                    GameLogic.Get().GetSettings().GetFighterTwoDevice().SetInputState(InputState.player);

                    CharacterSelectSystem.Get().SetPage(1);
                }
            }
        }
        else if (_type == GameType.watch) {
            GameLogic.Get().GetSettings().GetFighterOneDevice().SetInputState(InputState.ai);
            GameLogic.Get().GetSettings().GetFighterTwoDevice().SetInputState(InputState.ai);

            CharacterSelectSystem.Get().SetPage(1);
        }
        else if (_type == GameType.tutorial) {
            GameLogic.Get().GetSettings().GetFighterOneDevice().SetInputState(InputState.player);
            GameLogic.Get().GetSettings().GetFighterTwoDevice().SetInputState(InputState.ai);

            CharacterSelectSystem.Get().SetPage(1);
        }
        else if (_type == GameType.arcade)
        {
            GameLogic.Get().GetSettings().GetFighterOneDevice().SetInputState(InputState.player);
            GameLogic.Get().GetSettings().GetFighterTwoDevice().SetInputState(InputState.ai);

            CharacterSelectSystem.Get().SetPage(1);
        }
        else {
            if (!HasLeft() && !HasRight()) {
                _informative.text = "WAITING...";
            }
            else {
                _informative.text = "PRESS TO CONTINUE";
                if (GlobalInputManager.Get().GetSubmitInput()) {
                    if (HasLeftRaw()) {
                        GameLogic.Get().GetSettings().GetFighterOneDevice().SetInputState(InputState.player);
                        GameLogic.Get().GetSettings().GetFighterTwoDevice().SetInputState(InputState.ai);
                    }

                    if (HasRightRaw()) {
                        GameLogic.Get().GetSettings().GetFighterOneDevice().SetInputState(InputState.ai);
                        GameLogic.Get().GetSettings().GetFighterTwoDevice().SetInputState(InputState.player);
                    }

                    CharacterSelectSystem.Get().SetPage(1);
                }
            }
        }
    }

    public void OnPageOpened() {
        _hasLeft = false;
        _hasRight = false;

        _type = CharacterSelectSystem.Get().GetGameType();

        foreach (GameObject prefab in _currentControllerPrefabs) {
            Destroy(prefab);
        }

        foreach (GameObject target in _currentHomeTargets) {
            Destroy(target);
        }

        _currentControllerPrefabs.Clear();
        _currentHomeTargets.Clear();

        var allGamepads = Gamepad.all;

        foreach (Gamepad device in allGamepads) {
            var target = new GameObject();
            target.AddComponent<RectTransform>();
            target.transform.SetParent(_homeTargetHolder);
            target.transform.localScale = Vector3.one;
            _currentHomeTargets.Add(target);

            var prefab = Instantiate(_connectorPrefab, target.transform.position, Quaternion.identity);
            prefab.transform.SetParent(_controllerTab);
            prefab.transform.localScale = Vector3.one;

            prefab.GetComponent<DeviceConnector>().AddDevice(null, device, target.transform, this);
            _currentControllerPrefabs.Add(prefab);
        }

        if (Keyboard.current != null) {
            var target = new GameObject();
            target.AddComponent<RectTransform>();
            target.transform.SetParent(_homeTargetHolder);
            target.transform.localScale = Vector3.one;
            _currentHomeTargets.Add(target);

            var prefab = Instantiate(_connectorPrefab, target.transform.position, Quaternion.identity);
            prefab.transform.SetParent(_controllerTab);
            prefab.transform.localScale = Vector3.one;

            prefab.GetComponent<DeviceConnector>().AddDevice(Keyboard.current, null, target.transform, this);
            _currentControllerPrefabs.Add(prefab);
        }
    }

    public Vector3 GetLeftPosition() {
        return _leftControllerTarget.position;
    }

    public Vector3 GetRightPosition() {
        return _rightControllerTarget.position;
    }

    public void SetLeft(bool value, PlayerDevice device) {
        if (value) {
            GameLogic.Get().GetSettings().GetFighterOneDevice().SetDevice(device);
        }
        _hasLeft = value;
    }

    public void SetRight(bool value, PlayerDevice device) {
        if (value) {
            GameLogic.Get().GetSettings().GetFighterTwoDevice().SetDevice(device);
        }
        _hasRight = value;
    }

    public bool HasLeft() {
        if (_type == GameType.pva) {
            return _hasRight || _hasLeft;
        }
        return _hasLeft;
    }

    public bool HasRight() {
        if (_type == GameType.pva) {
            return _hasRight || _hasLeft;
        }
        return _hasRight;
    }

    public bool HasLeftRaw() {
        return _hasLeft;
    }

    public bool HasRightRaw() {
        return _hasRight;
    }


}
