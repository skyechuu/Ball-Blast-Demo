using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform muzzle;
    [SerializeField] Bullet bulletPrefab;

    [Header("Properties")]
    [SerializeField] int rpm;
    [SerializeField] int bulletDamage;
    [SerializeField] float smoothness;
    
    float nextShootTime;
    Vector3 currentVelocity;

    void Start()
    {
        GameController.instance.SetPlayer(this);
    }
    
    void Update()
    {
        if (!GameController.LevelManager.GetIsLevelStarted())
            return;

        if (PlayerInput.IsInputContinues && !PlayerInput.IsInputOverUI)
        {
            Move();
            Shoot();
        }
    }

    void Move()
    {
        // Clamp new position into game area
        float targetX = Mathf.Clamp(PlayerInput.InputPosition().x, -GameConstants.MAX_HORIZONTAL_DIMENSION_FOR_PLAYER, GameConstants.MAX_HORIZONTAL_DIMENSION_FOR_PLAYER);
        Vector3 newPosition = transform.position;
        newPosition.x = targetX;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref currentVelocity, smoothness);
    }

    void Shoot()
    {
        if (Time.time > nextShootTime)
        {
            nextShootTime = Time.time + RpmToSec(rpm);

            Bullet bullet = GameController.GetBulletFromPool();
            bullet.transform.position = muzzle.position;
            bullet.transform.rotation = muzzle.rotation;
            bullet.SetDamage(bulletDamage);
            bullet.gameObject.SetActive(true);
        }
    }

    float RpmToSec(float rpm)
    {
        return 60f / rpm;
    }

    public void UpgradeRPM(int amount)
    {
        rpm += amount;
    }

    public void UpgradeBulletDamage(int amount)
    {
        bulletDamage += amount;
    }
}
