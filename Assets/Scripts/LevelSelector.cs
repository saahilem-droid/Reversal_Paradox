using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private LevelButton[] levelButtons;

    private void Start()
    {
        RefreshButtons();
    }

    public void RefreshButtons()
    {
        if (LevelProgressManager.Instance == null)
        {
            Debug.LogError("LevelProgressManager not found!");
            return;
        }

        foreach (LevelButton button in levelButtons)
        {
            bool unlocked = LevelProgressManager.Instance.IsUnlocked(button.LevelNumber);
            button.SetUnlocked(unlocked);
        }
    }
}