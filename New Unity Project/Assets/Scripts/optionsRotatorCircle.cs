using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class optionsRotatorCircle : MonoBehaviour
{
    public moveMenus men;
    public FullScreenRotator fullRot;

    public TextMeshProUGUI[] _options;

    public float turningRate = 0.75f;
    int sceneIndex = 0;
    public int subSceneIndex = 10;
    float _rotatorConstant = 0.0f;

    int firstOption = 0;
    int lastOption = 2;

    //Confirm Selection(s)
    public TextMeshProUGUI confirmText;
    public GameObject confirmSelectionBox;
    public bool _isConfirmingOptions;
    public bool _hasConfirmedOptions;

    //Display Options
    int resIndex = 0;

    //Options Border
    public GameObject border50opac;
    public GameObject border100opac;

    bool _hasGoneUp = false;
    bool _hasGoneDown = false;
    bool _hasWrappedAroundToPlay = false;
    bool _hasWrappedAroundToQuit = false;
    bool _isControllingSubMenu = false;

    bool _returnSelected = false;
    bool _choicesConfirmed = false;

    public GameObject[] subMenus;
    public GameObject[] subMenuOptions;
    public GameObject[] subMenuSelections;

    public GameObject mainMenu;
    public GameObject optionsMenu;

    public GameObject backgroundPanel;
    public Sprite playMenuBG;
    public Sprite optionsMenuBG;

    public Sprite mainMenuBG;

    public GameObject[] mainMenuAssets;
    public GameObject[] optionsMenuAssets;

    public GameObject mainMenuScript;
    public GameObject optionsScript;

    Vector3 mainDesiredPos = new Vector3(2055.5352f, 310.5f, 90f);

    Vector3 optionsDesiredPos = new Vector3(625.0707f, 310.5f, 90f);

    private Quaternion _targetRotation = Quaternion.Euler(0.0f,0.0f, 0.0f);

    void PanToMenu()
    {
        men._movingToOptions = false;
        men._movingToMain = true;

    }

    void Start()
    {
        Debug.Log("Options Active");
        mainMenuScript.GetComponent<rotaterCircle>().enabled = false;
        _returnSelected = false;
        Screen.SetResolution(1920,1080,true);
    }

    void SlideToMainMenu()
    {
        mainDesiredPos = new Vector3(620.8455f, 310.5f, 90f);
        optionsDesiredPos = new Vector3(-813.8442f, 310.5f, 90f);
        _returnSelected = true;
        
    }

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
        if(sceneIndex == 0)
        {
            subMenuSelections[subSceneIndex].GetComponent<FullScreenRotator>().enabled = true;
            subMenuSelections[subSceneIndex + 1].GetComponent<FullScreenRotator>().enabled = false;
        }
        else if (sceneIndex == 1)
        {
            subMenuSelections[subSceneIndex].GetComponent<Audio_Value_Changer>().enabled = true;
            subMenuSelections[subSceneIndex+1].GetComponent<Audio_Value_Changer>().enabled = false;
        }

        subMenuOptions[subSceneIndex].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.75f);
        subMenuOptions[subSceneIndex+1].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        //fullRot.qualArrSelector = 0;
        //fullRot.resolutionsArrSelector = 0;
    }
    void SubMenuMoveDown()
    {
        if (sceneIndex == 0)
        {
            subMenuSelections[subSceneIndex].GetComponent<FullScreenRotator>().enabled = true;
            subMenuSelections[subSceneIndex - 1].GetComponent<FullScreenRotator>().enabled = false;
        }
        else if (sceneIndex == 1)
        {
            subMenuSelections[subSceneIndex].GetComponent<Audio_Value_Changer>().enabled = true;
            subMenuSelections[subSceneIndex - 1].GetComponent<Audio_Value_Changer>().enabled = false;
        }
        subMenuOptions[subSceneIndex].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.75f);
        subMenuOptions[subSceneIndex-1].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        //fullRot.qualArrSelector = 0;
        //fullRot.resolutionsArrSelector = 0;
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
                subMenuSelections[3].GetComponent<Audio_Value_Changer>().enabled = true;
            }
            subMenuOptions[subSceneIndex].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.75f);
            _isControllingSubMenu = true;
        }
    }

    //void ConfirmSettingsChoices()
    //{
    //    confirmSelectionBox.SetActive(true);
    //    _isConfirmingOptions = true;
    //}
    void controlSelectionWheel()
    {
        _isControllingSubMenu = false;

        for(int i = 0; i<3; i++)
        {
            subMenuSelections[i].GetComponent<FullScreenRotator>().enabled = false;
        }

        subMenuOptions[subSceneIndex].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        subSceneIndex = 10;
        //ConfirmSettingsChoices();
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

        if(sceneIndex == 2)
        {
            DisableAllBorders();
        }
    }

    void EnableOptionsBorder()
    {
        border50opac.SetActive(false);
        border100opac.SetActive(true);
    }
    void DisableOptionsBorder()
    {
        border50opac.SetActive(true);
        border100opac.SetActive(false);
    }
    void DisableAllBorders()
    {
        border50opac.SetActive(false);
        border100opac.SetActive(false);
    }

    void ResetOptions()
    {
        _isConfirmingOptions = false;
        fullRot.fullscreenEnabled = true;
        fullRot.resolutionsArrSelector = 0;
        fullRot.qualArrSelector = 0;
    }

    void HasSelectedOptions()
    {
        confirmSelectionBox.SetActive(false);
        _isConfirmingOptions = false;
    }

    void SetDisplayTextWhite()
    {
        _options[0].color = Color.white;
        _options[3].color = Color.white;
        _options[6].color = Color.white;
        _options[9].color = Color.white;

        _options[1].color = Color.black;
        _options[4].color = Color.black;
        _options[7].color = Color.black;
        _options[10].color = Color.black;

        _options[2].color = Color.black;
        _options[5].color = Color.black;
        _options[8].color = Color.black;
        _options[11].color = Color.black;
    }

    void SetAudioTextWhite()
    {
        _options[0].color = Color.black;
        _options[3].color = Color.black;
        _options[6].color = Color.black;
        _options[9].color = Color.black;

        _options[1].color = Color.white;
        _options[4].color = Color.white;
        _options[7].color = Color.white;
        _options[10].color = Color.white;

        _options[2].color = Color.black;
        _options[5].color = Color.black;
        _options[8].color = Color.black;
        _options[11].color = Color.black;
    }

    void SetReturnTextWhite()
    {
        _options[0].color = Color.black;
        _options[3].color = Color.black;
        _options[6].color = Color.black;
        _options[9].color = Color.black;

        _options[1].color = Color.black;
        _options[4].color = Color.black;
        _options[7].color = Color.black;
        _options[10].color = Color.black;

        _options[2].color = Color.white;
        _options[5].color = Color.white;
        _options[8].color = Color.white;
        _options[11].color = Color.white;
    }

    private void Update()
    {
        if (sceneIndex == 0)
        {
            SetDisplayTextWhite();
        }
        else if (sceneIndex == 1)
        {
            SetAudioTextWhite();
        }
        else
        {
            SetReturnTextWhite();
        }

        mainMenuScript.GetComponent<rotaterCircle>().enabled = false;
        if (GlobalInputManager.Get().GetSubmitInput())
        {
            if (sceneIndex != 2)
            {
                controlSubMenu();
            }
            
        }
        else if (GlobalInputManager.Get().GetBackInput())
        {
            controlSelectionWheel();
        }

        if (_isControllingSubMenu == false)
        {
            DisableOptionsBorder();
            if (!_isConfirmingOptions)
            {
                if (GlobalInputManager.Get().GetUpInput())
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
                else if (GlobalInputManager.Get().GetDownInput())
                {
                    Debug.Log("Options Move Down Submenu");
                    _rotatorConstant -= 30.0f;
                    sceneIndex += 1;
                    MoveUp();
                    if (sceneIndex > 2)
                    {
                        sceneIndex = 0;
                        WrapAroundToPlay();
                    }
                }
                else if (GlobalInputManager.Get().GetSubmitInput())
                {
                    if (sceneIndex == 2)
                    {
                        PanToMenu();
                    }
                }
            }
            else
            {
                
                //if(GlobalInputManager.Get().GetSubmitInput())
                //{
                //    _hasConfirmedOptions = true;
                //    HasSelectedOptions();
                //}
                //else if(GlobalInputManager.Get().GetBackInput())
                //{
                //    _hasConfirmedOptions = false;
                //    HasSelectedOptions();
                //}
            }
            

        }
        else
        {
            EnableOptionsBorder();
            if (GlobalInputManager.Get().GetUpInput())
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
            else if (GlobalInputManager.Get().GetDownInput())
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

        //for (int i = 0; i < 2; i++)
        //{
        //    mainMenuAssets[i].transform.position = Vector3.MoveTowards(backgroundPanel.transform.position, mainDesiredPos, 800f * Time.deltaTime);
        //    optionsMenuAssets[i].transform.position = Vector3.MoveTowards(optionsMenuAssets[0].transform.position, optionsDesiredPos, 800f * Time.deltaTime);
        //}

        //if (_returnSelected)
        //{
        //    //if (backgroundPanel.transform.position.x < -750)
        //    //{
        //    //    for (int i = 0; i < 2; i++)
        //    //    {
        //    //        optionsMenuAssets[i].SetActive(false);
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    for (int i = 0; i < 2; i++)
        //    //    {
        //    //        optionsMenuAssets[i].SetActive(true);
        //    //    }
        //    //}

        //    if (mainMenuAssets[0].transform.position.x < 630)
        //    {
        //        //for (int i = 0; i < 2; i++)
        //        //{
        //        //    optionsMenuAssets[i].SetActive(false);
        //        //}
        //        mainMenuScript.GetComponent<rotaterCircle>().enabled = true;

        //        Debug.Log("Returned Back");
        //    }
        //    //for (int i = 0; i < 2; i++)
        //    //{
        //    //    optionsMenuAssets[i].SetActive(true);
        //    //}
        //}

        _targetRotation = Quaternion.Euler(0.0f, 0.0f, _rotatorConstant);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, 100f * Time.deltaTime);
    }
}
