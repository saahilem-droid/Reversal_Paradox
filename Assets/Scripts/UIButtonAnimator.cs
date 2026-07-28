using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

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

    private Vector3 defaultScale;
    private Coroutine scaleRoutine;

    private void Awake()
    {
        if (target == null)
            target = GetComponent<RectTransform>();

        if (buttonText == null)
            buttonText = GetComponentInChildren<TMP_Text>();

        defaultScale = target.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AnimateScale(defaultScale * hoverScale);
        //buttonText.fontStyle = FontStyles.Bold;
//buttonText.color = hoverColor;

        if (buttonText != null)
        {
            buttonText.fontStyle = FontStyles.Bold;
buttonText.color = hoverColor;
    }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AnimateScale(defaultScale);
        //buttonText.fontStyle = FontStyles.Normal;
//buttonText.color = normalColor;

        if (buttonText != null)
        {
            buttonText.fontStyle = FontStyles.Normal;
buttonText.color = normalColor;
    }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        AnimateScale(defaultScale * pressedScale);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
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