using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class transformChecker : MonoBehaviour
{
    public GameObject mapMenu;

    void Update()
    {
        Debug.Log("X: " + mapMenu.transform.position.x);
        Debug.Log("Y: " + mapMenu.transform.position.y);
        Debug.Log("Z: " + mapMenu.transform.position.z);
    }
}
