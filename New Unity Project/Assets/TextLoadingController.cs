using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class TextLoadingController : MonoBehaviour
{
    float currentOpac = 0f;
    float opac = 0;
    public TextMeshProUGUI loadingText;

    // Update is called once per frame
    void Update()
    {
        currentOpac = Mathf.Cos(opac);
        opac += 0.001f;
        if(currentOpac < 0)
        {
            currentOpac *= -1;
        }
        Debug.Log(currentOpac);
        loadingText.color = new Color(0f,0f,0f,currentOpac);
    }
}
