using UnityEngine;
using UnityEngine.UI;

public class GameplayView : BaseView
{
    [SerializeField] Image hpProgressBar;
    [SerializeField] TMPro.TextMeshProUGUI progressText;

    void Update()
    {
        Render();
    }

    void Render()
    {
        float current = GameController.LevelManager.CurrentTotalHP;
        float max = GameController.LevelManager.MaxTotalHP;
        var value = current / max;
        hpProgressBar.fillAmount = value;
        progressText.text = string.Format("{0} / {1}", current, max);
    }
    
}
