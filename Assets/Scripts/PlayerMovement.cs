using UnityEngine;

public class PlayerMovement : CharacterMovement
{
    [SerializeField] private bool invertWhenCloneSpawns = false;

    private bool pendingInvert = false;

    public bool InvertWhenCloneSpawns => invertWhenCloneSpawns;

    public void EnablePendingInvert()
    {
        pendingInvert = true;
    }

    public void SetInvertControls(bool invert)
    {
        invertControls = invert;
    }

    private void Update()
    {
        if (!GameManager.Instance.GameplayEnabled)
            return;

        if (IsMoving)
            return;

        // LEFT
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector2Int recordedMove = invertControls ? Vector2Int.right : Vector2Int.left;

            if (MoveHorizontal(-1))
            {
                GameManager.Instance.PlayerMoved(recordedMove);
            }

            return;
        }

        // RIGHT
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector2Int recordedMove = invertControls ? Vector2Int.left : Vector2Int.right;

            if (MoveHorizontal(1))
            {
                GameManager.Instance.PlayerMoved(recordedMove);
            }

            return;
        }

        // UP
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector2Int recordedMove = invertControls ? Vector2Int.down : Vector2Int.up;

            if (MoveVertical(1))
            {
                GameManager.Instance.PlayerMoved(recordedMove);
            }

            return;
        }

        // DOWN
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector2Int recordedMove = invertControls ? Vector2Int.up : Vector2Int.down;

            if (MoveVertical(-1))
            {
                GameManager.Instance.PlayerMoved(recordedMove);
            }

            return;
        }
    }

    protected override void OnMovementFinished()
    {
        base.OnMovementFinished();

        if (pendingInvert)
        {
            invertControls = true;
            pendingInvert = false;
        }
    }
}