using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Progress;

public class ItemColliderWithPlayer : MonoBehaviour
{
    [SerializeField] private QTE qte;
    [SerializeField] private Items requiredItem;
    [SerializeField]
    private UnityEvent onQTEcomplete;
    private bool qteCompleted;

    
    private void Update()
    {
        if (qteCompleted) return;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3f);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
            {
                if (collider.GetComponent<Inventory>().activeItem == requiredItem)
                {
                    qte.OnQTESuccess.AddListener(HandleQteSuccess);
                    qte.StartQTE();
                }
                else
                {
                    Debug.Log($"You have to choose {requiredItem.itemName}");
                }
            }
        }


    }

    private void HandleQteSuccess()
    {
        onQTEcomplete.Invoke();
        qteCompleted = true;
    }
}
