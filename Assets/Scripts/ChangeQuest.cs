using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeQuest : MonoBehaviour
{ 
    public GameObject questList;
    public string currentTask;
    public Text textQuest;
    [SerializeField] private Thoughs thoughtsScript;

    [SerializeField] private Transform player;
    [HideInInspector] public bool pharmacyTaskDone = false;


    public void ChangeTask(string taskToWrite)
    {
        if(taskToWrite == "Explore the Pharmacy")
        {
            if (pharmacyTaskDone) return;

            currentTask = taskToWrite;
            textQuest.text = currentTask;

            pharmacyTaskDone = true;
        }
        else
        {
            currentTask = taskToWrite;
            textQuest.text = currentTask;
        }
    }

    public void EnableTask(ItemColliderWithPlayer script)
    {
        script.enabled = true;
    }
}
