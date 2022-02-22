using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class LoadingTimeController : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dateText;


    int hour;
    int minute;
    int year;
    int date;
    int month;

    string minutes;
    string months;

    private void Start()
    {
        //timeText = timeText.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        hour = System.DateTime.Now.Hour;
        minute = System.DateTime.Now.Minute;
        if(minute < 10)
        {
            minutes = "0" + minute;
        }
        else
        {
            minutes = ""+minute;
        }
        
        if(month < 10)
        {
            months = "0" + month;
        }
        else
        {
            months = ""+month;
        }
        
        date = System.DateTime.Now.Day;
        month = System.DateTime.Now.Month;

        timeText.text = "" + hour + ":" + minutes;
        dateText.text = "" + date + "." + months + ".XX";
    }
}
