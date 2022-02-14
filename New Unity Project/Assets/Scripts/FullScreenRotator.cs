using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class FullScreenRotator : MonoBehaviour
{
    float _rotatorConstant = 0.0f;
    private Quaternion _targetRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            _rotatorConstant += 180.0f;
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            _rotatorConstant -= 180.0f;
        }

        _targetRotation = Quaternion.Euler(0.0f, 0.0f, _rotatorConstant);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, 2.0f);
    }
}
