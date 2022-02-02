using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FighterControllerDebug : MonoBehaviour
{
    [SerializeField] FighterFilter _filter;
    [SerializeField] float _smoother = 0.05f;
    FighterController _controller;
    [SerializeField] TextMeshProUGUI _UIDebug;
    InputHandler _handler;
    [SerializeField] RectTransform _movementDebugPoint;
    [SerializeField] Sprite[] _driveBtnState;
    [SerializeField] Sprite[] _dropBtnState;
    [SerializeField] Image _driveBtn;
    [SerializeField] Image _dropBtn;

    float x = 0;
    float y = 0;
    float _driveCooldown;

    float inputY;

    private void Start() {
        if (_filter == FighterFilter.one) {
            _controller = GameManager.Get().GetFighterOne();
        }
        else {
            _controller = GameManager.Get().GetFighterTwo();
        }

        _handler = _controller.GetComponent<InputHandler>();
    }

    void Update() {
        string debugText = $"Fighter Stance = <color=red>{_controller.GetFighterStance()}</color> \n" +
            $"Fighter Action = <color=red>{_controller.GetFighterAction()}</color> \n" +
            $"Fighter Meter = <color=red>{_controller.GetMeter() * 100}</color>% \n" +
            $"Fighter Can Attack = <color=blue>{_controller.CanAttack()}</color> \n" +
             $"Fighter Speed = <color=red>{(int)_controller.GetComponent<Rigidbody>().velocity.magnitude}</color> \n";

        _UIDebug.text = debugText;

        if (_handler.GetJumpHeld()) {
            inputY = 1;
        }
        else {
            inputY = 0;
        }

        x = Mathf.Lerp(x, _handler.GetInputX() * 65f, _smoother);
        y = Mathf.Lerp(y, inputY * 65f, _smoother);

        _movementDebugPoint.anchoredPosition = new Vector3(-x, y, 0);
    }

}
