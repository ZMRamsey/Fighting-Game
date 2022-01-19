using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scroller : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI quoteText;
    [SerializeField] string text;
    [SerializeField] int index;
    [SerializeField] float speed;
    [SerializeField] float timer;
    // Update is called once per frame

    private void Awake()
    {

    }

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
