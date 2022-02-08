using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class optionsRotatorCircle : MonoBehaviour
{
    public float turningRate = 0.75f;
    int sceneIndex = 0;
    int subSceneIndex = 10;
    float _rotatorConstant = 0.0f;

    int firstOption = 0;
    int lastOption = 2;

    //Display Options
    int resIndex = 0;

    bool _hasGoneUp = false;
    bool _hasGoneDown = false;
    bool _hasWrappedAroundToPlay = false;
    bool _hasWrappedAroundToQuit = false;
    bool _isControllingSubMenu = false;


    public GameObject[] subMenus;
    public GameObject[] subMenuOptions;
    public GameObject[] subMenuSelections;

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

    //Change system for audio usage
    void ChangeToAudio()
    {
        firstOption = 3;
        lastOption = 5;
    }
    void ChangeToDisplay()
    {
        firstOption = 0;
        lastOption = 2;
    }

    void SubMenuMoveUp()
    {
        //Test code for Audio options
        //if(sceneIndex == 0)
        //{
        //    subSceneIndex = 0;
        //}
        //else if(subSceneIndex == 1)
        //{
        //    subSceneIndex = 3;
        //}

        subMenuOptions[subSceneIndex].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.75f);
        subMenuOptions[subSceneIndex+1].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        subMenuSelections[subSceneIndex].GetComponent<FullScreenRotator>().enabled = true;
        subMenuSelections[subSceneIndex+1].GetComponent<FullScreenRotator>().enabled = false;
    }
    void SubMenuMoveDown()
    {
        subMenuOptions[subSceneIndex].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.75f);
        subMenuOptions[subSceneIndex-1].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        subMenuSelections[subSceneIndex].GetComponent<FullScreenRotator>().enabled = true;
        subMenuSelections[subSceneIndex-1].GetComponent<FullScreenRotator>().enabled = false;
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
            if(sceneIndex == 0)
            {
                subSceneIndex = 0;
                subMenuSelections[0].GetComponent<FullScreenRotator>().enabled = true;
            }
            else if(sceneIndex == 1)
            {
                subSceneIndex = 3;
                subMenuSelections[3].GetComponent<FullScreenRotator>().enabled = true;
            }
            subMenuOptions[subSceneIndex].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.75f);
            _isControllingSubMenu = true;
        }
    }
    void controlSelectionWheel()
    {
        _isControllingSubMenu = false;

        for(int i = 0; i<subMenuSelections.Length; i++)
        {
            subMenuSelections[i].GetComponent<FullScreenRotator>().enabled = false;
        }
        subMenuOptions[subSceneIndex].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        subSceneIndex = 10;
    }

    void ReturnToMenu()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        backgroundPanel.GetComponent<Image>().sprite = mainMenuBG;
        subSceneIndex = 4;
        sceneIndex = 0;
        ChangeToDisplay();
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

        //WIP
        if(sceneIndex == 0)
        {
            ChangeToDisplay();
        }
        else if(sceneIndex == 1)
        {
            ChangeToAudio();
        }
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (sceneIndex != 2)
            {
                controlSubMenu();
            }
            
        }
        else if (Keyboard.current.zKey.wasPressedThisFrame)
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
                if(subSceneIndex < firstOption)
                {
                    subSceneIndex = firstOption;
                }
                else
                {
                    SubMenuMoveUp();
                }
            }
            else if (Keyboard.current.sKey.wasPressedThisFrame)
            {
                subSceneIndex += 1;
                if (subSceneIndex > lastOption)
                {
                    subSceneIndex = lastOption;
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
