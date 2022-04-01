using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectSystem : MonoBehaviour
{
    public static CharacterSelectSystem _instance;
    [SerializeField] CharacterSelectPage[] _characterSelectPages;

    private void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static CharacterSelectSystem Get()
    {
        return _instance;
    }

    public void SetPage(int ID)
    {
        DisablePages();

        _characterSelectPages[ID].EnablePage();
    }

    void DisablePages()
    {
        foreach (CharacterSelectPage page in _characterSelectPages)
        {
            if (page.isActiveAndEnabled)
            {
                page.DisablePage();
            }
        }
    }
}
