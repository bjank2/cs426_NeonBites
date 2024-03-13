using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timeRemaining = 60;
    bool timerIsRunning = false;
    public TMPro.TextMeshProUGUI TMP_Timer;


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
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }
    
    void DisplayTime(float timeToDisplay) {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        TMP_Timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void OnTimerEnd() {
        
    }

    public void StartTimer() {
        timerIsRunning = true; 
    }

    public void StopTimer() {
        timerIsRunning = false;
    }
}
