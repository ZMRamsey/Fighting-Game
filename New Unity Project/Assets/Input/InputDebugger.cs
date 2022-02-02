using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputDebugger : MonoBehaviour
{
    public void PressX(InputAction.CallbackContext context)
    {
        Debug.Log("X");
    }

    public void PressO(InputAction.CallbackContext context)
    {
        Debug.Log("O");
    }

    public void PressA(InputAction.CallbackContext context)
    {
        Debug.Log("Tri");
    }

    public void PressL(InputAction.CallbackContext context)
    {
        Debug.Log("[]");
    }

    public void PressUp(InputAction.CallbackContext context)
    {
        Debug.Log("^");
    }

    public void PressDown(InputAction.CallbackContext context)
    {
        Debug.Log("v");
    }

    public void PressRight(InputAction.CallbackContext context)
    {
        Debug.Log(">");
    }

    public void PressLeft(InputAction.CallbackContext context)
    {
        Debug.Log("<");
    }
}
