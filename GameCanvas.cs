using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
    [SerializeField] GameObject settingCanvas;
    [SerializeField] GameObject settingButton, closeButton;
    [SerializeField] Timer t;
    [SerializeField] PlayerControl pc;
    [SerializeField] Enemies e;

    [SerializeField] Slider slowSlider, freezeSlider, flySlider;

    void Start()
    {
        //Gets the slow slider
        Slider s = slowSlider.GetComponent<Slider>();

        //Gets the freeze slider
        Slider f = freezeSlider.GetComponent<Slider>();

        //Gets the fly slider
        Slider fl = freezeSlider.GetComponent<Slider>();

        initialize();
    }

    void Update()
    {
        slow();
        freeze();
        fly();
    }

    public void openSettings()
    {
        pauseGame();

        settingButton.SetActive(false);
        settingCanvas.SetActive(true);
        closeButton.SetActive(true);
        this.gameObject.SetActive(false);
        
    }

    public void resumeGame()
    {
        settingButton.SetActive(true);
        settingCanvas.SetActive(false);
        t.resume();
        pc.resume();
        e.resume();
    }

    public void pauseGame()
    {
        t.pause();
        pc.pause();
        e.pause();
    }

    public void slow()
    {
        float max = 5f;
        float num = e.slowTime;

        if (num == 0) slowSlider.value = num;
        else slowSlider.value = max - num;
    }

    public void freeze() 
    {
        float max = 5f;
        float num = e.freezeTime;

        if (num == 0) freezeSlider.value = num;
        else freezeSlider.value = max - num;
    }

    public void fly()
    {
        float max = 10f;
        float num = pc.flyTime;

        if (num == 0) flySlider.value = num;
        else flySlider.value = max - num;
    }

    private void initialize()
    {
        slowSlider.value = 0f;
        freezeSlider.value = 0f;
        flySlider.value = 0f;
    }
}
