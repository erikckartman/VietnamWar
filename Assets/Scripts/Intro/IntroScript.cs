using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class IntroScript : MonoBehaviour
{
    [SerializeField] private GameController controller;
    [SerializeField] private ChangeQuest changeQuest;
    [SerializeField] private GameObject intro;
    [SerializeField] private Thoughs thoughs;
    [SerializeField] private ProgressSaveSystem progressSaveSystem;

    [Header("Intro elements")]
    [SerializeField] private Image image;
    [SerializeField] private AudioSource audioSource1;
    [SerializeField] private AudioSource audioSource2;
    [SerializeField] private Text mainText;
    private float fadeDuration = 5f;

    private void Start()
    {
        bool isItLoads = PlayerPrefs.GetInt("LoadGame", 0) == 1;

        if (isItLoads)
        {
            progressSaveSystem.LoadProgress();

            if (progressSaveSystem.currentProgress > 0)
            {
                ContinueGame();
            }
            else
            {
                StartCoroutine(GoToGame());
            }
        }
        else
        {
            StartCoroutine(GoToGame());
        }
    }

    private IEnumerator FadeText(Text text, string newText, float fadeInDuration, float fadeOutDuration, float waitBeforeFadeIn)
    {
        if (progressSaveSystem.currentProgress > 0)
        {
            ContinueGame();
            yield break;
        }

        if (text == null) yield break;

        text.text = newText;

        float elapsedTime = 0f;
        Color originalColor = text.color;
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        text.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);

        yield return new WaitForSeconds(waitBeforeFadeIn);


        elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }
        text.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }

    private IEnumerator GoToGame()
    {
        yield return StartCoroutine(FadeText(mainText, "Vietnam\n1969", 3f, 3f, 2f));

        yield return new WaitForSeconds(1f);
        mainText.resizeTextForBestFit = true;

        yield return StartCoroutine(FadeText(mainText, "During the most tragic years of the Vietnam War, countless towns across the country were devastated by relentless bombing campaigns. Innocent civilians were drawn into the conflict, losing their families, homes, and everything they held dear...", 3f, 3f, 6f));

        float elapsedTime = 0f;
        Color originalColor = image.color;

        if(audioSource1 == null || audioSource2 == null) yield break;

        float originalVolume1 = audioSource1.volume;
        float originalVolume2 = audioSource2.volume;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            float alpha = Mathf.Lerp(originalColor.a, 0f, elapsedTime / fadeDuration);
            float volume1 = Mathf.Lerp(originalVolume1, 0f, elapsedTime / fadeDuration);
            float volume2 = Mathf.Lerp(originalVolume2, 0f, elapsedTime / fadeDuration);

            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            audioSource1.volume = volume1;
            audioSource2.volume = volume2;

            yield return null;
        }

        image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        audioSource1.volume = 0f;
        audioSource2.volume = 0f;

        controller.canMove = true;
        Destroy(mainText.gameObject);
        intro.SetActive(false);

        progressSaveSystem.UpdateProgress();
        thoughs.ShowThought(0);

        changeQuest.textQuest.text = "Explore the house with the yard";
        changeQuest.questList.SetActive(true);
        yield return new WaitForSeconds(3);

        thoughs.ShowThought(5);
    }

    private void ContinueGame()
    {
        Color color = intro.GetComponent<Image>().color;
        color.a = 0f;
        intro.GetComponent<Image>().color = color;

        Destroy(mainText.gameObject);
        intro.SetActive(false);

        controller.canMove = true;
        changeQuest.questList.SetActive(true);
    }
}
