using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DooreScript : MonoBehaviour
{
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openingSpeed = 2f;

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    private void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));
    }

    private void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, isOpen ? openRotation : closedRotation, Time.deltaTime * openingSpeed);
    }

    public void OpenCloseDoor()
    {
        isOpen = !isOpen;
    }
}
