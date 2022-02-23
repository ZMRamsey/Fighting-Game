using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class TimerManager : MonoBehaviour
{
    static TimerManager _instance;
    bool timerActive = false;
    float currentTime;
    bool redFlag = false;
    [SerializeField] float startMinutes;
    public TextMeshProUGUI currentTimeText;
    public Animator anim;
    protected Color textColor;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Awake()
    {
        _instance = this;
        timerActive = true;
        currentTime = startMinutes * 60;
        textColor = currentTimeText.color;
    }

    public static TimerManager Get()
    {
        return _instance;
    } 

    void ResetTimer()
    {
        timerActive = true;
        currentTime = startMinutes * 60;
        currentTimeText.text = currentTime.ToString().Split('.')[0];
        anim.enabled = false;
        currentTimeText.fontSize = 100.0f;
        currentTimeText.color = textColor;
        redFlag = false;
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

            if (timerActive)
            {
                currentTimeText.text = currentTime.ToString().Split('.')[0];
                if (currentTimeText.text == "30" && !redFlag)
                {
                    redTimer();
                }
            }
        }
        if (!timerActive && ScoreManager.Get().IsThereWinner())
        {
            timerActive = true;
            ScoreManager.Get().NextRound();
            GameManager.Get().NewRoundNeeded(true);
            ResetTimer();
        }
        if (!timerActive && !ScoreManager.Get().IsThereWinner())
        {
            anim.Play("OverTimer");
            currentTimeText.text = "OVERTIME";
            currentTimeText.fontSize = 60.0f;
        }
    }

    private void redTimer()
    {
        currentTimeText.color = Color.red;
        redFlag = true;
        anim.enabled = true;
        anim.Play("Timer");
    }

    public void EndMatch()
    {
    }


}
