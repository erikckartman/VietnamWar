using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SafeInteractions : MonoBehaviour
{
    [SerializeField] private string code;
    public bool onInteractionCompleted = false;
    [SerializeField] private UnityEvent onQTEcomplete;
    [SerializeField] private float distCol;
    [SerializeField] private EnterCodeScript enterCodeScript;

    private void Update()
    {
        if (onInteractionCompleted) return;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, distCol);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
            {
                enterCodeScript.requiredCode = code;
                enterCodeScript.CodePanelVisibility(true);
                enterCodeScript.CodeSucces.AddListener(HandleQteSuccess);
            }
        }
    }

    private void HandleQteSuccess()
    {
        enterCodeScript.CodeSucces.RemoveListener(HandleQteSuccess);
        onQTEcomplete.Invoke();
        onInteractionCompleted = true;
    }
}