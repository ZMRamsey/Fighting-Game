using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class TimerController : MonoBehaviour
{
    bool timerActive = false;
    float currentTime;
    public float startMinutes;
    public TextMeshProUGUI currentTimeText;

    // Start is called before the first frame update
    void Start()
    {
        timerActive = true;
        currentTime = startMinutes * 60;
    }

    // Update is called once per frame
    void Update()
    {

        if (timerActive)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                timerActive = false;
            }
            TimeSpan time = TimeSpan.FromSeconds(currentTime);
            currentTimeText.text = time.Seconds.ToString();
        }
        else
        {
            currentTimeText.text = "Match Over!";
        }
    }

    public void EndMatch()
    {
    }



}
