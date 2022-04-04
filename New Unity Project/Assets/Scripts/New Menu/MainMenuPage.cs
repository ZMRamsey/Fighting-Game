using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPage : MonoBehaviour
{
    public UIButton _backButton;
    public UIButton _quitButton;

    void Update() {
        if (_backButton && _backButton.OnClick()) {
            MainMenuSystem.Get().SetPage(0);
        }
        if (_quitButton && _quitButton.OnClick())
        {
            Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
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
