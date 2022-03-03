using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class rotaterCircle : MonoBehaviour
{
    public moveMenus men;

    public float turningRate = 0.75f;
    int sceneIndex = 0;
    float _rotatorConstant = 0.0f;

    bool _optionsSelected = false;
    bool _playSelected = false;
    bool _readyToPlay = false;

    int _playSubSceneIndex = 10;

    int _firstOption = 0;
    int _lastOption = 1;

    public static int _PlayerOptionSelected = 0;

    public GameObject playMenu;
    public GameObject optionsMenu;
    public GameObject mainMenu;

    public GameObject[] playMenuOptions;

    public GameObject backgroundPanel;
    public Sprite playMenuBG;
    public Sprite optionsMenuBG;

    public GameObject[] mainMenuAssets;
    public GameObject[] optionsMenuAssets;

    public GameObject optionsScript;
    public GameObject menuScript;

    public GameObject playSubMenu;

    //Test Start
    //Test End

    Vector3 mainDesiredPos = new Vector3(620.8455f, 310.5f, 90f);
    Vector3 finalPos = new Vector3(1926.31f, 0f , 0f);

    Vector3 optionsDesiredPos = new Vector3(-813.8442f, 310.5f, 90f);

    private Quaternion _targetRotation = Quaternion.Euler(0.0f,0.0f, 0.0f);

    void ControlPlaySubMenu()
    {
        
    }

    void PanToOptions()
    {
        men._movingToOptions = true;
        men._movingToMain = false;
    }

    void Start()
    {
        Debug.Log("Main Active");
        optionsScript.GetComponent<optionsRotatorCircle>().enabled = false;
        _optionsSelected = false;
        Screen.SetResolution(1920, 1080, true);
    }
    //void SlideToOptions()
    //{
    //    mainDesiredPos = new Vector3(2055.5352f, 310.5f, 90f);
    //    optionsDesiredPos = new Vector3(750.0707f, 310.5f, 90f);
    //    _optionsSelected = true;
    //}

    void loadSelectedMenu()
    {
        if (sceneIndex == 0)
        {
            playMenu.SetActive(true);
            mainMenu.SetActive(false);
            backgroundPanel.GetComponent<Image>().sprite = playMenuBG;
        }
        else if(sceneIndex == 1)
        {
            //backgroundPanel.GetComponent<Image>().sprite = optionsMenuBG;
            //optionsMenu.SetActive(true);
            //mainMenu.SetActive(false);
            //SlideToOptions();
            PanToOptions();
        }
        else if (sceneIndex == 2)
        {
            Application.Quit();
        }
    }

    void controlSubMenu()
    {
        if (!_playSelected)
        {
            _playSubSceneIndex = 0;
            playMenuOptions[_playSubSceneIndex].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.75f);
            _playSelected = true;
        }
    }

    void controlSelectionWheel()
    {
        _playSelected = false;
        playMenuOptions[_playSubSceneIndex].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        _playSubSceneIndex = 10;
        _readyToPlay = false;
    }

    void SubMenuMoveUp()
    {
        //if(_playSubSceneIndex == 1)
        //{
            Debug.Log("Move up");
            playMenuOptions[_playSubSceneIndex].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.75f);
            playMenuOptions[_playSubSceneIndex+1].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        //}
    }

    void SubMenuMoveDown()
    {
        //if (_playSubSceneIndex == 0)
        //{
            Debug.Log("Move down");
            playMenuOptions[_playSubSceneIndex].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.75f);
            playMenuOptions[_playSubSceneIndex - 1].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        //}
    }

    void PressPlay()
    {
        if(_playSubSceneIndex == 0)
        {
            _PlayerOptionSelected = 0;
        }
        else
        {
            _PlayerOptionSelected = 1;
        }
        SceneManager.LoadScene("InputTest");
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame))
        {
            if (sceneIndex == 0)
            {
                controlSubMenu();
            }

        }
        else if (Keyboard.current.zKey.wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame))
        {
            controlSelectionWheel();
        }

        if (sceneIndex == 0)
        {
            playSubMenu.SetActive(true);
        }
        else
        {
            playSubMenu.SetActive(false);
        }

        if (!_playSelected)
        {
            _targetRotation = Quaternion.Euler(0.0f, 0.0f, _rotatorConstant);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, 100f * Time.deltaTime);
            
            
            //optionsScript.GetComponent<optionsRotatorCircle>().enabled = false;
            if (Keyboard.current.wKey.wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.leftStick.up.wasPressedThisFrame))
            {
                if (!_optionsSelected)
                {
                    _rotatorConstant += 30.0f;
                    sceneIndex += 1;
                    if (sceneIndex > 2)
                    {
                        sceneIndex = 0;
                    }
                }

            }
            else if (Keyboard.current.sKey.wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.leftStick.down.wasPressedThisFrame))
            {
                if (!_optionsSelected)
                {
                    _rotatorConstant -= 30.0f;
                    sceneIndex -= 1;
                    if (sceneIndex < 0)
                    {
                        sceneIndex = 2;
                    }
                }
            }
            else if (Keyboard.current.spaceKey.wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame))
            {
                if (!_optionsSelected)
                {
                    loadSelectedMenu();
                }

            }

            //Move/Slide Main Menu Settings
            //for(int i = 0; i < 2; i++)
            //{
            //    mainMenuAssets[i].transform.position = Vector3.MoveTowards(backgroundPanel.transform.position, mainDesiredPos, 800f * Time.deltaTime);
            //    optionsMenuAssets[i].transform.position = Vector3.MoveTowards(optionsMenuAssets[0].transform.position, optionsDesiredPos, 800f * Time.deltaTime);
            //}
            ////Disable Main Menu Assets when moved and at final position
            //if (_optionsSelected)
            //{
            //    //if (backgroundPanel.transform.position.x > 2050)
            //    //{
            //    //    for (int i = 0; i < 2; i++)
            //    //    {
            //    //        mainMenuAssets[i].SetActive(false);
            //    //    }
            //    //}
            //    //else
            //    //{
            //    //    for (int i = 0; i < 2; i++)
            //    //    {
            //    //        mainMenuAssets[i].SetActive(true);
            //    //    }
            //    //}

            //    if (optionsMenuAssets[0].transform.position.x > 600)
            //    {
            //        optionsScript.GetComponent<optionsRotatorCircle>().enabled = true;
            //        Debug.Log("Sent");
            //    }
            

        }
        else
        {
            if (_readyToPlay)
            {
                Debug.Log("Play 2");
                if (Keyboard.current.spaceKey.wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame))
                {
                    //PressPlay();
                    loadSelectedMenu();
                }
            }
            _readyToPlay = true;
            Debug.Log(_playSubSceneIndex);
            if (Keyboard.current.wKey.wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.leftStick.up.wasPressedThisFrame))
            {
                _playSubSceneIndex -= 1;
                if (_playSubSceneIndex < _firstOption)
                {
                    _playSubSceneIndex = _firstOption;
                }
                else
                {
                    SubMenuMoveUp();
                }
            }
            else if (Keyboard.current.sKey.wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.leftStick.down.wasPressedThisFrame))
            {
                _playSubSceneIndex += 1;
                if (_playSubSceneIndex > _lastOption)
                {
                    _playSubSceneIndex = _lastOption;
                }
                else
                {
                    SubMenuMoveDown();
                }
            }
           

        }





        //If player is currently controlling play sub menu options

    }





        
    }
