using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    [SerializeField] private Text uiText;
    [SerializeField] private float maintimer;

    private float timer;
    private bool canCount = true;
    private bool doOnce = false;
    private int seconds;
    private int minutes;

    private void Start()
    {
        timer = maintimer;
    }

    void Update()
    {
        if (timer >= 0.0f && canCount)
        {
            timer -= Time.deltaTime;
            seconds = (int)(timer % 60);
            minutes = (int)(timer / 60) % 60;
            uiText.text = string.Format("{0:0}:{1:0}", minutes, seconds);
        }   
        else if (timer <= 0.0f && !doOnce)
        {
            canCount = false;
            doOnce = true;
            uiText.text = "0.00";
            timer = 0.0f;
        }
    }



    /*void TimerCounter()
    {
        timerParameter = Time.time - startTime;
        string minutes = ((int)timerParameter / 60).ToString();
        string seconds = ((timerParameter % 60).ToString("f2"));
        TimerText.text = minutes + ":" + seconds;
        timeUI.text = TimerText.text;
    }*/


}
