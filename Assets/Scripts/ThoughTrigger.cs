using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoughTrigger : MonoBehaviour
{
    [SerializeField] private Thoughs thoughs;
    [SerializeField] private int index;
    [SerializeField] private Collider boxCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            thoughs.ShowThought(index);
        }
    }

    public void EnableCollider()
    {
        if (boxCollider != null)
        {
            boxCollider.enabled = true;
        }
    }
}
