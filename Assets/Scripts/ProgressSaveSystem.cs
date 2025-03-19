using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    public int progress;
    public Vector3 playerPosition;
    public List<Items> inventoryItems = new List<Items>();
}

public class ProgressSaveSystem : MonoBehaviour
{
    public static ProgressSaveSystem instance;

    public int currentProgress = 0;
    [SerializeField] private GameObject player;
    [SerializeField] private Inventory inventory;
    
    [SerializeField] private string savePath;
    public static bool isLoadGame = false;

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "savegame.json");
    }
    private void Start()
    {
        if (isLoadGame)
        {
            LoadProgress();
            isLoadGame = false;
        }
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
            playerPosition = player.transform.position
        };

        foreach (var item in inventory.items)
        {
            data.inventoryItems.Add(new Items { itemName = item.itemName, itemID = item.itemID });
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Progress saved: " + savePath);
    }

    public void LoadProgress()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            currentProgress = data.progress;
            player.transform.position = data.playerPosition;

            inventory.items.Clear();
            foreach (var itemData in data.inventoryItems)
            {
                Items newItem = new Items { itemName = itemData.itemName, itemID = itemData.itemID };
                inventory.items.Add(newItem);
            }

            Debug.Log("Progress loaded");
        }
        else
        {
            Debug.Log("No save file found");
        }
    }

}
