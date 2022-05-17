using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingTextGenerator : MonoBehaviour
{
    int randQuoteNum;
    public string[] loadingFacts;

    private TextMeshProUGUI scrollingText;

    public void Start()
    {
        scrollingText = GetComponent<TextMeshProUGUI>();
        ___LoadingUpdateText();
    }

    public void ___LoadingUpdateText()
    {
        randQuoteNum = Random.Range(0, loadingFacts.Length);
        scrollingText.text = loadingFacts[randQuoteNum];
        //Debug.Log(randQuoteNum.ToString());
    }
}
