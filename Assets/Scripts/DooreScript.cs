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
}
