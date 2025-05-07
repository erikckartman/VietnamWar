using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ItemColliderWithPlayer : MonoBehaviour
{
    private enum QTEmode {
        QTEstandart,
        QTErapidpress
    }

    [Header("Progress items")]
    [SerializeField] private ProgressSaveSystem progressSaveSystem;
    [SerializeField] private int requiredProgress;

    [Header("Other")]
    [SerializeField] private QTEmode currentQTE;

    [SerializeField] private QTE qte;
    [SerializeField] private ChangeQuest changeQuest;
    [SerializeField] private Items requiredItem;
    [SerializeField] private UnityEvent onQTEcomplete;
    [SerializeField] private UnityEvent onDontHavingItem;
    [SerializeField] private GameController gameController;
    public bool qteCompleted;

    [SerializeField] private float distCol;
    [SerializeField] private Text alert;
    [SerializeField] private GameObject alertGO;

    [SerializeField] private bool showAlert;

    private void Start()
    {
        if(progressSaveSystem != null)
        {
            if(progressSaveSystem.currentProgress > requiredProgress)
            {
                qteCompleted = true;
            }
        }
    }

    private void Update()
    {
        if (qteCompleted || !gameController.canMove) return;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, distCol);
        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
            {
                if (requiredItem == null)
                {
                    gameObject.tag = "Untagged";
                    qte.OnQTESuccess.RemoveAllListeners();
                    qte.OnQTESuccess.AddListener(HandleQteSuccess);

                    if (currentQTE == QTEmode.QTEstandart)
                    {
                        qte.StartQTE();
                    }
                    else if (currentQTE == QTEmode.QTErapidpress)
                    {
                        qte.StartQTE2();
                    }
                }
                else
                {
                    if (collider.GetComponent<Inventory>().activeItem == requiredItem)
                    {
                        gameObject.tag = "Untagged";
                        StopCoroutine(WaitToEnd());
                        alertGO.SetActive(false);
                        qte.OnQTESuccess.RemoveAllListeners(); 
                        qte.OnQTESuccess.AddListener(HandleQteSuccess);
                        
                        if(currentQTE == QTEmode.QTEstandart)
                        {
                            qte.StartQTE();
                        }
                        else if(currentQTE == QTEmode.QTErapidpress)
                        {
                            qte.StartQTE2();
                        }
                    }
                    else
                    {
                        Debug.Log($"You have to choose {requiredItem.itemName}");
                        if (showAlert)
                        {
                            alert.text = "You have to choose " + requiredItem.itemName;
                        }
                        
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
