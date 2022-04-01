using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayMenuPage : MainMenuPage
{
    public override void Start()
    {
        if (_backButton)
        {
            _backButton.onClick.AddListener(() => MainMenuSystem.Get().SetPage(1));
        }
    }

}
