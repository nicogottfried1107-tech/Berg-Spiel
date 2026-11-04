
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [Header("Einstellungen")]
    public Transform cam;

    [Range(0f, 1f)]
    public float parallaxEffect = 0.2f;

    private Vector3 startPosition;
    private float startCameraX;

    void Start()
    {
        startPosition = transform.position;
        startCameraX = cam.position.x;
    }

    void LateUpdate()
    {
        float distanceX = cam.position.x - startCameraX;

        transform.position = new Vector3(
            startPosition.x + distanceX * parallaxEffect,
            startPosition.y,
            startPosition.z
        );
    }
}
