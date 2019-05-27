using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] int ballPreloadAmount = 7;
    [SerializeField] GameObject ballPrefab;
    
    void Start()
    {
        SpawnBall(1, true, 1, 9);
        SpawnBall(1, false, 2, 9);
    }
    
    void Update()
    {
        
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
        GameObject go = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);

        Ball ball = go.GetComponent<Ball>();
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
        GameObject go = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);

        Ball ball = go.GetComponent<Ball>();
        ball.SetHitPoint(hitPoint);
        ball.SetIsLeft(initialDirection);
        ball.SetHorizontalSpeed(horizontalSpeed);
        ball.SetBounceHeight(bounceHeight);
        ball.gameObject.SetActive(true);

        return ball;
    }

}
