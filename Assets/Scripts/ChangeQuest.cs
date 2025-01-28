using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeQuest : MonoBehaviour
{ 
    public GameObject questList;
    public Text textQuest;
    [SerializeField] private Thoughs thoughtsScript;

    [SerializeField] private Transform player;
    private bool canStart = false;
    private bool messageShown = false;

    public void ChangeTask(string currentTask)
    {
        textQuest.text = currentTask;
    }

    public void EnableTask(ItemColliderWithPlayer script)
    {
        script.enabled = true;
    }
}
