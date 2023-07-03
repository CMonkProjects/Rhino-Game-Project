using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

//Derived from a timer tutorial by John French (gamedevbeginner.com)
public class EnemySpawnWaveTimer : MonoBehaviour
{
    public float timerDefault = 10f;
    float timeRemaining;
    public bool timerIsRunning = false;

    public UnityEvent onTimerEnded;

    //include Text reference to UI
    public Text timerText;

    public void SetTimer(bool _bool)
    {
        timerIsRunning = _bool;

        if (timerIsRunning)
            timeRemaining = timerDefault;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                TimerEnded();
            }
        }
    }

    void TimerEnded()
    {
        onTimerEnded.Invoke();  //unity event
        timeRemaining = 0f;
        timerIsRunning = false;
        timerText.text = "";
    }

    //Display time
    void DisplayTime(float _timeToDisplay)
    {
        //float minutes = Mathf.FloorToInt(_timeToDisplay) / 60;
        float seconds = Mathf.FloorToInt(_timeToDisplay) % 60;
        float milliseconds = (_timeToDisplay % 1) * 1000;

        timerText.text = string.Format("{0:00}:{1:00}", seconds, milliseconds);
    }
}
