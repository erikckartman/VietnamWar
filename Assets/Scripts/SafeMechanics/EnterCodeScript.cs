using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnterCodeScript : MonoBehaviour
{
    [SerializeField] private InputField codeField;
    [SerializeField] private string inputCode;
    public string requiredCode;
    [SerializeField] private GameObject enterCodeUI;
    [SerializeField] private GameController gameController;
    [HideInInspector] public UnityEvent CodeSucces;

    public void EnterSymbol(string num)
    {
        if (inputCode.Length < requiredCode.Length)
        {
            inputCode += num;
            UpdateDisplay();
        }
    }

    private void Update()
    {
        if(!enterCodeUI.activeSelf) return;

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            CodePanelVisibility(false);
        }
    }

    public void DeleteLastDigit()
    {
        if (inputCode.Length > 0)
        {
            inputCode = inputCode.Substring(0, inputCode.Length - 1);
            UpdateDisplay();
        }
    }

    public void EnterCode()
    {
        if (inputCode == requiredCode)
        {
            Debug.Log("Code is right");
            inputCode = null;
            UpdateDisplay();
            CodePanelVisibility(false);
            CodeSucces?.Invoke();
        }
        else
        {
            inputCode = null;
            codeField.text = "ERR";
        }
    }

    private void UpdateDisplay()
    {
        codeField.text = inputCode;
    }

    public void CodePanelVisibility(bool isActive)
    {
        enterCodeUI.SetActive(isActive);
        UpdateDisplay();
        Cursor.visible = isActive;
        gameController.canMove = !isActive;
        if (isActive)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
