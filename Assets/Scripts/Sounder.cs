using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sounder : MonoBehaviour
{
    [SerializeField] private AudioSource m_AudioSource;
    public void PlaySound(AudioSource source)
    {
        source.Play();
    }

    public void PlayClip(AudioClip clip)
    {
        if(m_AudioSource != null)
        {
            m_AudioSource.clip = clip;
            m_AudioSource.Play();
        }
    }

    public void GoToScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
