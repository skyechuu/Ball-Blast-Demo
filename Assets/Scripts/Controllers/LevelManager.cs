using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    LevelData currentLevelData;

    void Start()
    {
        currentLevelData = GameController.GetCurrentLevelData();
    }


    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnBall(Random.Range(1, 10), (int)(Time.time * 100) % 2 == 0, Random.Range(2, 4), Random.Range(5, 8));
        }
#endif
    }

    /// <summary>
    /// Spawns bouncing ball with given properties from side of the screen.
    /// </summary>
    /// <param name="initialDirection">Initial direction of horizontal velocity of ball. Set 'true' for Left, 'false' for Right.</param>
    /// <param name="horizontalSpeed">Default 1 unity per second.</param>
    /// <param name="bounceHeight">Default 8 unit, which is 1 unit less than height of the side walls.</param>
    /// <param name="hitPoint">Hit point of the ball.</param>
    public Ball SpawnBall(int hitPoint = 1, bool initialDirection = false, float horizontalSpeed = 1f, float bounceHeight = 8f)
    {
        float positionX = 3.5f * (initialDirection ? 1 : -1);
        Vector3 spawnPosition = new Vector3(positionX, GameConstants.MAX_GAME_AREA_HEIGHT + 0.5f, 0);

        Ball ball = GameController.GetBallFromPool();
        ball.transform.position = spawnPosition;
        ball.SetHitPoint(hitPoint);
        ball.SetIsLeft(initialDirection);
        ball.SetHorizontalSpeed(horizontalSpeed);
        ball.SetBounceHeight(bounceHeight);
        ball.gameObject.SetActive(true);
        ball.SetIsParent(true);

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
    public Ball SpawnBall(Ball parent, int hitPoint = 1, bool initialDirection = false, float horizontalSpeed = 1f, float bounceHeight = 9f)
    {
        Vector3 spawnPosition = parent.transform.position;

        Ball ball = GameController.GetBallFromPool();
        ball.transform.position = spawnPosition;
        ball.SetHitPoint(hitPoint);
        ball.SetIsLeft(initialDirection);
        ball.SetHorizontalSpeed(horizontalSpeed);
        ball.SetBounceHeight(bounceHeight);
        ball.gameObject.SetActive(true);

        return ball;
    }
}
