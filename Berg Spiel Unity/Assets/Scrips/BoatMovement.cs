
using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    [Header("Bewegung")]
    public Transform startPoint;
    public Transform endPoint;
    public float travelTime = 4f;

    [Header("Wartezeit an den Endpunkten")]
    public float waitTime = 2f;

    [Header("Aktivierung")]
    public bool startActivated = false;

    private float timer = 0f;
    private float waitTimer = 0f;

    private bool movingToEnd = true;
    private bool isWaiting = false;

    void Start()
    {
        // Boot steht am Startpunkt
        if (startPoint != null)
        {
            transform.position = startPoint.position;
        }
    }

    void Update()
    {
        // Ohne Aktivierung bewegt sich nichts
        if (!startActivated)
            return;

        if (startPoint == null || endPoint == null)
            return;

        float duration = Mathf.Max(0.01f, travelTime);

        // Warten an den Endpunkten
        if (isWaiting)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= Mathf.Max(0f, waitTime))
            {
                waitTimer = 0f;
                isWaiting = false;
            }

            return;
        }

        // Bewegung
        timer += Time.deltaTime;

        float progress = Mathf.Clamp01(timer / duration);

        if (movingToEnd)
        {
            transform.position = Vector3.Lerp(
                startPoint.position,
                endPoint.position,
                progress
            );
        }
        else
        {
            transform.position = Vector3.Lerp(
                endPoint.position,
                startPoint.position,
                progress
            );
        }

        // Endpunkt erreicht
        if (progress >= 1f)
        {
            timer = 0f;
            movingToEnd = !movingToEnd;
            isWaiting = true;
            waitTimer = 0f;
        }
    }

    // Wird von der Druckplatte aufgerufen
    public void ActivateBoat()
    {
        if (startActivated)
            return;

        startActivated = true;

        timer = 0f;
        waitTimer = 0f;
        movingToEnd = true;
        isWaiting = false;
    }
}
