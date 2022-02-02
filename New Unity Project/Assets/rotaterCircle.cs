using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class rotaterCircle : MonoBehaviour
{
    public float turningRate = 0.75f;
    int sceneIndex = 0;
    float _rotatorConstant = 0.0f;

    public GameObject playMenu;
    public GameObject optionsMenu;
    public GameObject mainMenu;

    public GameObject backgroundPanel;
    public Sprite optionsMenuBG;

    private Quaternion _targetRotation = Quaternion.Euler(0.0f,0.0f, 0.0f);

    void loadSelectedMenu()
    {
        if (sceneIndex == 0)
        {
            playMenu.SetActive(true);
            mainMenu.SetActive(false);
            backgroundPanel.GetComponent<Image>().sprite = optionsMenuBG;
        }
        else if(sceneIndex == 1)
        {
            optionsMenu.SetActive(true);
            mainMenu.SetActive(false);
        }
        else if (sceneIndex == 2)
        {
            Application.Quit();
        }
    }

    private void Update()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            _rotatorConstant += 30.0f;
            sceneIndex +=1;
            if(sceneIndex > 2)
            {
                sceneIndex = 0;
            }
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            _rotatorConstant -= 30.0f;
            sceneIndex -=1;
            if(sceneIndex < 0)
            {
                sceneIndex = 2;
            }
        }
        else if (Keyboard.current.spaceKey.wasPressedThisFrame){
            loadSelectedMenu();
        }
        _targetRotation = Quaternion.Euler(0.0f, 0.0f, _rotatorConstant);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, 0.25f);
    }
}
