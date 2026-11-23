
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [Header("Kamera")]
    public Transform cam;

    [Header("Horizontaler Parallax")]
    public float parallaxEffect = 0.2f;

    [Header("Vertikaler Parallax")]
    public bool enableVerticalParallax = false;
    public float verticalParallaxEffect = 0.2f;

    private Vector3 startPosition;
    private float startCameraX;
    private float startCameraY;

    void Start()
    {
        startPosition = transform.position;

        if (cam == null)
        {
            Debug.LogWarning(
                "ParallaxBackground: Keine Kamera zugewiesen!",
                this
            );
            return;
        }

        startCameraX = cam.position.x;
        startCameraY = cam.position.y;
    }

    void LateUpdate()
    {
        if (cam == null)
            return;

        // Horizontale Kamerabewegung
        float distanceX = cam.position.x - startCameraX;

        float newX = startPosition.x
                     + distanceX * parallaxEffect;

        // Vertikale Position bleibt standardmäßig gleich
        float newY = startPosition.y;

        // Vertikalen Parallax nur berechnen,
        // wenn die Funktion aktiviert ist
        if (enableVerticalParallax)
        {
            float distanceY = cam.position.y - startCameraY;

            newY = startPosition.y
                   + distanceY * verticalParallaxEffect;
        }

        // Hintergrundposition aktualisieren
        transform.position = new Vector3(
            newX,
            newY,
            startPosition.z
        );
    }
}
