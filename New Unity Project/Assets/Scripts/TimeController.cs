using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class TimeController : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    int hour;
    int minute;
    int year;
    int date;
    int month;

    private void Start()
    {
        //timeText = timeText.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        hour = System.DateTime.Now.Hour;
        minute = System.DateTime.Now.Minute;

        timeText.text = "" + hour + ":" + minute;
    }
}
