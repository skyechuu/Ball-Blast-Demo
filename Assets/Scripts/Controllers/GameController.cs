using System.Collections;
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

    [Header("Configurations")]
    [SerializeField] string offlineConfigFileName;
    [SerializeField] GameData gameData;

    [Header("Controller references")]
    [SerializeField] PoolManager poolManager;

    static int currentLevelIndex = 0;

    void OnValidate()
    {
        Assert.IsNotNull(poolManager, "poolManager is set to null!");
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
        SceneManager.LoadScene("Start", LoadSceneMode.Additive);
        SceneManager.LoadScene("Gameplay", LoadSceneMode.Additive);
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
        return instance.gameData.levels[currentLevelIndex];
    }

    #endregion

}
