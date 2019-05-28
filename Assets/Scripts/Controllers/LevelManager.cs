using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    bool isLevelStarted = false;

    LevelData currentLevelData;
    Queue<BallData> remainigBallSpawns;
    List<Ball> activeBallsInLevel;

    int maxTotalHp = 0;
    int currentTotalHp = 0;
    float currentLevelTime = 0f;
    float nextBallSpawnTime = 0f;
    

    void Start()
    {
    }

    void Update()
    {
        if (!isLevelStarted)
            return;

        Debug();
        
        HandleWaves();
    }

    public void InitLevelData()
    {
        ResetLevel();
        currentLevelData = GameController.GetCurrentLevelData();
        remainigBallSpawns = new Queue<BallData>();
        activeBallsInLevel = new List<Ball>();

        foreach (var ball in currentLevelData.balls)
        {
            maxTotalHp += ball.hp;
            maxTotalHp += ball.splits[0];
            maxTotalHp += ball.splits[1];
            remainigBallSpawns.Enqueue(ball);
        }
        nextBallSpawnTime = remainigBallSpawns.Peek().delay;
        currentTotalHp = maxTotalHp;
        
        isLevelStarted = true;
    }

    void HandleWaves()
    {
        currentLevelTime += Time.deltaTime;
        if (remainigBallSpawns.Count > 0)
        {
            if (currentLevelTime > nextBallSpawnTime)
                SpawnNextBall();
        }
        else
        {
            // Win State
            if (currentTotalHp <= 0)
            {
                isLevelStarted = false;
                GameController.ChangeGameState(GameState.WIN);
            }
        }
    }

    void SpawnNextBall()
    {
        var ballData = remainigBallSpawns.Dequeue();
        var direction = (int)(Time.time * 100) % 2 == 0;
        var horizontalSpeed = Random.Range(2, 4);
        var bounceHeight = Random.Range(5, 8);
        Ball ball = SpawnBall(ballData.hp, direction, horizontalSpeed, bounceHeight, ballData.splits[0], ballData.splits[1]);
        activeBallsInLevel.Add(ball);

        if(remainigBallSpawns.Count > 0)
            nextBallSpawnTime = remainigBallSpawns.Peek().delay;
    }

    void ResetLevel()
    {
        if (activeBallsInLevel != null)
        {
            foreach(Ball ball in activeBallsInLevel)
            {
                ball.gameObject.SetActive(false);
            }
        }
        maxTotalHp = 0;
        currentLevelTime = 0f;
        nextBallSpawnTime = 0f;
    }

    /// <summary>
    /// Spawns bouncing ball with given properties from side of the screen.
    /// </summary>
    /// <param name="initialDirection">Initial direction of horizontal velocity of ball. Set 'true' for Left, 'false' for Right.</param>
    /// <param name="horizontalSpeed">Default 1 unity per second.</param>
    /// <param name="bounceHeight">Default 8 unit, which is 1 unit less than height of the side walls.</param>
    /// <param name="hitPoint">Hit point of the ball.</param>
    public Ball SpawnBall(int hitPoint = 1, bool initialDirection = false, float horizontalSpeed = 1f, float bounceHeight = 8f, int leftChildHP = 1, int rightChildHP = 1)
    {
        Vector3 ballSize = CalculateSizeWithHitPoint(hitPoint);
        float positionX = 3.5f * (initialDirection ? 1 : -1);
        float positionY = GameConstants.MAX_GAME_AREA_HEIGHT + ballSize.y / 2;
        Vector3 spawnPosition = new Vector3(positionX, positionY, 0);

        Ball ball = GameController.GetBallFromPool();
        ball.transform.position = spawnPosition;
        ball.SetMaxHitPoint(hitPoint);
        ball.SetIsLeft(initialDirection);
        ball.SetHorizontalSpeed(horizontalSpeed);
        ball.SetBounceHeight(bounceHeight);
        ball.transform.localScale = ballSize;
        ball.gameObject.SetActive(true);
        ball.SetParent(leftChildHP, rightChildHP);

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
        Vector3 ballSize = CalculateSizeWithHitPoint(hitPoint);

        Ball ball = GameController.GetBallFromPool();
        ball.transform.position = spawnPosition;
        ball.SetMaxHitPoint(hitPoint);
        ball.SetIsLeft(initialDirection);
        ball.SetHorizontalSpeed(horizontalSpeed);
        ball.SetBounceHeight(bounceHeight);
        ball.transform.localScale = ballSize;
        ball.gameObject.SetActive(true);

        return ball;
    }

    Vector3 CalculateSizeWithHitPoint(int hitPoint)
    {
        var sizeCoefficient = (hitPoint - 1) * 0.05f;

        if (sizeCoefficient > 0)
        {
            sizeCoefficient = Mathf.Clamp01(sizeCoefficient) * GameConstants.MAX_BALL_SIZE_COEFFICIENT;
            return Vector3.one * (1 + sizeCoefficient);
        }

        return Vector3.one;
    }

    public int CurrentTotalHP
    {
        get {
            return currentTotalHp;
        }
        set {
            currentTotalHp = value;
        }
    }

    public int MaxTotalHP
    {
        get {
            return maxTotalHp;
        }
    }

    public void SetIsLevelStarted(bool isLevelStarted)
    {
        this.isLevelStarted = isLevelStarted;
    }

    public bool GetIsLevelStarted()
    {
        return isLevelStarted;
    }

    void Debug()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnBall(30, (int)(Time.time * 100) % 2 == 0, Random.Range(2, 4), Random.Range(5, 8));
        }
#endif
    }
}
