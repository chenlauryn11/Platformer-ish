using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stats : MonoBehaviour
{
    public int enemiesKilled = 0;

    public float timeRemaining = 0;
    public float timeTaken = 0;
    public float totalTime = 0;

    [SerializeField] GameObject statsCanvas;
    [SerializeField] TextMeshProUGUI annText, killText, timeText;

    // Start is called before the first frame update
    void Start()
    {
        statsCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateStats()
    {
        timeTaken = totalTime - timeRemaining;
    }

    public void showStats(bool died)
    {
        updateStats();
        statsCanvas.SetActive(true);

        if (died)
        {
            annText.text = "You died!\nBetter luck next time!";
        }

        else
        {
            annText.text = "Congratulations!\nYou survived the ghosts!";
        }

        killText.text = "<u>Enemies Killed:</u>\n" + enemiesKilled;
        timeText.text = "<u>Survived For:</u>\n" + formatTime(timeTaken);
    }

    private string formatTime(float t)
    {
        int min = (int) Mathf.FloorToInt(t / 60);
        int sec = (int) Mathf.FloorToInt(t % 60);
        string str1 = " minutes";
        string str2 = " seconds";

        if (min == 1) str1 = " minute";
        if (sec == 1) str2 = " second";

        return min + str1 + " and " + sec + str2;
    }

    public void restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Loading");
    }
}
