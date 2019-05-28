using UnityEngine;

public abstract class BaseView : MonoBehaviour, IView
{
    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
