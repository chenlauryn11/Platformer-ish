using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    //Holds PlayerControl
    [SerializeField] PlayerControl player;

    //Holds the statistics script
    [SerializeField] Stats s;

    //Holds the time remaining text
    [SerializeField] TextMeshProUGUI timerText;

    //Initializes how much time the player has at the start of the game
    [SerializeField] float startTime;

    //Keeps track of how much time (in seconds) the player has left
    public float timeRemaining = 0f;

    //Initializes the timer to not running
    [SerializeField] bool timerIsRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        //Set start time to 120 seconds (2 minutes)
        startTime = 120f;

        //Set the time remaining to the start time
        timeRemaining = startTime;

        //Starts the timer automatically
        timerIsRunning = true;

        s.totalTime = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        //Checks to see if the timer is still running
        if (timerIsRunning)
        {
            //If time is still left ... 
            if (timeRemaining > 0)
            {
                //Subtract how much time has passed from timeRemaining
                timeRemaining -= Time.deltaTime;
            }

            //If time has run out ...
            else
            {
                //Set timeRemaining to 0, so there isn't negative time
                timeRemaining = 0;

                //Stop the timer from running
                timerIsRunning = false;

                player.gameOver = true;
            }

            timerText.text = "Time Remaining: " + formatTime(timeRemaining);

            s.timeRemaining = timeRemaining;
        }
    }

    public void pause()
    {
        timerIsRunning = false;
    }

    public void resume()
    {
        timerIsRunning = true;
    }

    public float TimeRemaining
    {
        get { return timeRemaining; }
    }

    public bool isTimerRunning
    {
        get { return timerIsRunning; }
    }

    private string formatTime(float t)
    {
        return string.Format("{0:0}:{1:00}", Mathf.FloorToInt(t / 60), Mathf.FloorToInt(t % 60));
    }
}
