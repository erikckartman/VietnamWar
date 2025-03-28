using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeIsOpened : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject objectToTake;
    public void OpenSafe()
    {
        objectToTake.GetComponent<Collider>().enabled = true;
        objectToTake.GetComponent<Rigidbody>().useGravity = true;
    }
}
