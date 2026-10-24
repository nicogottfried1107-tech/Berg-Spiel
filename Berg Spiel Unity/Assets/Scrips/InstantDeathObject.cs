using UnityEngine;


using UnityEngine;

public class InstantDeathObject : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        HealthSystem health = collision.gameObject.GetComponent<HealthSystem>();

        if (health != null)
        {
            health.InstantDeath();
        }
    }
}
