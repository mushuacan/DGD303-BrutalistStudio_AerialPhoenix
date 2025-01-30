using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options_Menu : MonoBehaviour
{
    public Slider sliderSFX;
    public Slider sliderMusic;
    public PlayerPrefers playerPrefers;
    public GameObject imageOfSoundsOn;
    public GameObject imageOfSoundOff;
    public bool escMenuOpen;
    public GameObject escMenuGO;
    public bool escapeMenuAbleity;

    void Start()
    {
        ResetSettings();
    }

    private void ResetSettings()
    {
        ArrangeSlidersAtStart();
        ArrangeSoundsImagesAtStart();
    }

    public void ArrangeMusicSlider()
    {
        playerPrefers.ArrangeMusic(sliderMusic.value);
    }
    public void ArrangeSFXSlider()
    {
        playerPrefers.ArrangeSFX(sliderSFX.value);
    }
    private void ArrangeSlidersAtStart()
    {
        sliderMusic.value = PlayerPrefs.GetFloat("MusicVolume");
        sliderSFX.value = PlayerPrefs.GetFloat("SFXVolume");
    }
    public void ArrangeButtonOfSounds()
    {
        string allSounds = PlayerPrefs.GetString("AllSounds");

        if (allSounds == "Yes")
        {
            allSounds = "No";
            PlayerPrefs.SetString("AllSounds", allSounds);
            playerPrefers.ArrangeAllSounds(allSounds);
            ArrangeSoundsImages(false);
        }
        else if (allSounds == "No")
        {
            allSounds = "Yes";
            PlayerPrefs.SetString("AllSounds", allSounds);
            playerPrefers.ArrangeAllSounds(allSounds);
            ArrangeSoundsImages(true);
        }
        else
        {
            allSounds = "Yes";
            PlayerPrefs.SetString("AllSounds", allSounds);
        }

        Debug.Log("Buton çalýþtý. allsounds -> " + allSounds);
    }
    private void ArrangeSoundsImages(bool isItOn)
    {
        if (isItOn)
        {
            imageOfSoundOff.SetActive(false);
            imageOfSoundsOn.SetActive(true);
        }
        else if (!isItOn)
        {
            imageOfSoundOff.SetActive(true);
            imageOfSoundsOn.SetActive(false);
        }
    }
    private void ArrangeSoundsImagesAtStart()
    {
        string allSounds = PlayerPrefs.GetString("AllSounds");

        if (allSounds == "Yes")
        {
            ArrangeSoundsImages(true);
        }
        else if (allSounds == "No")
        {
            ArrangeSoundsImages(false);
        }
    }
}
