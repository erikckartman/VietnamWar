using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeIsOpened : MonoBehaviour
{
    [SerializeField] private List<Animator> animator = new List<Animator>();
    [SerializeField] private GameObject objectToTake;
    private bool isOpening = false;
    private Quaternion targetRotation;
    [SerializeField] private float openSpeed = 5f;
    [SerializeField] private GameObject safeDoor;
    private SafeInteractions safeInteractions;

    private void Start()
    {
        safeInteractions = GetComponent<SafeInteractions>();

        if(safeInteractions != null)
        {
            Invoke(nameof(LoadGame), 1f);
        }
    }

    private void Update()
    {
        if(!isOpening) return;

        safeDoor.transform.rotation = Quaternion.Lerp(safeDoor.transform.rotation, targetRotation, Time.deltaTime * openSpeed);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            isOpening = false;
        }
    }
    public void OpenSafe()
    {
        targetRotation = Quaternion.Euler(safeDoor.transform.rotation.eulerAngles.x, safeDoor.transform.rotation.eulerAngles.y + 135, safeDoor.transform.rotation.eulerAngles.z);
        objectToTake.GetComponent<Collider>().enabled = true;
        objectToTake.GetComponent<Rigidbody>().useGravity = true;
        isOpening = true;
    }

    private void LoadGame()
    {
        if (safeInteractions.onInteractionCompleted)
        {
            targetRotation = Quaternion.Euler(safeDoor.transform.rotation.eulerAngles.x, safeDoor.transform.rotation.eulerAngles.y + 135, safeDoor.transform.rotation.eulerAngles.z);
            safeDoor.transform.rotation = targetRotation;
        }
    }
}
