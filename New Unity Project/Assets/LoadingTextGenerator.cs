using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingTextGenerator : MonoBehaviour
{
    public int _maxQuotes;
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
        randQuoteNum = Random.Range(0, _maxQuotes);
        scrollingText.text = loadingFacts[randQuoteNum];
        Debug.Log(randQuoteNum.ToString());
    }

    private void Update()
    {
        
    }
}
