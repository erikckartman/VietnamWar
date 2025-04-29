using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    private Vector3 velocity;
    public bool canMove = true;

    [Header("Audio")]
    [SerializeField] private AudioSource walkSound;

    [Header("Camera Look")]
    [SerializeField] private Transform cameraTransform;
    public float lookSensitivity = 2f;
    private float xRotation = 0f;

    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (canMove)
        {
            MovePlayer();
            RotateView();
        }
        else
        {
            if (walkSound.isPlaying)
            {
                walkSound.Stop();
            }
        }
    }

    private void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        characterController.Move(move * moveSpeed * Time.deltaTime);

        // Apply gravity
        if (!characterController.isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else
        {
            velocity.y = -2f; // to keep grounded
        }

        characterController.Move(velocity * Time.deltaTime);

        // Walk sound logic
        if (moveX == 0 && moveZ == 0)
        {
            if (walkSound.isPlaying)
            {
                walkSound.Stop();
            }
        }
        else
        {
            if (!walkSound.isPlaying)
            {
                walkSound.Play();
            }
        }
    }

    private void RotateView()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
