using System.Collections.Generic;
using UnityEngine;

public class CloneController : CharacterMovement
{
    
    public void SetInvertControls(bool invert)
{
    invertControls = invert;
}
    private IReadOnlyList<Vector2Int> recordedMoves;

    private int currentMoveIndex;

    protected override void Start()
{
    base.Start();

    Collider2D myCollider = GetComponent<Collider2D>();

    CloneController[] clones = FindObjectsOfType<CloneController>();

    foreach (CloneController clone in clones)
    {
        if (clone == this)
            continue;

        Physics2D.IgnoreCollision(
            myCollider,
            clone.GetComponent<Collider2D>(),
            true);
    }
}



    public void Initialize(
    IReadOnlyList<Vector2Int> moves,
    GameManager.CloneSettings settings)
{
    recordedMoves = moves;
    currentMoveIndex = 0;

    SetInvertControls(settings.invertControls);

    startingPlatform = settings.startingPlatform;

    SetRestartColliders(settings.restartColliders);

    ForcePlatform(startingPlatform);
}

    public void StepForward()
    {
        if (IsMoving)
            return;

        if (recordedMoves == null)
            return;

        if (currentMoveIndex >= recordedMoves.Count)
            return;

        Vector2Int move = recordedMoves[currentMoveIndex];

        currentMoveIndex++;

        if (move == Vector2Int.left)
{
    MoveHorizontal(-1);
}
else if (move == Vector2Int.right)
{
    MoveHorizontal(1);
}
else if (move == Vector2Int.up)
{
    MoveVertical(1);
}
else if (move == Vector2Int.down)
{
    MoveVertical(-1);
}
    }

    protected override void OnMovementFinished()
    {
        base.OnMovementFinished();
        // Clone doesn't notify the GameManager.
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.TriggerParadox();
        }
    }
}