using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    [Header("Recording")]
    [SerializeField] private int recordingSlots = 7;

    private List<Vector2Int> recordedMoves = new List<Vector2Int>();

    public bool IsRecordingFinished => recordedMoves.Count >= recordingSlots;

    public IReadOnlyList<Vector2Int> RecordedMoves => recordedMoves;

    public int RecordingSlots => recordingSlots;

    public int CurrentRecordedMoves => recordedMoves.Count;

    private void Start()
{
    HUDManager.Instance.InitializeCounter(recordingSlots);
}

    public void RecordMove(Vector2Int direction)
    {
        if (IsRecordingFinished)
            return;

        recordedMoves.Add(direction);

        HUDManager.Instance.AddArrow(direction);

HUDManager.Instance.SetCounter(
    recordingSlots - recordedMoves.Count);

        Debug.Log($"Recorded Move: {direction}");

        if (IsRecordingFinished)
{
    Debug.Log("Recording Complete!");

    HUDManager.Instance.FlipHourglass();

    HUDManager.Instance.GlitchCounter(recordingSlots);
}
    }

    public void ResetRecording()
{
    recordedMoves.Clear();

    HUDManager.Instance.ClearArrows();
    HUDManager.Instance.SetCounter(recordingSlots);
}
}