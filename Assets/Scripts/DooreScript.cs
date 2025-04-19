using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DooreScript : MonoBehaviour
{
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openingSpeed = 2f;
    private ItemColliderWithPlayer itemColliderWithPlayer;

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    private void Start()
    {
        itemColliderWithPlayer = GetComponent<ItemColliderWithPlayer>();

        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));

        if(itemColliderWithPlayer != null)
        {
            Invoke(nameof(LoadGame), 1f);
        }
    }

    private void Update()
    {
        if(!isOpen) return;

        transform.rotation = Quaternion.Slerp(transform.rotation, openRotation, Time.deltaTime * openingSpeed);
        if (Quaternion.Angle(transform.rotation, openRotation) < 0.1f)
        {
            isOpen = false;
        }
    }

    public void OpenCloseDoor()
    {
        isOpen = true;
    }

    private void LoadGame()
    {
        if (itemColliderWithPlayer.qteCompleted)
        {
            transform.rotation = openRotation;
        }
    }
}
