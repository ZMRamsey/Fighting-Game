using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FighterControllerDebug : MonoBehaviour
{
    [SerializeField] float _smoother = 0.05f;
    [SerializeField] FighterController _controller;
    [SerializeField] TextMeshProUGUI _UIDebug;
    [SerializeField] InputHandler _handler;
    [SerializeField] RectTransform _movementDebugPoint;

    float x = 0;
    float y = 0;

    float inputY;

    void Update()
    {
        string debugText = $"Fighter Stance = <color=red>{_controller.GetFighterStance()}</color> \n" +
            $"Fighter Action = <color=red>{_controller.GetFighterAction()}</color> \n" +
            $"Current Input String =";

        _UIDebug.text = debugText;

        if (_handler.GetJumpHeld()) {
            inputY = 1;
        }
        else {
            inputY = 0;
        }

        x = Mathf.Lerp(x, _handler.GetInputX() * 65f, _smoother);
        y = Mathf.Lerp(y, inputY * 65f, _smoother);

        _movementDebugPoint.anchoredPosition = new Vector3(-x , y, 0);
    }

}
