using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeQuest : MonoBehaviour
{ 
    [SerializeField] private GameObject questList;
    [SerializeField] private Text textQuest;
    [SerializeField] private GameObject mainObject;

    [SerializeField] private Transform player;
    private void Start()
    {
        StartCoroutine(TaskOne());
    }

    private void Update()
    {
        float distance = (player.position - mainObject.transform.position).magnitude;

        if(distance < 10f)
        {
            questList.SetActive(false);
        }
        else
        {
            questList.SetActive(true);
        }
    }

    public void ChangeTask(string currentTask)
    {
        textQuest.text = currentTask;
    }

    public void ChangeCount(GameObject currentObject)
    {
        mainObject = currentObject;
    }

    private IEnumerator TaskOne()
    {
        yield return new WaitForSeconds(15);

        textQuest.text = "Explore the house";
        questList.SetActive(true);
    }
}
