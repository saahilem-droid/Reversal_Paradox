using UnityEngine;

public class PlayerMovement : CharacterMovement
{
    private Vector2Int lastMove;

    private void Update()
    {
        if (IsMoving)
            return;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            lastMove = invertControls ? Vector2Int.right : Vector2Int.left;
MoveHorizontal(-1);
            return;
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            lastMove = invertControls ? Vector2Int.left : Vector2Int.right;
MoveHorizontal(1);
            return;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            lastMove = invertControls ? Vector2Int.down : Vector2Int.up;
MoveVertical(1);
            return;
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            lastMove = invertControls ? Vector2Int.up : Vector2Int.down;
MoveVertical(-1);
            return;
        }
    }

    protected override void OnMovementFinished()
    {
        base.OnMovementFinished();

        GameManager.Instance.PlayerMoved(lastMove);
    }
}