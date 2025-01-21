using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostboxInteractions : MonoBehaviour
{
    [SerializeField] private Items itemToTake;
    [SerializeField] private Vector3 spawnPos;
    public void OpenPostBox()
    {
        Debug.Log("You've just opened postbox");
        GameObject itemThrow = Instantiate(itemToTake.itemObject, transform.position + spawnPos, Quaternion.identity);
        itemThrow.transform.localScale = itemToTake.onSceneScale;
    }
}
