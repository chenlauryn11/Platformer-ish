using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingCanvas : MonoBehaviour
{
    [SerializeField] GameCanvas gc;
    [SerializeField] GameObject gameCanvas;
    [SerializeField] GameObject closeButton;

    public void closeSettings()
    {
        closeButton.SetActive(false);
        gameCanvas.SetActive(true);
        gc.resumeGame();
    }

}
