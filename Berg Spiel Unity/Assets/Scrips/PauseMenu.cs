using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject pauseScreen;
    public Image pauseButtonImage;

    [Header("Button Texturen")]
    public Sprite normalSprite;
    public Sprite pausedSprite;

    private bool isPaused = false;

    void Start()
    {
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);
        pauseButtonImage.sprite = normalSprite;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;

        pauseScreen.SetActive(isPaused);

        pauseButtonImage.sprite = isPaused
            ? pausedSprite
            : normalSprite;
    }

    void OnDestroy()
    {
        if (isPaused)
            Time.timeScale = 1f;
    }
}