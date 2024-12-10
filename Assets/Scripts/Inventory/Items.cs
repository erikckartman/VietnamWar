using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class Items : ScriptableObject
{
    public string itemName;
    public int itemID;
    public Sprite itemIcon;
    public GameObject itemObject;
}
