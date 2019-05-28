using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public float gravity;
    public int bullet_count_increase;
    public int bullet_damage_increase;
    public List<LevelData> levels;
}

[System.Serializable]
public class BallData
{
    public int hp;
    public List<int> splits;
    public int delay;
}

[System.Serializable]
public class LevelData
{
    public List<BallData> balls;
}