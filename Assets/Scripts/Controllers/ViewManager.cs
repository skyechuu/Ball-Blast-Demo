using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    [Header("View References")]
    [SerializeField] StartMenuView startMenuView;
    [SerializeField] GameplayView gameplayView;
    [SerializeField] WinView winView;
    [SerializeField] LostView lostView;
    
    List<BaseView> views;

    void Start()
    {
        GameController.instance.SetViewManager(this);
        InitViews();
        ActivateView(startMenuView);
    }
    
    void Update()
    {
        
    }

    void InitViews()
    {
        views = new List<BaseView>();
        views.Add(startMenuView);
        views.Add(gameplayView);
        views.Add(winView);
        views.Add(lostView);

        InitViewActions();
    }

    void InitViewActions()
    {
        startMenuView.OnClickStartAction += delegate
        {
            GameController.ChangeGameState(GameState.GAMEPLAY);
        };

        winView.OnClickStartAction += delegate
        {
            GameController.ChangeGameState(GameState.GAMEPLAY);
        };

        lostView.OnClickStartAction += delegate
        {
            GameController.ChangeGameState(GameState.GAMEPLAY);
        };
    }

    void ActivateView(BaseView view)
    {
        view.Enable();
        views.ForEach(i =>
        {
            if(i != view)
                i.Disable();
        });
    }

    public void ActivateStartView()
    {
        ActivateView(startMenuView);
    }

    public void ActivateGameplayView()
    {
        ActivateView(gameplayView);
    }

    public void ActivateWinView()
    {
        ActivateView(winView);
    }

    public void ActivateLostView()
    {
        ActivateView(lostView);
    }
}
