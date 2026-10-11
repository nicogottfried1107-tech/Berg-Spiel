
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float travelTime = 4f;

    private float timer = 0f;

    void Update()
    {
        if (startPoint == null || endPoint == null)
            return;

        timer += Time.deltaTime;

        float progress = Mathf.PingPong(timer / travelTime, 1f);

        transform.position = Vector3.Lerp(
            startPoint.position,
            endPoint.position,
            progress
        );
    }
}
