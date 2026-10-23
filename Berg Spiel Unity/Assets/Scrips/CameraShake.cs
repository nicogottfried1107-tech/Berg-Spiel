using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private Vector3 originalPosition;

    private void Awake()
    {
        Instance = this;
        originalPosition = transform.localPosition;
    }

    public void Shake(float duration, float strength)
    {
        StartCoroutine(ShakeCoroutine(duration, strength));
    }

    private IEnumerator ShakeCoroutine(float duration, float strength)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.localPosition = originalPosition + (Vector3)Random.insideUnitCircle * strength;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
