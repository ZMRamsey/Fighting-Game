using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuRacketRotator : MonoBehaviour
{
    
    private Quaternion _targetRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

    public float _rotatorConstant = 0.0f;

    // Update is called once per frame
    void Update()
    {
        _targetRotation = Quaternion.Euler(0.0f, 0.0f, _rotatorConstant);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, 0.3f);
    }
}
