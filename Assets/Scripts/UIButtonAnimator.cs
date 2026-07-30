using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ButtonType
{
    Normal,
    LevelSelect
}

public class UIButtonAnimator : MonoBehaviour, 
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler   
{
    [Header("References")]
    [SerializeField] private RectTransform target;
    [SerializeField] private TMP_Text buttonText;

    [Header("Scale")]
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float pressedScale = 0.95f;
    [SerializeField] private float animationSpeed = 12f;

    [Header("Glow")]
[SerializeField] private Color normalColor = Color.white;
[SerializeField] private Color hoverColor = new Color(1f, 0.9f, 0.3f);

[Header("Button Type")]
[SerializeField] private ButtonType buttonType = ButtonType.Normal;

private UnityEngine.UI.Button button;

    private Vector3 defaultScale;
    private Coroutine scaleRoutine;

    private void Awake()
    {
        if (target == null)
            target = GetComponent<RectTransform>();

        if (buttonText == null)
            buttonText = GetComponentInChildren<TMP_Text>();

            button = GetComponent<UnityEngine.UI.Button>();

        defaultScale = target.localScale;
    }

    private bool CanAnimate()
{
    if (buttonType == ButtonType.Normal)
        return true;

    if (button == null)
        return false;

    return button.interactable;
}

    public void OnPointerEnter(PointerEventData eventData)
{
    if (!CanAnimate())
        return;
        FindFirstObjectByType<AudioManager>()?.PlayButtonHover();

    AnimateScale(defaultScale * hoverScale);

    if (buttonText != null)
    {
        buttonText.fontStyle = FontStyles.Bold;
        buttonText.color = hoverColor;
    }
}

    public void OnPointerExit(PointerEventData eventData)
{
    if (!CanAnimate())
        return;

    AnimateScale(defaultScale);

    if (buttonText != null)
    {
        buttonText.fontStyle = FontStyles.Normal;
        buttonText.color = normalColor;
    }
}

public void OnPointerClick(PointerEventData eventData)
{
    Debug.Log("UIButtonAnimator Click");
    FindFirstObjectByType<AudioManager>()?.PlayButtonClick();
}

    public void OnPointerDown(PointerEventData eventData)
{
    if (!CanAnimate())
        return;

    AnimateScale(defaultScale * pressedScale);
}

    public void OnPointerUp(PointerEventData eventData)
{
    if (!CanAnimate())
        return;

    AnimateScale(defaultScale * hoverScale);
}

    private void AnimateScale(Vector3 targetScale)
    {
        if (scaleRoutine != null)
            StopCoroutine(scaleRoutine);

        scaleRoutine = StartCoroutine(ScaleRoutine(targetScale));
    }

    private IEnumerator ScaleRoutine(Vector3 targetScale)
    {
        while (Vector3.Distance(target.localScale, targetScale) > 0.001f)
        {
            target.localScale = Vector3.Lerp(
                target.localScale,
                targetScale,
                animationSpeed * Time.unscaledDeltaTime);

            yield return null;
        }

        target.localScale = targetScale;
    }
}