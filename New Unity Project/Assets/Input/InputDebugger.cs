using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputDebugger : MonoBehaviour
{
    public void Yell(InputAction.CallbackContext context)
    {
        Debug.Log("AAAAAAAAAAAAAAA");
    }
}
