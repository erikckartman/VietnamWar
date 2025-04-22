using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

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

    public bool isPause = false;
    [Header("Other elements")]
    [SerializeField] private GameController gameController;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private InventoryUI inventoryUI;

    private void Start()
    {
        master.value = PlayerPrefs.GetFloat("MasterVolume", 0f);
        sfx.value = PlayerPrefs.GetFloat("SFXVolume", 0f);
        music.value = PlayerPrefs.GetFloat("MusicVolume", 0f);
    }

    private void Update()
    {

        if (gameController.canMove && !inventoryUI.openInventor)
        {
            if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
            {
                isPause = pause.activeSelf;

                gameController.canMove = !isPause;
                menu.SetActive(isPause);
                Cursor.visible = isPause;
                Cursor.lockState = isPause ? CursorLockMode.None : CursorLockMode.Locked;
            }
            
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log($"Is pause = {isPause}, can move = {gameController.canMove}");

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

        SceneManager.LoadScene("MainMenu");
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
