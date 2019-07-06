using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public Animator animator;
    public GameObject ExitDialogue;
    public Slider MusicSlider;
    public Slider EffectsSlider;
    public GameObject ClickSound;
    public GameObject TransitionSound;

    public void Start()
    {
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        EffectsSlider.value = PlayerPrefs.GetFloat("EffectsVolume");
    }
    public void Play()
    {
        FindObjectOfType<SaveDataPanel>().ShowPanel("", AddressInfo.recipient[1], AddressInfo.Asset);
    }
    public void PlayClick()
    {
        ClickSound.GetComponent<AudioSource>().Play();
    }
    public void PlayTransition()
    {
        TransitionSound.GetComponent<AudioSource>().Play();
    }
    public void ExitButton()
    {
        ExitDialogue.SetActive(true);
    }
    public void No()
    {
        ExitDialogue.SetActive(false);
    }
    public void Yes()
    {
        Application.Quit();
    }
    public void MainMenuToSettings()
    {
        animator.SetTrigger("MainMenuToOptions");
    }
    public void MainMenuToCharacter()
    {
        animator.SetTrigger("MainMenuToCharacter");
    }
    public void CharacterToMainMenu()
    {
        animator.SetTrigger("CharacterToMainMenu");
    }
    public void MainMenuToData()
    {
        animator.SetTrigger("MainMenuToData");
    }
    public void DataToMainMenu()
    {
        animator.SetTrigger("DataToMainMenu");
    }
    public void SettingsToMainMenu()
    {
        animator.SetTrigger("OptionsToMainMenu");
    }
}
