using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InteractiveObject
{
    public GameObject obj;
    public int messageIndex;
}
public class ChangeQuest : MonoBehaviour
{ 
    [SerializeField] private GameObject questList;
    public Text textQuest;
    [SerializeField] private Thoughs thoughtsScript;

    [SerializeField] private Transform player;
    [SerializeField] private List<InteractiveObject> interactiveObjects = new List<InteractiveObject>();
    private bool canStart = false;
    private bool messageShown = false;


    private void Start()
    {
        StartCoroutine(TaskOne());
        questList.SetActive(true);
    }

    public void ChangeTask(string currentTask)
    {
        textQuest.text = currentTask;
    }

    public void EnableTask(ItemColliderWithPlayer script)
    {
        script.enabled = true;
    }


    private IEnumerator TaskOne()
    {
        yield return new WaitForSeconds(13);

        textQuest.text = "Explore the house";
        questList.SetActive(true);
        canStart = true;
    }

}
