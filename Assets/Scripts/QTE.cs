using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QTE : MonoBehaviour
{
    [SerializeField] private Text qteText;

    private float qteDuration = 2f;
    private float currentTime;

    private bool qteActive = false;
    private bool inputLocked = false;

    private int currentStep = 0;

    [SerializeField] private KeyCode[] qteSequence;

    [HideInInspector]public UnityEvent OnQTESuccess;

    private void Update()
    {
        if (qteActive)
        {
            if (inputLocked)
            {
                inputLocked = false;
                return;
            }

            currentTime -= Time.deltaTime;

            if (Input.anyKeyDown)
            {
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
                else
                {
                    qteActive = false;
                    FailQTE();
                }
            }            

            if (currentTime < 0)
            {
                qteActive = false;
                FailQTE();
            }
        }
    }

    public void TurnUI(bool active)
    {
        qteText.GetComponent<Text>().enabled = active;
    }

    public void StartQTE()
    {
        TurnUI(true);
        qteSequence = new KeyCode[3] { RandomKey(), RandomKey(), RandomKey() };

        qteText.text = qteSequence[currentStep].ToString();

        currentTime = qteDuration;
        qteActive = true;
        inputLocked = true;
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
        currentStep = 0;
        TurnUI(false);
        Debug.Log("Success!");
        OnQTESuccess?.Invoke();
    }

    private void FailQTE()
    {
        currentStep = 0;
        TurnUI(false);
        Debug.Log("Fail!");
    }
}
