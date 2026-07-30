using System.Collections;
using UnityEngine;

public class UIPanelZoom : MonoBehaviour
{
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private AnimationCurve curve =
        AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Vector3 targetScale;

    private void Awake()
    {
        targetScale = transform.localScale;
    }

    public void PlayZoomIn()
    {
        StopAllCoroutines();
        transform.localScale = Vector3.zero;
        StartCoroutine(ZoomIn());
    }

    private IEnumerator ZoomIn()
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;

            transform.localScale = Vector3.LerpUnclamped(
                Vector3.zero,
                targetScale,
                curve.Evaluate(t / duration));

            yield return null;
        }

        transform.localScale = targetScale;
    }
}