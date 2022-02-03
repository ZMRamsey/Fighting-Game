using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class optionsRotatorCircle : MonoBehaviour
{
    public float turningRate = 0.75f;
    int sceneIndex = 0;
    int subSceneIndex = 0;
    float _rotatorConstant = 0.0f;

    bool _hasGoneUp = false;
    bool _hasGoneDown = false;
    bool _hasWrappedAroundToPlay = false;
    bool _hasWrappedAroundToQuit = false;
    bool _isControllingSubMenu = false;


    public GameObject[] subMenus;
    public GameObject[] subMenuOptions;

    public GameObject mainMenu;
    public GameObject optionsMenu;

    public GameObject backgroundPanel;
    public Sprite playMenuBG;
    public Sprite optionsMenuBG;

    public Sprite mainMenuBG;

    private Quaternion _targetRotation = Quaternion.Euler(0.0f,0.0f, 0.0f);

    void MoveUp()
    {
        _hasGoneUp = true;
        _hasGoneDown = false;
    }
    void MoveDown()
    {
        _hasGoneUp = false;
        _hasGoneDown = true;
    }
    void NotMove()
    {
        _hasGoneUp = false;
        _hasGoneDown = false;
    }

    void SubMenuMoveUp()
    {
        subMenuOptions[subSceneIndex].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        subMenuOptions[subSceneIndex+1].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
    }
    void SubMenuMoveDown()
    {
        subMenuOptions[subSceneIndex].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        subMenuOptions[subSceneIndex-1].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
    }

    void WrapAroundToPlay()
    {
        _hasWrappedAroundToPlay = true;
        _hasWrappedAroundToQuit = false;
        _hasGoneUp = false;
        _hasGoneDown = false;
    }
    void WrapAroundToQuit()
    {
        _hasWrappedAroundToPlay = false;
        _hasWrappedAroundToQuit = true;
        _hasGoneUp = false;
        _hasGoneDown = false;
    }

    void controlSubMenu()
    {
        if(_isControllingSubMenu == false)
        {
            subSceneIndex = 0;
            subMenuOptions[subSceneIndex].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            _isControllingSubMenu = true;
        }
    }
    void controlSelectionWheel()
    {
        _isControllingSubMenu = false;
        subMenuOptions[subSceneIndex].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        subSceneIndex = 0;
    }

    void ReturnToMenu()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        backgroundPanel.GetComponent<Image>().sprite = mainMenuBG;
    }

    void highlightSelectedOption()
    {
        if(_hasGoneUp == true)
        {
            subMenus[sceneIndex - 1].SetActive(false);
        }
        else if(_hasGoneDown == true)
        {
            subMenus[sceneIndex+1].SetActive(false);
        }
        else if(_hasWrappedAroundToPlay == true)
        {
            subMenus[2].SetActive(false);
        }
        else if(_hasWrappedAroundToQuit == true)
        {
            subMenus[0].SetActive(false);
        }
        subMenus[sceneIndex].SetActive(true);
    }

    private void Update()
    {
        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            controlSubMenu();
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            controlSelectionWheel();
        }

        if (_isControllingSubMenu == false)
        {
            if (Keyboard.current.wKey.wasPressedThisFrame)
            {
                _rotatorConstant -= 30.0f;
                sceneIndex += 1;
                MoveUp();
                if (sceneIndex > 2)
                {
                    sceneIndex = 0;
                    WrapAroundToPlay();
                }
            }
            else if (Keyboard.current.sKey.wasPressedThisFrame)
            {
                _rotatorConstant += 30.0f;
                sceneIndex -= 1;
                MoveDown();
                if (sceneIndex < 0)
                {
                    sceneIndex = 2;
                    WrapAroundToQuit();
                }
            }
            else if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if (sceneIndex == 2)
                {
                    ReturnToMenu();
                }
            }

        }
        else
        {
            if (Keyboard.current.wKey.wasPressedThisFrame)
            {
                subSceneIndex -= 1;
                if(subSceneIndex < 0)
                {
                    subSceneIndex = 0;
                }
                else
                {
                    SubMenuMoveUp();
                }
            }
            else if (Keyboard.current.sKey.wasPressedThisFrame)
            {
                subSceneIndex += 1;
                if (subSceneIndex > 2)
                {
                    subSceneIndex = 2;
                }
                else
                {
                    SubMenuMoveDown();
                }
            }
        }
        highlightSelectedOption();
        _targetRotation = Quaternion.Euler(0.0f, 0.0f, _rotatorConstant);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, 0.25f);
    }
}
