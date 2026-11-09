using UnityEngine;
using UnityEngine.UI;

public class MusicToggle : MonoBehaviour
{
    public Image musicButtonImage;

    public Sprite musicOnSprite;
    public Sprite musicOffSprite;

    private bool musicOn = true;

    void Start()
    {
        musicOn = !AudioListener.pause;
        UpdateButton();
    }

    public void ToggleMusic()
    {
        musicOn = !musicOn;

        AudioListener.pause = !musicOn;

        UpdateButton();
    }

    void UpdateButton()
    {
        musicButtonImage.sprite = musicOn
            ? musicOnSprite
            : musicOffSprite;
    }
}
