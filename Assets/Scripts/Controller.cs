using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Movement
    private float moveSpeed = 5f;
    private float lookSensitivity = 2f;

    // Aiming
    [SerializeField] private Transform cameraTransform;
    private float xRotation = 0f;

    // Shooting
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform dulo;
    private int bullets = 10;
    [SerializeField] private float fireRate = 0.1f; 
    private bool canShoot = true;

    // Recoil
    [SerializeField] private float recoilAmount = 3f; 

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        MovePlayer();
        RotateView();

        if (Input.GetMouseButton(0) && bullets > 0 && canShoot)
        {
            StartCoroutine(Shoot());
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            bullets = 10;
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

    private IEnumerator Shoot()
    {
        canShoot = false; 
        bullets--;

        GameObject bulletClone = Instantiate(bullet, dulo.position, dulo.rotation);
        Rigidbody bulletRB = bulletClone.GetComponent<Rigidbody>();
        bulletRB.velocity = dulo.forward * 50f;
        Destroy(bulletClone, 0.5f);

        xRotation -= recoilAmount;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }
}
