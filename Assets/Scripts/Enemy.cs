using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum AIState { PATROL, FOLLOW, SHOOT }
    private AIState state;

    [SerializeField] private Transform player;
    private float viewDistance = 15f;
    private float shootingDistance = 10f;
    private float followDistance = 20f;

    private float patrolLookAngle = 45f;
    private float patrolLookSpeed = 2f;
    private float patrolTimer = 0f;
    private bool lookingRight = true;

    private float followSpeed = 5f;
    private float shootCooldown = 2f;
    private float nextShootTime = 0f;

    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform shootPoint;
    private float bulletSpeed = 20f;
    private float dodgeSpeed = 3f;
    private float dodgeDistance = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        switch (state)
        {
            case AIState.PATROL:
                Patrol();
                break;
            case AIState.FOLLOW:
                Follow();
                break;
            case AIState.SHOOT:
                Shoot();
                break;
        }

        CheckStateChange();
    }

    private void Patrol()
    {
        patrolTimer += Time.deltaTime * patrolLookSpeed;
        float angle = lookingRight ? patrolLookAngle : -patrolLookAngle;
        transform.rotation = Quaternion.Euler(0f, angle * Mathf.Sin(patrolTimer), 0f);

        if (Mathf.Abs(Mathf.Sin(patrolTimer)) >= 0.99f)
        {
            lookingRight = !lookingRight;
        }
    }

    private void Follow()
    {
        transform.LookAt(player);
        transform.position = Vector3.MoveTowards(transform.position, player.position, followSpeed * Time.deltaTime);

        if (Time.time >= nextShootTime && Vector3.Distance(transform.position, player.position) <= shootingDistance)
        {
            FireBullet();
            nextShootTime = Time.time + shootCooldown;
        }
    }

    private void Shoot()
    {
        transform.LookAt(player);

        if (ShouldDodge())
        {
            //Dodge();
            return;
        }
        else
        {
            if (Time.time >= nextShootTime)
            {
                FireBullet();
                nextShootTime = Time.time + shootCooldown;
            }
        }
    }

    private bool ShouldDodge()
    {
        return Random.value > 0.5f;
    }

    private void Dodge()
    {
        Vector3 dodgeDirection = transform.right * (Random.value > 0.5f ? 1 : -1);
        Vector3 dodgePosition = transform.position + dodgeDirection * dodgeDistance;
        transform.position = Vector3.Lerp(transform.position, dodgePosition, dodgeSpeed * Time.deltaTime);
    }

    private void FireBullet()
    {
        GameObject bulletClone = Instantiate(bullet, shootPoint.position, shootPoint.rotation);
        Rigidbody bulletRB = bulletClone.GetComponent<Rigidbody>();
        bulletRB.velocity = transform.forward * bulletSpeed;
        Destroy(bulletClone, 2f);
    }

    private void CheckStateChange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (state)
        {
            case AIState.PATROL:
                if (distanceToPlayer <= viewDistance)
                {
                    state = AIState.FOLLOW;
                }
                break;

            case AIState.FOLLOW:
                if (distanceToPlayer > followDistance)
                {
                    state = AIState.PATROL;
                }
                else if (distanceToPlayer <= shootingDistance)
                {
                    state = AIState.SHOOT;
                }
                break;

            case AIState.SHOOT:
                if (distanceToPlayer > shootingDistance)
                {
                    state = AIState.FOLLOW;
                }
                break;
        }
    }
}
