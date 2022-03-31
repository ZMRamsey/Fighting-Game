using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSystem : MonoBehaviour
{
    [SerializeField] MainMenuPage[] _mainMenuPages;

    void Start()
    {
        SetPage(0);
    }

    public void SetPage(int ID)
    {
        DisablePages();

        _mainMenuPages[ID].EnablePage();
    }

    void DisablePages()
    {
        foreach (MainMenuPage page in _mainMenuPages)
        {
            if (page.isActiveAndEnabled)
            {
                page.DisablePage();
            }
        }
    }

    public void LoadToCharacterSelect()
    {

    }
}
