using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private ChangeQuest changeQuest;
    public List<Items> items = new List<Items>();
    public Items activeItem;
    [SerializeField] private int maxInventorySize = 16;
    [SerializeField] private AudioSource pickupAudio;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickUpItem();
        }
    }

    private void TryPickUpItem()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3f);

        foreach (Collider hit in hitColliders)
        {
            ItemPickup itemPickup = hit.GetComponent<ItemPickup>();
            if (itemPickup != null && items.Count < maxInventorySize)
            {
                if(itemPickup.item.pickupSound != null)
                {
                    pickupAudio.clip = itemPickup.item.pickupSound;
                    pickupAudio.Play();
                }

                if (itemPickup.item.taskChanger != "null" && !itemPickup.item.isTaskChanged)
                {
                    changeQuest.ChangeTask(itemPickup.item.taskChanger);
                    itemPickup.item.isTaskChanged = true;                    
                }

                AddItem(itemPickup.item);
                Destroy(hit.gameObject);
                break;            
            }
        }
    }

    public void AddItem(Items newItem)
    {
        if (!items.Contains(newItem))
        {
            items.Add(newItem);
        }
    }

    public void SelectItem(Items selectedItem)
    {
        if (items.Contains(selectedItem))
        {
            activeItem = selectedItem;
        }
    }

    public void RemoveItem(Items itemToRemove)
    {
        if (items.Contains(itemToRemove))
        {
            GameObject removeItem = Instantiate(itemToRemove.itemObject, transform.position, Quaternion.identity);
            removeItem.transform.localScale = itemToRemove.onSceneScale;
            items.Remove(itemToRemove);

            if(activeItem == itemToRemove)
            {
                activeItem = null;
            }
        }
    }
}