using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectPage : MonoBehaviour
{
    public Button _backButton;

    public void Start()
    {
        if (_backButton)
        {
            _backButton.onClick.AddListener(() => CharacterSelectSystem.Get().SetPage(0));
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
