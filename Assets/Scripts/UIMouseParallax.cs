using UnityEngine;

public class UIMouseParallax : MonoBehaviour
{
    [SerializeField] private RectTransform target;

    [Header("Movement")]
    [SerializeField] private float moveAmount = 20f;
    [SerializeField] private float smoothSpeed = 6f;

    private Vector2 initialPosition;
    private Vector2 targetPosition;

    private void Awake()
    {
        if (target == null)
            target = GetComponent<RectTransform>();

        initialPosition = target.anchoredPosition;
    }

    private void Update()
    {
        Vector2 mouse = Input.mousePosition;

        float x = (mouse.x / Screen.width - 0.5f) * 2f;
        float y = (mouse.y / Screen.height - 0.5f) * 2f;

        targetPosition = initialPosition + new Vector2(x, y) * moveAmount;

        target.anchoredPosition = Vector2.Lerp(
            target.anchoredPosition,
            targetPosition,
            smoothSpeed * Time.deltaTime);
    }
}