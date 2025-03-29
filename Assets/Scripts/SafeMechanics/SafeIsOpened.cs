using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeIsOpened : MonoBehaviour
{
    [SerializeField] private List<Animator> animator = new List<Animator>();
    [SerializeField] private GameObject objectToTake;
    public void OpenSafe()
    {
        objectToTake.GetComponent<Collider>().enabled = true;
        objectToTake.GetComponent<Rigidbody>().useGravity = true;
    }
}
