using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPage : MonoBehaviour
{
    public void EnablePage()
    {
        gameObject.SetActive(true);
    }

    public void DisablePage()
    {
        gameObject.SetActive(false);
    }
}
