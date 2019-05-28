using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
    static Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        //HandleInputs();
    }

    void HandleInputs()
    {
        if (NoInputExists) return;
    }

    bool NoInputExists
    {
#if UNITY_EDITOR
        get { return false; }
#else
        get
        {
            return Input.touchCount <= 0;
        }
#endif
    }

    public static Vector2 RawInputPosition
    {
#if UNITY_EDITOR
        get { return Input.mousePosition; }
#else
        get { return Input.GetTouch(0).position; }
#endif
    }

    public bool IsInputStarted
    {
#if UNITY_EDITOR
        get {
            return Input.GetMouseButtonDown(0);
        }
#else
        get { return Input.GetTouch(0).phase == TouchPhase.Began; }
#endif
    }

    public static bool IsInputContinues
    {
#if UNITY_EDITOR
        get { return Input.GetMouseButton(0); }
#else
        get { return Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary; }
#endif
    }

    public static bool IsInputEnded
    {
#if UNITY_EDITOR
        get { return Input.GetMouseButtonUp(0); }
#else
        get { return Input.GetTouch(0).phase == TouchPhase.Ended; }
#endif
    }

    public static bool IsInputOverUI
    {
        get { return EventSystem.current.IsPointerOverGameObject(-1); }
    }

    public static Vector3 InputPosition()
    {
        Ray ray = cam.ScreenPointToRay(RawInputPosition);
        Plane groundPlane = new Plane(-Vector3.forward, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            Debug.DrawLine(ray.origin, point, Color.red);
            return point;
        }
        else
            return Vector3.zero;
    }
}
