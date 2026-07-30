using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private int currentLevel = 1;

    private bool levelCompleted;

    private void OnTriggerEnter2D(Collider2D other)
{
    if (levelCompleted)
        return;

    if (!other.CompareTag(playerTag))
        return;

    levelCompleted = true;

    StartCoroutine(LevelCompleteRoutine());
}

private System.Collections.IEnumerator LevelCompleteRoutine()
{
    CharacterMovement movement =
        FindFirstObjectByType<CharacterMovement>();

    if (movement != null)
        movement.enabled = false;

    yield return new WaitForSeconds(0.5f);

GameManager.Instance.SetGameplayEnabled(false);
LevelProgressManager.Instance?.CompleteLevel(currentLevel);
AudioManager audio = FindFirstObjectByType<AudioManager>();

if (audio != null)
{
    audio.PlayLevelComplete();
}

    UIManager ui = FindFirstObjectByType<UIManager>();

if (ui != null)
{
    ui.ShowLevelComplete();
}
}
}