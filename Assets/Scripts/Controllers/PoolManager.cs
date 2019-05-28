using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [Header("Ball Pool configurations")]
    [SerializeField] int ballPreloadAmount = 10;
    [SerializeField] GameObject ballPrefab;

    [Header("Bullet Pool configurations")]
    [SerializeField] int bulletPreloadAmount = 30;
    [SerializeField] GameObject bulletPrefab;

    List<Ball> ballPool;
    List<Bullet> bulletPool;

    void Awake()
    {
        ballPool = new List<Ball>();
        bulletPool = new List<Bullet>();
    }

    void Start()
    {
        InitializePools();
    }

    void InitializePools()
    {
        ExpandTheBallPool(ballPreloadAmount);
        ExpandTheBulletPool(bulletPreloadAmount);
    }

    /// <summary>
    /// Gets available ball from pool. If there is no available ball, expands pool for future requests by 30%.
    /// </summary>
    /// <returns></returns>
    public Ball GetBallFromPool()
    {
        var pool = ballPool.Where(item => item.gameObject.activeInHierarchy == false);

        if (pool.Count<Ball>() > 0)
            return pool.First();
        else
        {
            ExpandTheBallPool(Mathf.CeilToInt(ballPool.Count * 0.3f));
            return GetBallFromPool();
        }
    }

    /// <summary>
    /// Gets available bullet from pool. If there is no available bullet, expands pool for future requests by 30%.
    /// </summary>
    /// <returns></returns>
    public Bullet GetBulletFromPool()
    {
        var pool = bulletPool.Where(item => item.gameObject.activeInHierarchy == false);

        if (pool.Count<Bullet>() > 0)
            return pool.First();
        else
        {
            ExpandTheBulletPool(Mathf.CeilToInt(bulletPool.Count * 0.3f));
            return GetBulletFromPool();
        }
    }

    void ExpandTheBallPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject go = Instantiate(ballPrefab, transform);
            Ball ball = go.GetComponent<Ball>();
            ballPool.Add(ball);
        }
    }

    void ExpandTheBulletPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject go = Instantiate(bulletPrefab, transform);
            Bullet bullet = go.GetComponent<Bullet>();
            bulletPool.Add(bullet);
        }
    }
}
