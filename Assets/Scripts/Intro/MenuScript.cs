using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    [Header("Pause menu pages")]
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject load;

    [Header("Sliders")]
    [SerializeField] private Slider master;
    [SerializeField] private Slider sfx;
    [SerializeField] private Slider music;

    [SerializeField] private AudioMixer audioMixer;
    private void Start()
    {
        master.value = PlayerPrefs.GetFloat("MasterVolume", 0f);
        sfx.value = PlayerPrefs.GetFloat("SFXVolume", 0f);
        music.value = PlayerPrefs.GetFloat("MusicVolume", 0f);
    }

    public void StartGame(bool loading)
    {
        load.SetActive(true);

        PlayerPrefs.SetInt("LoadGame", loading ? 1 : 0);

        PlayerPrefs.SetFloat("MasterVolume", master.value);
        PlayerPrefs.SetFloat("SFXVolume", sfx.value);
        PlayerPrefs.SetFloat("MusicVolume", music.value);

        SceneManager.LoadScene("SampleScene");
    }

    public void OpenOptions()
    {
        menu.SetActive(false);
        options.SetActive(true);
    }

    public void CloseOptions()
    {
        options.SetActive(false);
        menu.SetActive(true);
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
