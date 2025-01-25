using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ItemColliderWithPlayer : MonoBehaviour
{
    [SerializeField] private QTE qte;
    [SerializeField] private Items requiredItem;
    [SerializeField] private UnityEvent onQTEcomplete;
    [SerializeField] private UnityEvent onDontHavingItem;
    [SerializeField] private bool qteCompleted;

    [SerializeField] private float distCol;
    [SerializeField] private Text alert;
    [SerializeField] private GameObject alertGO;
    private void Update()
    {
        if (qteCompleted) return;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, distCol);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
            {
                if (requiredItem == null)
                {
                    qte.OnQTESuccess.RemoveAllListeners();
                    qte.OnQTESuccess.AddListener(HandleQteSuccess);
                    qte.StartQTE();
                }
                else
                {
                    if (collider.GetComponent<Inventory>().activeItem == requiredItem)
                    {
                        StopCoroutine(WaitToEnd());
                        alertGO.SetActive(false);
                        qte.OnQTESuccess.RemoveAllListeners(); 
                        qte.OnQTESuccess.AddListener(HandleQteSuccess);
                        qte.StartQTE();
                    }
                    else
                    {
                        Debug.Log($"You have to choose {requiredItem.itemName}");
                        alert.text = "You have to choose " + requiredItem.itemName;
                        if(onDontHavingItem != null)
                        {
                            onDontHavingItem.Invoke();
                        }
                        alertGO.SetActive(true);
                        StartCoroutine(WaitToEnd());
                    }
                }
            }
        }
    }

    private IEnumerator WaitToEnd()
    {
        yield return new WaitForSeconds(1);
        alertGO.SetActive(false);
    }

    private void HandleQteSuccess()
    {
        qte.OnQTESuccess.RemoveListener(HandleQteSuccess);
        onQTEcomplete.Invoke();
        qteCompleted = true;
    }
}
