using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    [SerializeField] Image _RGBMask;

    public void UpdatePallete(Material mat) {
        if (_RGBMask) {
            _RGBMask.material = mat;
        }
    }

    public void SetState(bool value) {
        if (_RGBMask) {
            _RGBMask.enabled = value;
        }
    }
}
