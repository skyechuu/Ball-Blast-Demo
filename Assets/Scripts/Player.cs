using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform muzzle;
    [SerializeField] Bullet bulletPrefab;

    [Header("Combat properties")]
    [SerializeField] float rpm;

    float nextShootTime;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }
    }

    public void Shoot()
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
