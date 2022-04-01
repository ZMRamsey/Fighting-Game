using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPage : MonoBehaviour
{
    public UIButton _backButton;

    void Update() {
        if (_backButton && _backButton.OnClick()) {
            MainMenuSystem.Get().SetPage(0);
        }
    }

    public void EnablePage()
    {
        gameObject.SetActive(true);
    }

    public void DisablePage()
    {
        gameObject.SetActive(false);
    }
}
