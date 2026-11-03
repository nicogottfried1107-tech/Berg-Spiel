using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Scale")]
    public float hoverScale = 1.06f;

    [Header("Movement")]
    public float moveRight = 5f;

    [Header("Speed")]
    public float animationSpeed = 12f;

    [Header("Brightness")]
    public Color normalColor = Color.white;
    public Color hoverColor = new Color(1.15f, 1.15f, 1.15f, 1f);

    private Vector3 originalScale;
    private Vector3 targetScale;

    private Vector2 originalPos;
    private Vector2 targetPos;

    private Image image;

    void Start()
    {
        image = GetComponent<Image>();

        originalScale = transform.localScale;
        targetScale = originalScale;

        RectTransform rt = GetComponent<RectTransform>();
        originalPos = rt.anchoredPosition;
        targetPos = originalPos;

        image.color = normalColor;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.unscaledDeltaTime * animationSpeed);

        RectTransform rt = GetComponent<RectTransform>();

        rt.anchoredPosition = Vector2.Lerp(
            rt.anchoredPosition,
            targetPos,
            Time.unscaledDeltaTime * animationSpeed);

        image.color = Color.Lerp(
            image.color,
            targetPos == originalPos ? normalColor : hoverColor,
            Time.unscaledDeltaTime * animationSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * hoverScale;
        targetPos = originalPos + new Vector2(moveRight, 0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
        targetPos = originalPos;
    }
}