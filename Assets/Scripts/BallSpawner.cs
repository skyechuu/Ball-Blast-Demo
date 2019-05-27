using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] int ballPreloadAmount = 10;
    [SerializeField] GameObject ballPrefab;

    List<Ball> ballPool;

    void Awake()
    {
        ballPool = new List<Ball>();
    }
    
    void Start()
    {
        InitializePool();
        
    }
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            SpawnBall(Random.Range(1, 10), (int)(Time.time * 100) % 2 == 0, Random.Range(2, 4), Random.Range(4, 8));
        }
    }

    /// <summary>
    /// Spawns bouncing ball with given properties from side of the screen.
    /// </summary>
    /// <param name="initialDirection">Initial direction of horizontal velocity of ball. Set 'true' for Left, 'false' for Right.</param>
    /// <param name="horizontalSpeed">Default 1 unity per second.</param>
    /// <param name="bounceHeight">Default 8 unit, which is 1 unit less than height of the side walls.</param>
    /// <param name="hitPoint">Hit point of the ball.</param>
    Ball SpawnBall(int hitPoint = 1, bool initialDirection = false, float horizontalSpeed = 1f, float bounceHeight = 8f)
    {
        float positionX = 3.5f * (initialDirection ? 1 : -1);
        Vector3 spawnPosition = new Vector3(positionX, transform.position.y, 0);

        Ball ball = GetBallFromPool();
        ball.transform.position = spawnPosition;
        ball.SetHitPoint(hitPoint);
        ball.SetIsLeft(initialDirection);
        ball.SetHorizontalSpeed(horizontalSpeed);
        ball.SetBounceHeight(bounceHeight);
        ball.gameObject.SetActive(true);

        return ball;
    }

    /// <summary>
    /// Spawns bouncing ball with given properties from given Ball position.
    /// </summary>
    /// <param name="parent">Parent ball that spawned the balls.</param>
    /// <param name="initialDirection">Initial direction of horizontal velocity of ball. Set 'true' for Left, 'false' for Right.</param>
    /// <param name="horizontalSpeed">Default 1 unity per second.</param>
    /// <param name="bounceHeight">Default 9 unit, which is also size of the side walls.</param>
    /// <param name="hitPoint">Hit point of the ball.</param>
    Ball SpawnBall(Ball parent, int hitPoint = 1, bool initialDirection = false, float horizontalSpeed = 1f, float bounceHeight = 9f)
    {
        Vector3 spawnPosition =  parent.transform.position;

        Ball ball = GetBallFromPool();
        ball.transform.position = spawnPosition;
        ball.SetHitPoint(hitPoint);
        ball.SetIsLeft(initialDirection);
        ball.SetHorizontalSpeed(horizontalSpeed);
        ball.SetBounceHeight(bounceHeight);
        ball.gameObject.SetActive(true);

        return ball;
    }

    /// <summary>
    /// Gets available ball from pool. If there is no available ball, expands pool for future requests by 30%.
    /// </summary>
    /// <returns></returns>
    Ball GetBallFromPool()
    {
        var pool = ballPool.Where(item => item.gameObject.activeInHierarchy == false);

        if (pool.Count<Ball>() > 0)
            return pool.First();
        else
        {
            ExpandThePool(Mathf.CeilToInt(ballPool.Count * 0.3f));
            return GetBallFromPool();
        }
    }

    void InitializePool()
    {
        ExpandThePool(ballPreloadAmount);
    }

    void ExpandThePool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject go = Instantiate(ballPrefab);
            Ball ball = go.GetComponent<Ball>();
            ballPool.Add(ball);
        }
    }
    

}
