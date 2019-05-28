using System;
using UnityEngine;

public class LostView : BaseView
{
    public event Action OnClickStartAction;
    [SerializeField] TMPro.TextMeshProUGUI currentScoreText;

    void OnEnable()
    {
        int score = GameController.LevelManager.MaxTotalHP - GameController.LevelManager.CurrentTotalHP;
        currentScoreText.text = score.ToString();
    }

    public void OnClickStart()
    {
        OnClickStartAction();
    }
    
}
