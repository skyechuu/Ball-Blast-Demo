using System;

public class WinView : BaseView
{
    public event Action OnClickStartAction;

    public void OnClickStart()
    {
        OnClickStartAction();
    }
}
