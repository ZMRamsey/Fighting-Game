using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class TextLoadingController : MonoBehaviour
{
    float currentOpac = 0f;
    float opac = 0;
    public TextMeshProUGUI loadingText;

    //public GameObject fun;

    int fadeoutCount = 0;

    // Update is called once per frame
    void Update()
    {
        currentOpac = Mathf.Cos(opac);
        opac += 0.001f;
        if(currentOpac < 0)
        {
            currentOpac *= -1;
        }

        if(currentOpac < 0.1)
        {
            fadeoutCount += 1;
            Debug.Log(fadeoutCount);
        }
        loadingText.color = new Color(0f,0f,0f,currentOpac);

        //
        //fun.transform.Rotate(currentOpac, opac, currentOpac);
        //

        if (fadeoutCount > 300)
        {
            loadingText.text = "Continue";
        }
        if(fadeoutCount > 300 && Keyboard.current.spaceKey.wasPressedThisFrame || Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            SceneManager.LoadScene("InputTest");
        }
    }
}
