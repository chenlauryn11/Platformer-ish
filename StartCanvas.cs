using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCanvas : MonoBehaviour
{
    [SerializeField] GameObject startCanvas;
    [SerializeField] GameObject instructionCanvas;

    // Start is called before the first frame update
    void Start()
    {
        startCanvas.SetActive(true);
        instructionCanvas.SetActive(false);
    }

    public void openInstructions()
    {
        startCanvas.SetActive(false);
        instructionCanvas.SetActive(true);
    }

    public void closeInstructions()
    {
        instructionCanvas.SetActive(false);
        startCanvas.SetActive(true);
    }

    public void openGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
}
