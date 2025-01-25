using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class QTE : MonoBehaviour
{
    [SerializeField] private Text qteText;

    private float qteDuration = 3f;
    private float currentTime;

    private bool qteActive = false;
    private bool inputLocked = false;

    private int currentStep = 0;

    [SerializeField] private KeyCode[] qteSequence;

    [HideInInspector]public UnityEvent OnQTESuccess;

    [SerializeField] private GameController controller;

    //Second QTE
    private bool rapidPressActive = false;
    private KeyCode rapidPressKey;
    private int requiredPresses = 10;
    private int currentPressCount = 0;
    private float rapidPressDuration = 2f;
    private float rapidPressTimer;


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

        if (rapidPressActive)
        {

            rapidPressTimer -= Time.deltaTime;
            if (Input.GetKeyDown(rapidPressKey))
            {
                TurnUI(false);
                currentPressCount++;
                qteText.text = rapidPressKey.ToString();
                Debug.Log($"{rapidPressKey} : {currentPressCount}/{requiredPresses}");
                TurnUI(true);

                if (currentPressCount >= requiredPresses)
                {
                    rapidPressActive = false;
                    SuccessQTE();
                }
            }       

            if (rapidPressTimer <= 0)
            {
                rapidPressActive = false;
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
        qteSequence = new KeyCode[4] { RandomKey(), RandomKey(), RandomKey(), RandomKey() };

        qteText.text = qteSequence[currentStep].ToString();
        controller.canMove = false;

        currentTime = qteDuration;
        qteActive = true;
        inputLocked = true;
    }

    public void StartQTE2()
    {
        TurnUI(true);
        rapidPressKey = RandomKey();
        currentPressCount = 0;
        qteText.text = rapidPressKey.ToString();
        Debug.Log($"{rapidPressKey} : {currentPressCount}/{requiredPresses}");
        controller.canMove = false;

        
        rapidPressTimer = rapidPressDuration;
        rapidPressActive = true;
        inputLocked = true;
    }

    private KeyCode RandomKey()
    {
        int rand = Random.Range(0, 6);
        
        switch (rand)
        {
            case 0:
                return KeyCode.A;
            case 1:
                return KeyCode.S;
            case 2:
                return KeyCode.D;
            case 3:
                return KeyCode.Z;
            case 4:
                return KeyCode.X;
            case 5:
                return KeyCode.C;
            default:
                return KeyCode.Space;
        }
    }

    private void SuccessQTE()
    {
        rapidPressActive = false;
        qteActive = false;
        currentStep = 0;
        currentPressCount = 0;
        Debug.Log("Success!");
        controller.canMove = true;
        OnQTESuccess?.Invoke();
        qteDuration -= 0.2f;
        TurnUI(false);
    }

    private void FailQTE()
    {
        rapidPressActive = false;
        qteActive = false;
        currentStep = 0;
        currentPressCount = 0;
        controller.canMove = true;
        Debug.Log("Fail!");
        TurnUI(false);
    }
}
