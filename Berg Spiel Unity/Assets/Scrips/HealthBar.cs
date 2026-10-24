using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    public HealthSystem healthSystem;

    public Image life1;
    public Image life2;
    public Image life3;

    public Sprite lifeFull;
    public Sprite lifeFlash;
    public Sprite lifeEmpty;

    void Update()
    {
    }

    void Start()
    {
        // Setze die Lebensanzeige auf den vollen Zustand zu Beginn
        life1.sprite = lifeFull;
        life2.sprite = lifeFull;
        life3.sprite = lifeFull;
    }

    public void PlayDamageAnimation(int remainingLives)
    {
        if (remainingLives == 2)
        {
            StartCoroutine(FlashLife(life3));
        }
        else if (remainingLives == 1)
        {
            StartCoroutine(FlashLife(life2));
        }
        else if (remainingLives == 0)
        {
            StartCoroutine(FlashLife(life1));
        }
    }

    private IEnumerator FlashLife(Image lifeImage)
    {
        lifeImage.sprite = lifeFlash;

        yield return new WaitForSeconds(0.3f);

        lifeImage.sprite = lifeEmpty;
    }

    public void ShowAllLivesEmpty()
    {
        StopAllCoroutines();

        life1.sprite = lifeEmpty;
        life2.sprite = lifeEmpty;
        life3.sprite = lifeEmpty;
    }

    public void PlayInstantDeathAnimation()
    {
        StopAllCoroutines();
        StartCoroutine(FlashAllLives());
    }

    private IEnumerator FlashAllLives()
    {
        life1.sprite = lifeFlash;
        life2.sprite = lifeFlash;
        life3.sprite = lifeFlash;

        yield return new WaitForSeconds(0.3f);

        life1.sprite = lifeEmpty;
        life2.sprite = lifeEmpty;
        life3.sprite = lifeEmpty;
    }


}
