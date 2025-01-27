using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class IntroScript : MonoBehaviour
{
    [SerializeField] private GameController controller;
    [SerializeField] private GameObject intro;
    [SerializeField] private Thoughs thoughs;

    [Header("Intro elements")]
    [SerializeField] private Image image;
    [SerializeField] private AudioSource audioSource1;
    [SerializeField] private AudioSource audioSource2;
    [SerializeField] private Text mainText;
    private float fadeDuration = 5f;

    private void Start()
    {
        StartCoroutine(GoToGame());
    }

    private IEnumerator GoToGame()
    {
        if (mainText != null)
        {
            float elapsedTime1 = 0f;
            Color originalTextColor = mainText.color;

            while (elapsedTime1 < 5f)
            {
                elapsedTime1 += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, elapsedTime1 / 5f);
                mainText.color = new Color(originalTextColor.r, originalTextColor.g, originalTextColor.b, alpha);
                yield return null;
            }

            mainText.color = new Color(originalTextColor.r, originalTextColor.g, originalTextColor.b, 1f);
        }

        float elapsedTime = 0f;
        Color originalColor = image.color;
        float originalVolume1 = audioSource1.volume;
        float originalVolume2 = audioSource2.volume;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            float alpha = Mathf.Lerp(originalColor.a, 0f, elapsedTime / fadeDuration);
            Color originalTextColorFinal = mainText != null ? mainText.color : Color.clear;
            float volume1 = Mathf.Lerp(originalVolume1, 0f, elapsedTime / fadeDuration);
            float volume2 = Mathf.Lerp(originalVolume2, 0f, elapsedTime / fadeDuration);

            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            mainText.color = new Color(originalTextColorFinal.r, originalTextColorFinal.g, originalTextColorFinal.b, alpha);

            audioSource1.volume = volume1;
            audioSource2.volume = volume2;

            yield return null;
        }

        image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        audioSource1.volume = 0f;
        audioSource2.volume = 0f;

        controller.canMove = true;
        Destroy(intro);
        
        thoughs.ShowThought(0);
        yield return new WaitForSeconds(3);

        thoughs.ShowThought(5);
    }
}
