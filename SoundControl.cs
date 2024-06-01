using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    //Initializes the sliders to control volume
    [SerializeField] Slider musicSlider, sfxSlider;

    //Initializes the audio sources
    [SerializeField] AudioSource musicAudio, collPotionAudio, killGhostAudio, loseGameAudio, winGameAudio, loseHealthAudio, gainHealthAudio;

    void Awake()
    {
        //Gets the music slider
        Slider m = musicSlider.GetComponent<Slider>();

        //Gets the sound effects slider
        Slider s = sfxSlider.GetComponent<Slider>();

        //Sets the volume of the music
        initialize();
    }

    //Start is called before the first frame update
    void Start()
    {

    }

    //Plays correct answer sfx
    public void playCollPotion()
    {
        collPotionAudio.Play();
    }

    //Plays incorrect answer sfx
    public void playKillGhost()
    {
        killGhostAudio.Play();
    }

    //Plays win game sfx
    public void playWin()
    {
        winGameAudio.Play();
    }

    //Plays lose game sfx
    public void playLose()
    {
        loseGameAudio.Play();
    }

    //Plays lose health sfx
    public void playLoseHealth()
    {
        loseHealthAudio.Play();
    }

    //Plays gain health sfx
    public void playGainHealth()
    {
        gainHealthAudio.Play();
    }

    //Change volume of music using value of musicSlider
    public void changeMusicVol()
    {
        musicAudio.volume = musicSlider.value;
        Save();
    }

    //Change volume of sound effects using value of sfxSlider
    public void changeSFXVol()
    {
        collPotionAudio.volume = sfxSlider.value;
        killGhostAudio.volume = sfxSlider.value;
        loseGameAudio.volume = sfxSlider.value;
        winGameAudio.volume = sfxSlider.value;
        loseHealthAudio.volume = sfxSlider.value;
        gainHealthAudio.volume = sfxSlider.value;
        Save();
    }

    //Mute the music
    public void muteMusic()
    {
        musicAudio.volume = 0;
        musicSlider.value = 0;
        Save();
    }

    //Make the music max volume
    public void maxMusic()
    {
        musicAudio.volume = 1;
        musicSlider.value = 1;
        Save();
    }

    //Mute the sound effects
    public void muteSFX()
    {
        collPotionAudio.volume = 0;
        killGhostAudio.volume = 0;
        loseGameAudio.volume = 0;
        winGameAudio.volume = 0;
        loseHealthAudio.volume = 0;
        gainHealthAudio.volume = 0;
        sfxSlider.value = 0;
        Save();
    }

    //Make the sound effects max volume
    public void maxSFX()
    {
        collPotionAudio.volume = 1;
        killGhostAudio.volume = 1;
        loseGameAudio.volume = 1;
        winGameAudio.volume = 1;
        loseHealthAudio.volume = 1;
        gainHealthAudio.volume = 1;
        sfxSlider.value = 1;
        Save();
    }

    public void maxSounds()
    {
        maxMusic();
        maxSFX();
    }

    public void muteSounds()
    {
        muteMusic();
        muteSFX();
    }

    //Mute both music and sound effects and doesn't save values
    public void mute()
    {
        musicAudio.volume = 0;
        collPotionAudio.volume = 0;
        killGhostAudio.volume = 0;
        loseGameAudio.volume = 0;
        winGameAudio.volume = 0;
        loseHealthAudio.volume = 0;
        gainHealthAudio.volume = 0;
    }

    //Mute background music don't save values
    public void endGame()
    {
        musicAudio.volume = 0;
    }

    //Sets music and sound effects to its maximum volume
    public void initialize()
    {
        //If player prefs doesn't have a value for music volume ...
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            //Set volume to max
            PlayerPrefs.SetFloat("musicVolume", 1);
        }

        //If player prefs doesn't have a value for sfx volume ...
        if (!PlayerPrefs.HasKey("sfxVolume"))
        {
            //Set volume to max
            PlayerPrefs.SetFloat("sfxVolume", 1);
        }

        //Sets volume to player prefs value
        Load();

        //Sets the slider values to the audio volume
        musicSlider.value = musicAudio.volume;
        sfxSlider.value = collPotionAudio.volume;
        sfxSlider.value = killGhostAudio.volume;
        sfxSlider.value = loseGameAudio.volume;
        sfxSlider.value = winGameAudio.volume;
        sfxSlider.value = loseHealthAudio.volume;
        sfxSlider.value = gainHealthAudio.volume;
    }

    //Sets the volume to the value of the player preferences
    private void Load()
    {
        musicAudio.volume = PlayerPrefs.GetFloat("musicVolume");
        collPotionAudio.volume = PlayerPrefs.GetFloat("sfxVolume");
        killGhostAudio.volume = PlayerPrefs.GetFloat("sfxVolume");
        loseGameAudio.volume = PlayerPrefs.GetFloat("sfxVolume");
        winGameAudio.volume = PlayerPrefs.GetFloat("sfxVolume");
        loseHealthAudio.volume = PlayerPrefs.GetFloat("sfxVolume");
        gainHealthAudio.volume = PlayerPrefs.GetFloat("sfxVolume");
    }

    //Saves the volume into player prefs
    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", musicAudio.volume);
        PlayerPrefs.SetFloat("sfxVolume", collPotionAudio.volume);
    }

    //Restarts the volume settings
    public void restart()
    {
        initialize();
    }
}