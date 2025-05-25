using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateInteractParametres : MonoBehaviour
{
    public bool isUpdated = false;
    [SerializeField] private ItemColliderWithPlayer itemColliderWithPlayer;
    [SerializeField] private ChangeQuest changeQuest;
    [SerializeField] private Teleport teleport;

    [SerializeField] private Items newRequiredItem;
    [SerializeField] private string newTask;

    [Header("Map parts")]
    [SerializeField] private Transform moveToPart1;
    [SerializeField] private Transform moveToPart2;

    public int currentMapPart = 1; //This var has to be only 1 or 2

    public void ChangeVariables(bool withSwap)
    {
        if (withSwap)
        {
            CurrentPosSwap();
        }   
        
        SetTeleportPosition();

        itemColliderWithPlayer.qteCompleted = false;

        itemColliderWithPlayer.requiredItem = newRequiredItem;

        if (newTask != null && newTask != "")
        {
            changeQuest.ChangeTask(newTask);
        }

        isUpdated = true;
    }

    private void CurrentPosSwap()
    {
        if(currentMapPart == 1)
        {
            currentMapPart = 2;
        }
        else if(currentMapPart == 2)
        {
            currentMapPart = 1;
        }
    }

    public void SetTeleportPosition()
    {
        if(currentMapPart == 1)
        {
            teleport.nextPoint = moveToPart2;
            Debug.Log("Updated to moveToPart2");
        }
        else if(currentMapPart == 2)
        {
            teleport.nextPoint = moveToPart1;
            Debug.Log("Updated to moveToPart1");
        }
    }
}