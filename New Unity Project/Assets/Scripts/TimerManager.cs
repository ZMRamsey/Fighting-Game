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
    private float currentPointTimer;
    private int internalTarget;
    [SerializeField] int targetTime = 10;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Awake()
    {
        _instance = this;
        timerActive = true;
        currentTime = startMinutes * 60;
        currentTimeText.text = currentTime.ToString().Split('.')[0];
        textColor = currentTimeText.color;
    }

    public static TimerManager Get()
    {
        return _instance;
    } 

    public void ResetTimer()
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

        if (timerActive && GameManager.Get().KOCoroutine == null && GameManager.Get().EndGameCoroutine == null)
        {
            currentTime -= Time.deltaTime;
            if (timerActive && currentTime <= 0)
            {
                timerActive = false;
                currentTime = 0;
            }

            if (timerActive)
            {
                currentTimeText.text = currentTime.ToString().Split('.')[0];
                if (currentTimeText.text == "30" && !redFlag)
                {
                    redTimer();
                }
            }

            currentPointTimer += Time.deltaTime;
            
            if (GetCurrentPointTime() >= internalTarget)
            {
                GameManager.Get().GetShuttle().increaseBounces();
                internalTarget += targetTime;
            }
        }

        if (!timerActive && ScoreManager.Get().IsThereWinner() && currentTime == 0)
        {
            timerActive = true;
            ScoreManager.Get().NextRound();
            GameManager.Get().NewRoundNeeded(true);
            //ResetTimer();
        }
        if (!timerActive && !ScoreManager.Get().IsThereWinner() && currentTime == 0)
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

    public void SetTimerState(bool state)
    {
        timerActive = state;
    }

    private int GetCurrentPointTime()
    {
        return (int)Math.Floor(currentPointTimer);
    }

    public void ResetPointTimer()
    {
        currentPointTimer = 0;
        internalTarget = targetTime;
    }
}
