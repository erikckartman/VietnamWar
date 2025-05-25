using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform player;
    public Transform nextPoint;
    [SerializeField] private Image blackScreen;
    
    [SerializeField] private UpdateInteractParametres updateInteractParametres;
    private bool isGoing = false;

    public void GoThroughCar()
    {
        if(isGoing) return;
        StartCoroutine(TeleportPlayer());
    }

    private IEnumerator TeleportPlayer()
    {
        isGoing = true;
        player.gameObject.GetComponent<GameController>().canMove = false;
        blackScreen.gameObject.SetActive(true);
        yield return StartCoroutine(FadeImageTo(1f, 2f));

        yield return new WaitForSeconds(1f);
        player.position = nextPoint.position;

        yield return StartCoroutine(FadeImageTo(0f, 2f));
        player.gameObject.GetComponent<GameController>().canMove = true;
        blackScreen.gameObject.SetActive(false);
                
        updateInteractParametres.ChangeVariables(true);
        isGoing = false;
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
