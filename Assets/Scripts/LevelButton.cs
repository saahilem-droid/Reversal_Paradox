using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] private int levelNumber = 1;

    [SerializeField] private string sceneName;

    [Header("References")]
    [SerializeField] private Button button;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private TMP_Text levelText;

    public int LevelNumber => levelNumber;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (levelText != null)
            levelText.text = levelNumber.ToString();

        button.onClick.AddListener(OnClick);
    }

    public void SetUnlocked(bool unlocked)
    {
        button.interactable = unlocked;

        if (lockIcon != null)
            lockIcon.SetActive(!unlocked);
    }

    private void OnClick()
    {
        if (!button.interactable)
            return;

        FindFirstObjectByType<UIManager>().LoadScene(sceneName);
    }
}