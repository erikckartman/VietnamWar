using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // Movement
    private float moveSpeed = 5f;
    private float lookSensitivity = 2f;

    public bool canMove;

    //Looking
    [SerializeField] private Transform cameraTransform;
    private float xRotation = 0f;

    private void Start()
    {
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //SceneManager.LoadSceneAsync("EndScene");
        }
    }

    private void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX * moveSpeed * Time.deltaTime + transform.forward * moveZ * moveSpeed * Time.deltaTime;
        transform.position += move;
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
