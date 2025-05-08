using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateInteractParametres : MonoBehaviour
{
    private bool isUpdated = false;
    private ItemColliderWithPlayer itemColliderWithPlayer;
    private ChangeQuest changeQuest;
    private Teleport teleport;

    [SerializeField] private Items newRequiredItem;
    [SerializeField] private Transform newSpawnPos;
    [SerializeField] private string newTask;

    private void Start()
    {
        itemColliderWithPlayer = GetComponent<ItemColliderWithPlayer>();
        teleport = GetComponent<Teleport>();
    }

    public void ChangeVariables()
    {
        if (isUpdated) return;

        itemColliderWithPlayer.qteCompleted = false;

        if(newSpawnPos != null)
            teleport.nextPoint = newSpawnPos;

        if(newRequiredItem != null)
            itemColliderWithPlayer.requiredItem = newRequiredItem;
        
        if(newTask != null)
            changeQuest.ChangeTask(newTask);

        isUpdated = true;
    }
}