using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectIntro : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private RectTransform panel;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CanvasGroup buttonsCanvasGroup;

    [Header("Buttons")]
    [SerializeField] private RectTransform[] buttons;
    [SerializeField] private float buttonDropDistance = 200f;
    [SerializeField] private float buttonDropDuration = 0.3f;
    [SerializeField] private float delayBetweenButtons = 0.08f;

    [Header("Panel Animation")]
    [SerializeField] private float introDuration = 0.4f;
    [SerializeField] private float overshootScale = 1.05f;

    private Vector2[] originalPositions;

    private Vector3 originalScale;

    private void Awake()
    {
        if (panel == null)
            panel = GetComponent<RectTransform>();

        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        originalPositions = new Vector2[buttons.Length];

        for (int i = 0; i < buttons.Length; i++)
        {
            originalPositions[i] = buttons[i].anchoredPosition;

            buttons[i].anchoredPosition += Vector2.up * buttonDropDistance;

            Button b = buttons[i].GetComponent<Button>();
            if (b != null)
                b.interactable = false;
        }

        originalScale = panel.localScale;

panel.localScale = originalScale * 0.2f;
canvasGroup.alpha = 0f;
    }

    private void Start()
    {
        StartCoroutine(PlayIntro());
    }

    private IEnumerator PlayIntro()
    {
        // Panel Zoom + Fade
        float timer = 0f;

        while (timer < introDuration)
        {
            timer += Time.unscaledDeltaTime;

            float t = timer / introDuration;

            panel.localScale = Vector3.Lerp(
    originalScale * 0.2f,
    originalScale * overshootScale,
    t);
            canvasGroup.alpha = t;

            yield return null;
        }

        timer = 0f;

        while (timer < 0.12f)
        {
            timer += Time.unscaledDeltaTime;

            float t = timer / 0.12f;

            panel.localScale = Vector3.Lerp(
    originalScale * overshootScale,
    originalScale,
    t);

            yield return null;
        }

        panel.localScale = originalScale;

        // Buttons Drop
        for (int i = 0; i < buttons.Length; i++)
        {
            StartCoroutine(DropButton(buttons[i], originalPositions[i]));

            yield return new WaitForSecondsRealtime(delayBetweenButtons);
        }
    }

    private IEnumerator DropButton(RectTransform button, Vector2 targetPos)
    {
        Vector2 startPos = button.anchoredPosition;

        float timer = 0f;

        while (timer < buttonDropDuration)
        {
            timer += Time.unscaledDeltaTime;

            float t = timer / buttonDropDuration;

            button.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);

            yield return null;
        }

        button.anchoredPosition = targetPos;

       
    }
}