using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShuttleCockDebug : MonoBehaviour
{
    ShuttleCock _cock;
    [SerializeField] TextMeshProUGUI _text;

    private void Start() {
        _cock = GameManager.Get().GetShuttle();
    }

    void Update() {
        _text.text = $"Ball Velocity\n{_cock.GetVelocity().magnitude}";
    }
}
