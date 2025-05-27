using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Thoughs;

public class EndingTrigger : MonoBehaviour
{
    [SerializeField] private Thoughs thoughs;
    [SerializeField] private int index;
    [SerializeField] private Image blackScreen;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<GameController>().canMove = false;
            other.GetComponent<CharacterController>().enabled = false;            
            thoughs.ShowThought(index);
            StartCoroutine(EndGame());
        }
    }

    private IEnumerator EndGame()
    {
        blackScreen.gameObject.SetActive(true);
        yield return new WaitForSeconds(thoughs.thoughts[index].duration/2);
        yield return StartCoroutine(FadeImageTo(1f, 4f));
        yield return new WaitForSeconds(3f);
        SceneManager.LoadSceneAsync("MainMenu");
    }

    IEnumerator FadeImageTo(float targetAlpha, float duration)
    {
        float startAlpha = blackScreen.color.a;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, timeElapsed / duration);
            SetImageTransparency(newAlpha);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        SetImageTransparency(targetAlpha);
    }

    private void SetImageTransparency(float alpha)
    {
        Color color = blackScreen.color;
        color.a = alpha;
        blackScreen.color = color;
    }
}
