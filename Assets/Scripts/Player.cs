using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform muzzle;
    [SerializeField] Bullet bulletPrefab;

    [Header("Properties")]
    [SerializeField] float rpm;
    [SerializeField] float smoothness;
    
    float nextShootTime;
    Vector3 currentVelocity;

    void Awake()
    {
    }

    void Update()
    {
        if (PlayerInput.IsInputContinues)
        {
            Move();
            Shoot();
        }
    }

    void Move()
    {
        // Clamp new position into game area
        float targetX = Mathf.Clamp(PlayerInput.InputPosition().x, -2.5f, 2.5f);
        Vector3 newPosition = transform.position;
        newPosition.x = targetX;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref currentVelocity, smoothness);
    }

    void Shoot()
    {
        if (Time.time > nextShootTime)
        {
            nextShootTime = Time.time + RpmToSec(rpm);
            Bullet bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
            bullet.OnSpawn();
        }
    }

    float RpmToSec(float rpm)
    {
        return 60f / rpm;
    }
}
