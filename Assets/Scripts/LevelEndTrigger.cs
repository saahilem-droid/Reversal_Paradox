using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

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

    UIManager.Instance.ShowLevelComplete();
}
}