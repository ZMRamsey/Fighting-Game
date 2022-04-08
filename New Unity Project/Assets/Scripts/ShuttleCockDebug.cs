using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShuttleCockDebug : MonoBehaviour
{
    ShuttleCock _shuttle;
    [SerializeField] TextMeshProUGUI _text;

    private void Start() {
        _shuttle = GameManager.Get().GetShuttle();
    }

    void Update() {
        _text.text = $"MAGNITUDE: <color=red>{(int)_shuttle.GetVelocity().magnitude}</color>\n" +
            $"POWER: <color=red>{_shuttle.GetRawSpeed()}</color>\n" +
            $"OVERPOWER: <color=red>{(int)_shuttle.GetOverSpeed()}</color>";
    }
}
