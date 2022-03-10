using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class Audio_Value_Changer : MonoBehaviour
{

    private TextMeshProUGUI textMesh;
    int value;
    int maxVal = 100;
    int minVal = 0;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.buttonWest.wasPressedThisFrame))
        {
            value -= 5;
            if (value < minVal)
            {
                value = minVal;
            }
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame))
        {
            value += 5;
            if (value > maxVal)
            {
                value = maxVal;
            }
        }
        textMesh.text = value.ToString();
    }
}
