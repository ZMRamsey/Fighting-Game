using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class TimerManager : MonoBehaviour
{
    bool timerActive = false;
    [SerializeField] float currentTime = 300;
    [SerializeField] float startMinutes;
    public TextMeshProUGUI currentTimeText;

    // Start is called before the first frame update
    void Start()
    {
        timerActive = true;
        //currentTime = startMinutes * 60;
    }

    // Update is called once per frame
    void Update()
    {

        if (timerActive && GameManager.Get().KOCoroutine == null)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                timerActive = false;
            }

            currentTimeText.text = currentTime.ToString().Split('.')[0];
        }
        else
        {
            //currentTimeText.text = "Match Over!";
        }
    }

    public void EndMatch()
    {
    }



}
