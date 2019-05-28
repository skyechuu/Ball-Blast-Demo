using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.localPosition;
        GameController.instance.SetCameraController(this);
    }

    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(ShakeEnumerator(duration, magnitude));
    }

    IEnumerator ShakeEnumerator (float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while(elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = new Vector3(x, y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
    }


}
