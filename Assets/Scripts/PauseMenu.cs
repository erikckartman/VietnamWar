using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause menu pages")]
    [SerializeField] private GameObject pause;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject menu;

    [Header("Sliders")]
    [SerializeField] private Slider master;
    [SerializeField] private Slider sfx;
    [SerializeField] private Slider music;

    private bool isPause = false;
    [Header("Other elements")]
    [SerializeField] private GameController gameController;
    [SerializeField] private AudioMixer audioMixer;

    private void Start()
    {
        master.value = PlayerPrefs.GetFloat("MasterVolume", 0f);
        sfx.value = PlayerPrefs.GetFloat("SFXVolume", 0f);
        music.value = PlayerPrefs.GetFloat("MusicVolume", 0f);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isPause = !isPause;

            menu.SetActive(isPause);
            Cursor.visible = isPause;
            
            gameController.canMove = !isPause;
        }

        if (isPause)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void CloseMenu()
    {
        isPause = false;
        menu.SetActive(isPause);

        Cursor.visible = isPause;
        Cursor.lockState = CursorLockMode.Locked;

        gameController.canMove = !isPause;
    }

    public void ExitToMenu()
    {
        PlayerPrefs.SetFloat("MasterVolume", master.value);
        PlayerPrefs.SetFloat("SFXVolume", sfx.value);
        PlayerPrefs.SetFloat("MusicVolume", music.value);
    }

    public void OpenOptions()
    {
        pause.SetActive(false);
        options.SetActive(true);
    }

    public void CloseOptions()
    {
        options.SetActive(false);
        pause.SetActive(true);
    }

    public void MasterVolume()
    {
        audioMixer.SetFloat("Masterexpose", master.value);
    }

    public void SFXVolume()
    {
        audioMixer.SetFloat("SFXexpose", sfx.value);
    }

    public void MusicVolume()
    {
        audioMixer.SetFloat("Musicexpose", music.value);
    }
}
