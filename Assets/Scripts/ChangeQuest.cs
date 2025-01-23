using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeQuest : MonoBehaviour
{
    [SerializeField] private GameObject questList;
    [SerializeField] private Text textQuest;

    private void Start()
    {
        StartCoroutine(TaskOne());
    }

    public void ChangeTask(string currentTask)
    {
        textQuest.text = currentTask;       
    }

    private IEnumerator TaskOne()
    {
        yield return new WaitForSeconds(15);

        textQuest.text = "Explore the house";
        questList.SetActive(true);
    }
}
