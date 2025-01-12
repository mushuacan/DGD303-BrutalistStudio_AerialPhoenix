using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class PlayerPrefers : MonoBehaviour
{
    public AudioMixer audioMixer; // Audio Mixer 

    // Start is called before the first frame update
    void Start()
    {
        ArrangeSFX(PlayerPrefs.GetFloat("SFXVolume"));
        ArrangeMusic(PlayerPrefs.GetFloat("MusicVolume"));
        ArrangeAllSounds(PlayerPrefs.GetString("AllSounds"));
    }

    public void ArrangeSFX(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", volume);

        float db = Mathf.Log10(volume) * 20f;

        audioMixer.SetFloat("SFXVolume", db);

        //Debug.Log("SFXVOLUME DEÐÝÞTÝRÝLDÝ: volume " + volume + ", db " + db);
    }

    public void ArrangeMusic(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);

        float db = Mathf.Log10(volume) * 20f;

        audioMixer.SetFloat("MusicVolume", db);
    }

    public void ArrangeAllSounds(string soundsOn)
    {
        PlayerPrefs.SetString("AllSounds", soundsOn);

        if (soundsOn == "Yes")
        {
            audioMixer.SetFloat("MasterVolume", 0);
        }
        else if (soundsOn == "No")
        {
            audioMixer.SetFloat("MasterVolume", -80);
        }
        float currentVolume;
        audioMixer.GetFloat("MasterVolume", out currentVolume);
        Debug.Log("TÜM SESLER DEÐÝÞTÝRÝLDÝ: soundsOn " + soundsOn + ", db " + currentVolume);
    }
}
