using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class FullScreenRotator : MonoBehaviour
{

    public optionsRotatorCircle rot;

    float _rotatorConstant = 0.0f;
    private Quaternion _targetRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    public string[] resolutionsArr;
    public int resolutionsArrSelector;
    int _minRes = 0;
    int _maxRes = 2;

    public string[] qualArr;
    public int qualArrSelector;
    int _minQual = 0;
    int _maxQual = 2;

    public bool fullscreenEnabled = true;
    public TextMeshProUGUI fullscreenText;
    public TextMeshProUGUI resolutionText;
    public TextMeshProUGUI qualityText;

    // Update is called once per frame
    void Update()
    {
        if(rot.subSceneIndex < 1)
        {
            Debug.Log("Fullscreen");
            if (GlobalInputManager.Get().GetLeftInput() || GlobalInputManager.Get().GetRightInput())
            {
                fullscreenEnabled = !fullscreenEnabled;
            }
            fullscreenText.text = fullscreenEnabled.ToString();
            Screen.fullScreen = fullscreenEnabled;
        }
        if(rot.subSceneIndex == 1)
        {
            Debug.Log("Resolution");
            if (Keyboard.current.aKey.wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.leftStick.left.wasPressedThisFrame))
            {
                resolutionsArrSelector -= 1;
                if(resolutionsArrSelector < _minRes)
                {
                    resolutionsArrSelector = _minRes;
                }
            }
            else if(Keyboard.current.dKey.wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.leftStick.right.wasPressedThisFrame))
            {
                resolutionsArrSelector += 1;
                if (resolutionsArrSelector > _maxRes)
                {
                    resolutionsArrSelector = _maxRes;
                }
            }
            resolutionText.text = resolutionsArr[resolutionsArrSelector];
            //if (resolutionsArrSelector == 0)
            //{
            //    Screen.SetResolution(1920, 1080, fullscreenEnabled);
            //}
            //else if (resolutionsArrSelector == 1)
            //{
            //    Screen.SetResolution(1280, 720, fullscreenEnabled);
            //}
            //else
            //{
            //    Screen.SetResolution(720, 576, fullscreenEnabled);
            //}
        }
        if(rot.subSceneIndex == 2)
        {
            Debug.Log("Quality");
            if (Keyboard.current.aKey.wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.leftStick.left.wasPressedThisFrame))
            {
                qualArrSelector -= 1;
                if (qualArrSelector < _minQual)
                {
                    qualArrSelector = _minQual;
                }
            }
            else if (Keyboard.current.dKey.wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.leftStick.right.wasPressedThisFrame))
            {
                qualArrSelector += 1;
                if (qualArrSelector > _maxQual)
                {
                    qualArrSelector = _maxQual;
                }
            }
            qualityText.text = qualArr[qualArrSelector];
            QualitySettings.SetQualityLevel(qualArrSelector);
        }
        

    }
}
