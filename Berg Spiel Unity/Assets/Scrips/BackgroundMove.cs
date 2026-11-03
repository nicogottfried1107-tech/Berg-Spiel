
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    public Transform player;
    public float speed = 0.3f;

    private Vector3 lastPlayerPosition;

    void Start()
    {
        lastPlayerPosition = player.position;
    }

    void LateUpdate()
    {
        Vector3 movement = player.position - lastPlayerPosition;

        transform.position += new Vector3(
            movement.x * speed,
            movement.y * speed,
            0
        );

        lastPlayerPosition = player.position;
    }
}
