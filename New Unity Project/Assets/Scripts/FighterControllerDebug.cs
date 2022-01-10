using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FighterControllerDebug : MonoBehaviour
{
    [SerializeField] FighterController _controller;
    [SerializeField] TextMeshProUGUI _UIDebug;

    void Update()
    {
        string debugText = $"Fighter Stance = <color=red>{_controller.GetFighterStance()}</color> \n" +
            $"Fighter Action = <color=red>{_controller.GetFighterAction()}</color> \n" +
            $"Current Input String =";

        _UIDebug.text = debugText;
    }

}
