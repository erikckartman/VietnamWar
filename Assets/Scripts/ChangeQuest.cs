using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
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
    [SerializeField] private Text textQuest;
    [SerializeField] private GameObject mainObject;
    [SerializeField] private Thoughs thoughtsScript;

    [SerializeField] private Transform player;
    [SerializeField] private List<InteractiveObject> interactiveObjects = new List<InteractiveObject>();
    private bool canStart = false;
    private bool messageShown = false;

    private void Start()
    {
        StartCoroutine(TaskOne());
    }

    private void Update()
    {
        if (canStart)
        {
            float distance = (player.position - mainObject.transform.position).magnitude;

            if (distance < 13f)
            {
                questList.SetActive(false);
            }
            else
            {
                questList.SetActive(true);
            }

            if(distance <= 2f)
            {
                DisplayMessage();
            }
        }
    }

    public void ChangeTask(string currentTask)
    {
        textQuest.text = currentTask;
    }

    public void EnableTask(ItemColliderWithPlayer script)
    {
        script.enabled = true;
    }

    public void ChangeCount(GameObject currentObject)
    {
        mainObject = currentObject;
    }

    private IEnumerator TaskOne()
    {
        yield return new WaitForSeconds(13);

        textQuest.text = "Explore the house";
        questList.SetActive(true);
        canStart = true;
    }

    private void DisplayMessage()
    {
        InteractiveObject foundObject = interactiveObjects.Find(obj => obj.obj == mainObject);

        if (foundObject != null)
        {
            thoughtsScript.ShowThought(foundObject.messageIndex);
        }
    }
}
