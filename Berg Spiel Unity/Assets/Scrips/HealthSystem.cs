
using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int life = 3;

    public CapsuleCollider2D playerCollider;
    public PlayerMovement playerMovement;

    private bool kannSchadenErhalten = true;
    private bool istTot = false;

    public Animator playerAnimator;

    public HealthBar healthBar;

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
            TakeDamage();

            StartCoroutine(DamageCooldown());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (istTot) return;

        if (collision.gameObject.CompareTag("Bush"))
        {
            TakeDamage();
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

    public void TakeDamage()
    {
        if (istTot) return;

        life--;
        Debug.Log("Player Life: " + life);

        healthBar.PlayDamageAnimation(life);
        CameraShake.Instance.Shake(0.12f, 0.15f);

        if (life <= 0)
        {
            Sterben();
            return;
        }
    }

    public void InstantDeath()
    {
        if (istTot) return;

        life = 0;
        Debug.Log("Sofortiger Tod durch Objekt!");

        healthBar.PlayInstantDeathAnimation();

        Sterben();
    }

    public void TakeGoatDamage(Transform goat)
    {
        if (istTot || !kannSchadenErhalten) return;

        TakeDamage();
        StartCoroutine(DamageCooldown());

        if (!istTot)
        {
            playerMovement.BackBounceBush(goat);
        }
    }
}
