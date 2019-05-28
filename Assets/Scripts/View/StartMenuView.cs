using System;
using UnityEngine;

public class StartMenuView : BaseView
{
    public event Action OnClickStartAction;

    public void OnClickStart()
    {
        OnClickStartAction();
    }
}
