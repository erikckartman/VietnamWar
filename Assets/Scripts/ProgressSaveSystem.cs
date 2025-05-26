using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Linq;
using Unity.VisualScripting;

public class SaveData
{
    public int progress;
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public string taskToSave;
    public List<int> inventoryItems = new List<int>();
    public List<bool> completedTasks = new List<bool>();
    public List<bool> completedTasks2 = new List<bool>();
    public bool updatedTask;
    public List<OnSceneObject> sceneObjects = new List<OnSceneObject>();
    public int mapPartIndex;
}

[System.Serializable]
public class OnSceneObject
{
    public int itemID;
    public Vector3 position;
}

public class ProgressSaveSystem : MonoBehaviour
{
    public static ProgressSaveSystem instance;

    [SerializeField] private ChangeQuest changeQuest;
    [SerializeField] private List<Items> allItems = new List<Items>();
    [SerializeField] private List<ItemColliderWithPlayer> allInteracts = new List<ItemColliderWithPlayer>();
    [SerializeField] private List<SafeInteractions> allInteracts2 = new List<SafeInteractions>();
    [SerializeField] private UpdateInteractParametres allInteracts3;
    public List<ItemPickup> onSceneObjects = new List<ItemPickup>();
    [SerializeField] private List<Items> itemsToSpawn = new List<Items>();
    public int currentProgress = 0;
    [SerializeField] private GameObject player;
    [SerializeField] private Inventory inventory;
    
    [SerializeField] private string savePath;
    private bool isLoadGame = false;

    private Dictionary<int, GameObject> itemObjects = new Dictionary<int, GameObject>();

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "savegame.json");
        ItemPickup[] pickupableObjects = FindObjectsOfType<ItemPickup>();
        onSceneObjects = pickupableObjects.ToList();
    }
    public void UpdateProgress()
    {
        currentProgress++;
    }

    public void SaveProgress()
    {
        SaveData data = new SaveData()
        {
            progress = currentProgress,
            playerPosition = player.transform.position,
            playerRotation = player.transform.rotation,
            taskToSave = changeQuest.currentTask,
            updatedTask = allInteracts3.isUpdated
        };

        foreach (var item in inventory.items)
        {
            data.inventoryItems.Add(item.itemID);
        }

        foreach (var interact in allInteracts)
        {
            data.completedTasks.Add(interact.qteCompleted);
        }

        foreach (var interact in allInteracts2)
        {
            data.completedTasks2.Add(interact.onInteractionCompleted);
        }

        for (int i = 0; i < onSceneObjects.Count; i++)
        {
            ItemPickup obj = onSceneObjects[i];
            if (obj != null)
            {
                data.sceneObjects.Add(new OnSceneObject
                {
                    itemID = obj.item.itemID,
                    position = obj.transform.position
                });
            }
        }
        
        data.mapPartIndex = allInteracts3.currentMapPart;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Progress saved: " + savePath);
        Debug.Log(json);
    }

    public void LoadProgress()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            currentProgress = data.progress;

            changeQuest.ChangeTask(data.taskToSave);

            player.GetComponent<CharacterController>().enabled = false;
            player.transform.position = data.playerPosition;
            player.transform.rotation = data.playerRotation;

            inventory.items.Clear();
            foreach (var itemData in data.inventoryItems)
            {
                Items newItem = FindItemByID(itemData);
                if (newItem != null)
                {
                    inventory.items.Add(newItem);
                }
            }

            allInteracts3.isUpdated = data.updatedTask;

            for (int i = 0; i < data.completedTasks.Count; i++)
            {
                if (i < allInteracts.Count)
                {
                    allInteracts[i].qteCompleted = data.completedTasks[i];
                    Debug.Log($"[{i}] Setting qteCompleted = {data.completedTasks[i]} for object: {allInteracts[i].name} = {allInteracts[i].qteCompleted}");

                    if (data.completedTasks[i] == true)
                    {
                        allInteracts[i].gameObject.tag = "Untagged";
                    }

                    if (allInteracts[i].gameObject.GetComponent<UpdateInteractParametres>() != null)
                    {
                        
                        if (allInteracts[i].gameObject.GetComponent<UpdateInteractParametres>().isUpdated)
                        {
                            allInteracts[i].gameObject.GetComponent<UpdateInteractParametres>().currentMapPart = data.mapPartIndex;
                            allInteracts[i].gameObject.GetComponent<UpdateInteractParametres>().ChangeVariables(false);
                        }
                    }
                }
            }

            for (int i = 0; i < data.completedTasks2.Count; i++)
            {
                if (i < allInteracts2.Count)
                {
                    allInteracts2[i].onInteractionCompleted = data.completedTasks2[i];
                }
            }

            for (int i = onSceneObjects.Count - 1; i >= 0; i--)
            {
                ItemPickup sceneObject = onSceneObjects[i];

                if (data.inventoryItems.Contains(sceneObject.item.itemID))
                {
                    Destroy(sceneObject.gameObject);
                    onSceneObjects.RemoveAt(i);
                }
            }

            foreach (Items item in itemsToSpawn)
            {
                OnSceneObject getObjectData = data.sceneObjects.Find(obj => obj.itemID == item.itemID);

                if (getObjectData != null)
                {
                    if (item.itemObject != null)
                    {
                        GameObject spawnedObject = Instantiate(item.itemObject, getObjectData.position, Quaternion.identity);
                        spawnedObject.transform.localScale = item.onSceneScale;
                        if (spawnedObject.GetComponent<ItemPickup>() != null)
                        {
                            onSceneObjects.Add(spawnedObject.GetComponent<ItemPickup>());
                        }
                    }
                }
            }

            foreach (ItemPickup sceneObject in onSceneObjects)
            {
                OnSceneObject getObjectData = data.sceneObjects.Find(obj => obj.itemID == sceneObject.item.itemID);
                if (getObjectData != null)
                {
                    sceneObject.transform.position = getObjectData.position;
                }
            }

            Debug.Log("Progress loaded");
            player.GetComponent<CharacterController>().enabled = true;
        }
        else
        {
            Debug.Log("No save file found");
        }
    }

    private Items FindItemByID(int id)
    {
        return allItems.Find(item => item.itemID == id);
    }

    private ItemColliderWithPlayer FindInteract(bool isDone)
    {
        return allInteracts.Find(c => c.qteCompleted == isDone);
    }

    public void AddItemToList(GameObject itemToSave)
    {
        onSceneObjects.Add(itemToSave.GetComponent<ItemPickup>());
    }

    public void RemoveItemFromList(ItemPickup itemToDontSave)
    {
        if (itemToDontSave != null)
        {
            if (onSceneObjects.Contains(itemToDontSave))
            {
                onSceneObjects.Remove(itemToDontSave);
            }
        }
    }
}
