using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    private bool _active;
    [SerializeField] GameObject _pausePanel;
    [SerializeField] GameObject _selector;

    //[SerializeField] TextMeshProUGUI _selector;
    //[SerializeField] TextMeshProUGUI _selector;
    //[SerializeField] TextMeshProUGUI _selector;
    //[SerializeField] TextMeshProUGUI _selector;


    // Update is called once per frame
    void Update()
    {
        
    }

    public void Enable() 
    { 
        _active = true;
        _pausePanel.SetActive(true);
    }
    public void Disable()
    {
        _active = false;
        _pausePanel.SetActive(false);
    }

    public bool IsActive()
    {
        return _active;
    }
}
