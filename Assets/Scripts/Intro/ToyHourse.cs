using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToyHourseScript : MonoBehaviour
{
    [SerializeField] private float amplitude = 15f;
    [SerializeField] private float speed = 2f;

    private float startRotation;

    private void Start()
    {
        startRotation = transform.rotation.eulerAngles.x;
    }

    private void Update()
    {
        float rotation = startRotation + Mathf.Sin(Time.time * speed) * amplitude;
        transform.rotation = Quaternion.Euler(rotation, 0f, 0f);
    }
}
