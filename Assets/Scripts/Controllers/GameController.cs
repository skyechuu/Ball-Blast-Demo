using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public enum GameState
{
    MAIN,
    GAMEPLAY,
    WIN,
    LOST
}

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Header("State")]
    [SerializeField] GameState state = GameState.MAIN;

    [Header("Configurations")]
    [SerializeField] string offlineConfigFileName;
    [SerializeField] GameData gameData;

    [Header("Controller references")]
    [SerializeField] PoolManager poolManager;
    [SerializeField] LevelManager levelManager;
    
    [Header("Runtime references")]
    [SerializeField] ViewManager viewManager;
    [SerializeField] Player player;
    [SerializeField] CameraController cameraController;

    static int currentLevelIndex = 0;

    void OnValidate()
    {
        Assert.IsNotNull(poolManager, "poolManager is set to null!");
        Assert.IsNotNull(levelManager, "levelManager is set to null!");
    }
    
    void Awake()
    {
        instance = this;
		StartCoroutine(LoadConfigFile());
        StartCoroutine(IsGameDataReady());
    }
    
    void Start()
    {
    }
    
    void Update()
    {
    }

    IEnumerator LoadConfigFile()
    {
        var filePath = Path.Combine(Application.streamingAssetsPath, offlineConfigFileName);

        var jsonData = "";
        if (filePath.Contains("://"))
        {
            UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(filePath);
            yield return www.SendWebRequest();
            jsonData = www.downloadHandler.text;
            gameData = JsonUtility.FromJson<GameData>(jsonData);
        }
        else
        {
            jsonData = System.IO.File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<GameData>(jsonData);
        }
    }

    IEnumerator IsGameDataReady()
    {
        while (gameData == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
        OnLoadSuccessful();
    }

    void OnLoadSuccessful()
    {
        print("Game Data is ready!");
        SceneManager.LoadScene("View", LoadSceneMode.Additive);
        SceneManager.LoadScene("Gameplay", LoadSceneMode.Additive);
    }
    
    void OnStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.MAIN:
                OnStateMain();
                break;
            case GameState.GAMEPLAY:
                OnStateGameplay();
                break;
            case GameState.LOST:
                OnStateLost();
                break;
            case GameState.WIN:
                OnStateWin();
                break;
        }
    }

    void OnStateGameplay()
    {
        levelManager.InitLevelData();
        viewManager.ActivateGameplayView();
        player.gameObject.SetActive(true);
    }

    void OnStateMain()
    {
        viewManager.ActivateStartView();
    }

    void OnStateLost()
    {
        levelManager.SetIsLevelStarted(false);
        viewManager.ActivateLostView();
    }

    void OnStateWin()
    {
        currentLevelIndex++;
        viewManager.ActivateWinView();
    }

    LevelData GetLevelData()
    {
        if(currentLevelIndex >= gameData.levels.Count)
        {
            // Generate Random Level with diff +- %20 of Level 5
            float diff = Random.Range(0.8f, 1.2f);

            LevelData lastPremadeLevel = gameData.levels[gameData.levels.Count - 1];
            LevelData newLevel = new LevelData();
            newLevel.balls = new List<BallData>();

            foreach(var ball in lastPremadeLevel.balls)
            {
                BallData ballData = new BallData();
                ballData.splits = new List<int>();
                ballData.delay = ball.delay;
                ballData.hp = Mathf.CeilToInt(ball.hp * diff);
                ballData.splits.Add(Mathf.CeilToInt(ball.splits[0] * diff));
                ballData.splits.Add(Mathf.CeilToInt(ball.splits[1] * diff));
                newLevel.balls.Add(ballData);
            }
            return newLevel;
        }
        return gameData.levels[currentLevelIndex];
    }

    public void SetGameState(GameState state)
    {
        this.state = state;
        OnStateChanged(state);
    }

    public void SetViewManager(ViewManager viewManager)
    {
        this.viewManager = viewManager;
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }

    public void SetCameraController(CameraController cameraController)
    {
        this.cameraController = cameraController;
    }

    #region STATIC API

    public static Ball GetBallFromPool()
    {
        return instance.poolManager.GetBallFromPool();
    }

    public static Bullet GetBulletFromPool()
    {
        return instance.poolManager.GetBulletFromPool();
    }

    public static GameData GetGameData()
    {
        return instance.gameData;
    }

    public static LevelData GetCurrentLevelData()
    {
        return instance.GetLevelData();
    }

    public static LevelData ProceedNextLevel()
    {
        currentLevelIndex++;
        return GetCurrentLevelData();
    }

    public static LevelManager LevelManager
    {
        get {
            return instance.levelManager;
        }
    }

    public static void ChangeGameState(GameState state)
    {
        instance.SetGameState(state);
    }

    public static void Shake(float duration, float magnitude)
    {
        instance.cameraController.Shake(duration, magnitude);
    }

    #endregion

}
