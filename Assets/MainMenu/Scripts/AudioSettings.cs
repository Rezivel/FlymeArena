using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour {

    public void Start()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("EffectsVolume");
        }
    }

    public void UpdateMusicVolume()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
    }
    public void UpdateEffectsVolume()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("EffectsVolume");
    }

    public void MusicSlider(Slider slider)
    {
        PlayerPrefs.SetFloat("MusicVolume", slider.value);
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
    }
    public void EffectsSlider(Slider slider)
    {
        PlayerPrefs.SetFloat("EffectsVolume", slider.value);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("EffectsVolume");
        }
    }
}
