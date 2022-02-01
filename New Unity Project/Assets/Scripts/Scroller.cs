using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scroller : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI quoteText;
    [SerializeField] public string text;
    [SerializeField] float speed;

    int index;
    float timer;

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer += speed;
            index++;
            quoteText.text = text.Substring(0, index);
        }
        if (index == text.Length)
        {
            enabled = false;
        }
    }
}
