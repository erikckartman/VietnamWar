using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorFallScript : MonoBehaviour
{
    public Transform player;
    public float interactionDistance = 3f;
    public float fallSpeed = 3f;
    private bool isFalling = false;

    private Quaternion targetRotation;
    private ItemColliderWithPlayer itemColliderWithPlayer;

    private void Start()
    {
        targetRotation = Quaternion.Euler(-90, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        itemColliderWithPlayer = GetComponent<ItemColliderWithPlayer>();
        
        if(itemColliderWithPlayer != null)
        {
            Invoke(nameof(OnLoadGame), 1f);
        }
    }

    private void Update()
    {
        if (isFalling)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * fallSpeed);

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                isFalling = false;
            }
        }
    }

    public void BreakTheDoor()
    {
        isFalling = true;
    }

    public void OnLoadGame()
    {
        if (itemColliderWithPlayer.qteCompleted)
        {
            isFalling = true;
            transform.rotation = targetRotation;
        }
    }
}
