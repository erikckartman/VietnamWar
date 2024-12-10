using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public List<Items> items = new List<Items>();
    [SerializeField] private Items item;

    private void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            AddItem(item);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            RemoveItem(item);
        }
    }

    public void AddItem(Items newItem)
    {
        if (!items.Contains(newItem))
        {
            items.Add(newItem);
        }
    }

    public void RemoveItem(Items itemToRemove)
    {
        if (items.Contains(itemToRemove))
        {
            items.Remove(itemToRemove);
        }
    }
}
