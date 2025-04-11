using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveData
{
    public int progress;
    public Vector3 playerPosition;
    public string taskToSave;
    public List<int> inventoryItems = new List<int>();
    public List<bool> completedTasks = new List<bool>();
}

public class ProgressSaveSystem : MonoBehaviour
{
    public static ProgressSaveSystem instance;

    [SerializeField] private ChangeQuest changeQuest;
    [SerializeField] private List<Items> allItems = new List<Items>();
    [SerializeField] private List<ItemColliderWithPlayer> allInteracts = new List<ItemColliderWithPlayer>();
    [SerializeField] private List<SafeInteractions> allInteracts2 = new List<SafeInteractions>();
    public int currentProgress = 0;
    [SerializeField] private GameObject player;
    [SerializeField] private Inventory inventory;
    
    [SerializeField] private string savePath;
    private bool isLoadGame = false;

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "savegame.json");
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
            taskToSave = changeQuest.currentTask
        };

        foreach (var item in inventory.items)
        {
            data.inventoryItems.Add(item.itemID);
        }

        foreach (var interact in allInteracts)
        {
            data.completedTasks.Add(interact.qteCompleted);
        }

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
            player.transform.position = data.playerPosition;
            changeQuest.ChangeTask(data.taskToSave);

            inventory.items.Clear();
            foreach (var itemData in data.inventoryItems)
            {
                Items newItem = FindItemByID(itemData);
                if (newItem != null)
                {
                    inventory.items.Add(newItem);
                }
            }

            for (int i = 0; i < data.completedTasks.Count; i++)
            {
                if (i < allInteracts.Count)
                {
                    allInteracts[i].qteCompleted = data.completedTasks[i];
                }
            }

            for (int i = 0; i < data.completedTasks.Count; i++)
            {
                if (i < allInteracts2.Count)
                {
                    allInteracts2[i].onInteractionCompleted = data.completedTasks[i];
                }
            }

            Debug.Log("Progress loaded");
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
}
