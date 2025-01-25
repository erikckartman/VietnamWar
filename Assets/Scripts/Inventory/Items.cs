using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item")]
public class Items : ScriptableObject
{
    public string itemName;
    public int itemID;
    public Sprite itemIcon;
    public GameObject itemObject;
    public Vector3 onSceneScale;
    public string listText;
    public AudioClip pickupSound;
    public int thoughIndex;
}
