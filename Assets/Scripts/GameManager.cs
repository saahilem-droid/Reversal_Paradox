using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    [SerializeField] private Recorder recorder;
    [SerializeField] private CloneController clonePrefab;
[SerializeField] private CloneSettings[] clones;

    [System.Serializable]
public class CloneSettings
{
    public Transform spawnPoint;
    public bool invertControls;
    public Platform startingPlatform;

    // Assign any level-specific colliders here
    public Collider2D[] restartColliders;
}

    private List<CloneController> currentClones = new List<CloneController>();

    public bool CloneSpawned => currentClones.Count > 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public bool GameplayEnabled { get; private set; } = true;

public void SetGameplayEnabled(bool enabled)
{
    GameplayEnabled = enabled;
}

    public void PlayerMoved(Vector2Int direction)
    {
        // Record the first N moves
        if (!recorder.IsRecordingFinished)
        {
            recorder.RecordMove(direction);

            if (recorder.IsRecordingFinished)
            {
                SpawnClones();
            }

            return;
        }

        // Recording finished
        if (currentClones != null)
        {
            foreach (CloneController clone in currentClones)
{
    clone.StepForward();
}
        }
    }

    private void SpawnClones()
{
    currentClones.Clear();

    foreach (CloneSettings settings in clones)
{
    CloneController clone = Instantiate(
        clonePrefab,
        settings.spawnPoint.position,
        Quaternion.identity);

    clone.Initialize(
        recorder.RecordedMoves,
        settings);

    currentClones.Add(clone);
}

// Change background after clone(s) have spawned
if (BackgroundManager.Instance != null)
{
    BackgroundManager.Instance.ShowCloneBackground();
}


    HUDManager.Instance.FlipHourglass();

HUDManager.Instance.SetCounter(recorder.RecordedMoves.Count);

PlayerMovement player = FindFirstObjectByType<PlayerMovement>();

if (player != null && player.InvertWhenCloneSpawns)
{
   player.EnablePendingInvert();
}

AudioManager audio = FindFirstObjectByType<AudioManager>();

if (audio != null)
{
    audio.PlayCloneSpawn();
}

MusicManager music = FindFirstObjectByType<MusicManager>();

if (music != null)
{
    music.PlayCloneMusic();
}
}

private bool gameOver;

public void PlayerDied()
{
    if (gameOver)
        return;

    gameOver = true;

    SetGameplayEnabled(false);
    AudioManager audio = FindFirstObjectByType<AudioManager>();

if (audio != null)
{
    audio.PlayGameOver();
}

    UIManager ui = FindFirstObjectByType<UIManager>();

if (ui != null)
{
    ui.ShowGameOver();
}


}



   public void TriggerParadox()
{
    PlayerDied();
}
}