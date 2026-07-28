using System.Collections;
using TMPro;
using UnityEngine;

public class GlitchNumber : MonoBehaviour
{
    [SerializeField] private TMP_Text numberText;

    [Header("Glitch")]
    [SerializeField] private float glitchDuration = 0.4f;
    [SerializeField] private float updateRate = 0.03f;

    [Header("Shake")]
    [SerializeField] private float shakeAmount = 3f;

    private Vector3 originalPos;

    private void Awake()
    {
        if (numberText == null)
            numberText = GetComponent<TMP_Text>();

        originalPos = numberText.rectTransform.localPosition;
    }

    public void SetNumberInstant(int value)
    {
        numberText.text = value.ToString();
    }

    public void GlitchToNumber(int value)
    {
        StopAllCoroutines();
        StartCoroutine(GlitchRoutine(value));
    }

    private IEnumerator GlitchRoutine(int finalValue)
    {
        float timer = 0f;

        while (timer < glitchDuration)
        {
            timer += updateRate;

            numberText.text = Random.Range(0, 10).ToString();

            numberText.rectTransform.localPosition =
                originalPos +
                (Vector3)Random.insideUnitCircle * shakeAmount;

            yield return new WaitForSecondsRealtime(updateRate);
        }

        numberText.rectTransform.localPosition = originalPos;
        numberText.text = finalValue.ToString();
    }
}