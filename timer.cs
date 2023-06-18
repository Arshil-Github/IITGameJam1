using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class timer : MonoBehaviour
{

    public Text timerMinutes;
    public Text timerSeconds;
    public Text timerSeconds100;

    private float startTime;
    private float stopTime;
    private float timerTime;
    private bool isRunning = false;

    public float turnBackTimeInSec;

    public UnityEvent startOfTimer;
    public UnityEvent endOfTimer;
    // Use this for initialization
    void Start()
    {
        TimerReset();
    }

    public void TimerStart()
    {
        stopTime = FindObjectOfType<GameMaster>().TurnBackTime;

        startOfTimer.Invoke();
        if (!isRunning)
        {
            print("START");
            isRunning = true;
            startTime = Time.time;
        }
        int minutesInt = (int)stopTime / 60;
        int secondsInt = (int)stopTime % 60;
        int seconds100Int = (int)(Mathf.Floor((stopTime - (secondsInt + minutesInt * 60)) * 100));

        if (isRunning)
        {
            timerMinutes.text = (minutesInt < 10) ? "0" + minutesInt : minutesInt.ToString();
            timerSeconds.text = (secondsInt < 10) ? "0" + secondsInt : secondsInt.ToString();
            timerSeconds100.text = (seconds100Int < 10) ? "0" + seconds100Int : seconds100Int.ToString();
        }
    }

    public void TimerStop()
    {
        endOfTimer.Invoke();
        if (isRunning)
        {
            print("STOP");
            isRunning = false;
            timerTime = 00;

            timerMinutes.text = "00";
            timerSeconds.text = "00";
            timerSeconds100.text = "00";

            isRunning = false;

        }
    }

    public void TimerReset()
    {
        print("RESET");
        stopTime = turnBackTimeInSec;
        isRunning = false;
        timerMinutes.text = timerSeconds.text = timerSeconds100.text = "00";
    }

    // Update is called once per frame
    void Update()
    {
        timerTime = stopTime - (Time.time - startTime);
        int minutesInt = (int)timerTime / 60;
        int secondsInt = (int)timerTime % 60;
        int seconds100Int = (int)(Mathf.Floor((timerTime - (secondsInt + minutesInt * 60)) * 100));

        if (isRunning)
        {
            timerMinutes.text = (minutesInt < 10) ? "0" + minutesInt : minutesInt.ToString();
            timerSeconds.text = (secondsInt < 10) ? "0" + secondsInt : secondsInt.ToString();
            timerSeconds100.text = (seconds100Int < 10) ? "0" + seconds100Int : seconds100Int.ToString();
        }
        if (minutesInt < 0 || seconds100Int < 0 || secondsInt < 0)
        {
            TimerStop();
        }
    }
}