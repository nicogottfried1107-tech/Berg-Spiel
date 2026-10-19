
using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    int life = 3;

    public CapsuleCollider2D playerCollider;
    public PlayerMovement playerMovement;

    private bool kannSchadenErhalten = true;
    private bool istTot = false;

    public Animator playerAnimator;

    IEnumerator DamageCooldown()
    {
        kannSchadenErhalten = false;
        yield return new WaitForSeconds(1.5f);
        kannSchadenErhalten = true;
    }

    void Update()
    {
        if (istTot) return;

        if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Water"))
            && kannSchadenErhalten)
        {
            life--;
            Debug.Log("Player Life: " + life);

            if (life <= 0)
            {
                Sterben();
                return;
            }

            StartCoroutine(DamageCooldown());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (istTot) return;

        if (collision.gameObject.CompareTag("Bush"))
        {
            life--;
            Debug.Log("Player Life: " + life);

            if (life <= 0)
            {
                Sterben();
                return;
            }

            playerMovement.BackBounceBush(collision.transform);
        }
    }

    private void Sterben()
    {
        if (istTot) return;

        istTot = true;
        Debug.Log("Spieler ist gestorben!");

        if (DeathScreenManager.Instance != null)
        {
            DeathScreenManager.Instance.StartDeathSequence(
                gameObject,
                playerAnimator
            );
        }
        else
        {
            Debug.LogError("DeathScreenManager wurde nicht gefunden!");
        }
    }
}
