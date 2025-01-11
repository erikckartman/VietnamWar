using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTE : MonoBehaviour
{
    [SerializeField] private Text qteText;
    [SerializeField] private Text timerText;

    private float qteDuration = 2f;
    private float currentTime;

    private bool qteActive = false;

    private int currentStep = 0;

    [SerializeField] private KeyCode[] qteSequence;

    private void Update()
    {
        if (qteActive)
        {
            currentTime -= Time.deltaTime;

            if(timerText != null)
            {
                timerText.text = "Time: " + Mathf.Round(currentTime).ToString();
            }

            if (Input.GetKeyDown(qteSequence[currentStep]))
            {
                currentStep++;
                currentTime = qteDuration;

                if (currentStep >= qteSequence.Length)
                {
                    qteActive = false;
                    SuccessQTE();
                }
                else
                {
                    qteText.text = qteSequence[currentStep].ToString();
                }
            }

            if (currentTime <= 0)
            {
                qteActive = false;
                FailQTE();
            }
        }
    }

    public void TurnUI(bool active)
    {
        timerText.GetComponent<Text>().enabled = active;
        qteText.GetComponent<Text>().enabled = active;
    }

    public void StartQTE()
    {
        TurnUI(true);
        qteSequence = new KeyCode[3] { RandomKey(), RandomKey(), RandomKey() };

        qteText.text = qteSequence[currentStep].ToString();

        currentTime = qteDuration;
        qteActive = true;
    }

    private KeyCode RandomKey()
    {
        int rand = Random.Range(0, 3);
        
        switch (rand)
        {
            case 0:
                return KeyCode.A;
            case 1:
                return KeyCode.S;
            case 2:
                return KeyCode.D;
            default:
                return KeyCode.Space;
        }
    }

    private void SuccessQTE()
    {
        TurnUI(false);
        Debug.Log("Success!");
    }

    private void FailQTE()
    {
        TurnUI(false);
        Debug.Log("Fail!");
    }
}
