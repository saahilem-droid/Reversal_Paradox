using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [Header("References")]
    [SerializeField] private RectTransform hourglass;
    [SerializeField] private TMP_Text moveCounter;
    [SerializeField] private Transform arrowContainer;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GlitchNumber glitchNumber;

    [Header("Hourglass")]
[SerializeField] private float hourglassRotationDuration = 0.8f;

    private readonly List<CanvasGroup> arrows = new List<CanvasGroup>();

    private void Awake()
    {
        Instance = this;
    }

   

public void AddArrow(Vector2Int move)
{
    GameObject arrow = Instantiate(arrowPrefab, arrowContainer);

    RectTransform rt = arrow.GetComponent<RectTransform>();

    rt.anchorMin = new Vector2(0, 0.5f);
    rt.anchorMax = new Vector2(0, 0.5f);
    rt.pivot = new Vector2(0.5f, 0.5f);

    float spacing = 34f;

    rt.anchoredPosition = new Vector2(
        arrows.Count * spacing,
        0);

    rt.sizeDelta = new Vector2(28, 28);

    if (move == Vector2Int.right)
        rt.localRotation = Quaternion.Euler(0, 0, 0);
    else if (move == Vector2Int.up)
        rt.localRotation = Quaternion.Euler(0, 0, 90);
    else if (move == Vector2Int.left)
        rt.localRotation = Quaternion.Euler(0, 0, 180);
    else if (move == Vector2Int.down)
        rt.localRotation = Quaternion.Euler(0, 0, 270);

    arrows.Add(arrow.GetComponent<CanvasGroup>());
}

public void ConsumeArrow(int index)
{
    if (index < 0 || index >= arrows.Count)
        return;

    arrows[index].alpha = 0.25f;
}

public void ClearArrows()
{
    foreach (Transform child in arrowContainer)
        Destroy(child.gameObject);

    arrows.Clear();
}

public void FlipHourglass()
{
    StopCoroutine(nameof(RotateHourglass));
    StartCoroutine(RotateHourglass());
}

private IEnumerator RotateHourglass()
{
    Quaternion startRotation = hourglass.localRotation;
    Quaternion endRotation = startRotation * Quaternion.Euler(0, 0, 180);

    float timer = 0f;

    while (timer < hourglassRotationDuration)
    {
        timer += Time.unscaledDeltaTime;

        float t = timer / hourglassRotationDuration;

        // Smooth easing
        t = Mathf.SmoothStep(0f, 1f, t);

        hourglass.localRotation =
            Quaternion.Lerp(startRotation, endRotation, t);

        yield return null;
    }

    hourglass.localRotation = endRotation;
}

public void GlitchCounter(int value)
{
    if (glitchNumber != null)
        glitchNumber.GlitchToNumber(value);
}

public void SetCounter(int value)
{
    if (glitchNumber != null)
        glitchNumber.SetNumberInstant(value);
    else
        moveCounter.text = value.ToString();
}

}