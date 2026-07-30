using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager Instance;

    [Header("Background")]
    [SerializeField] private SpriteRenderer backgroundRenderer;

    [Header("Sprites")]
    [SerializeField] private Sprite normalBackground;
    [SerializeField] private Sprite cloneBackground;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (backgroundRenderer == null)
            backgroundRenderer = GetComponent<SpriteRenderer>();

        backgroundRenderer.sprite = normalBackground;
    }

    public void ShowNormalBackground()
    {
        backgroundRenderer.sprite = normalBackground;
    }

    public void ShowCloneBackground()
    {
        Debug.Log("Changing to clone background");
        backgroundRenderer.sprite = cloneBackground;
    }
}