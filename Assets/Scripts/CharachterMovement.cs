using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] protected float moveSpeed = 6f;
    [SerializeField] protected float horizontalMoveDistance = 1.45f;

    [Header("References")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Platform startingPlatform;

[Header("Level Mechanics")]
[SerializeField] protected bool invertControls = false;
    protected Platform currentPlatform;

    protected bool isMoving;
    protected bool isGrounded = true;

    [Header("Level Boundaries")]
[SerializeField] private Collider2D[] restartColliders;

    protected virtual void Start()
    {
        ForcePlatform(startingPlatform);
        currentPlatform = startingPlatform;

        if (currentPlatform != null)
        {
            transform.position = currentPlatform.centerPoint.position;
        }
    }

    protected void MoveLeft()
    {
        if (!CanMove())
            return;

        Vector3 target =
            transform.position +
            Vector3.left * horizontalMoveDistance;

        StartCoroutine(
            MoveRoutine(
                target,
                null));
    }

    protected void MoveRight()
    {
        if (!CanMove())
            return;

        Vector3 target =
            transform.position +
            Vector3.right * horizontalMoveDistance;

        StartCoroutine(
            MoveRoutine(
                target,
                null));
    }

    protected bool CanMove()
    {
        if (isMoving)
            return false;

        if (!isGrounded)
            return false;

        return true;
    }

    protected void ClimbUp()
{
    if (!CanMove())
        return;

    if (currentPlatform == null)
        return;

    if (currentPlatform.ladderUp == null)
        return;

    Platform targetPlatform = currentPlatform.ladderUp.topPlatform;

    StartCoroutine(
        MoveRoutine(
            targetPlatform.centerPoint.position,
            targetPlatform));
}

protected void ClimbDown()
{
    if (!CanMove())
        return;

    if (currentPlatform == null)
        return;

    if (currentPlatform.ladderDown == null)
        return;

    Platform targetPlatform = currentPlatform.ladderDown.bottomPlatform;

    StartCoroutine(
        MoveRoutine(
            targetPlatform.centerPoint.position,
            targetPlatform));
}

protected void MoveHorizontal(int direction)
{
    if (invertControls)
        direction *= -1;

    if (direction < 0)
        MoveLeft();
    else
        MoveRight();
}

protected void MoveVertical(int direction)
{
    if (invertControls)
        direction *= -1;

    if (direction > 0)
        ClimbUp();
    else
        ClimbDown();
}

private void LateUpdate()
{
    CheckRestartColliders();
}

protected IEnumerator MoveRoutine(Vector3 targetPosition, Platform targetPlatform)
{
    isMoving = true;

    while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
    {
        rb.MovePosition(
            Vector2.MoveTowards(
                rb.position,
                targetPosition,
                moveSpeed * Time.deltaTime));

        yield return new WaitForFixedUpdate();
    }

    rb.MovePosition(targetPosition);
    transform.position = targetPosition;

    if (targetPlatform != null)
        currentPlatform = targetPlatform;

    isMoving = false;

    OnMovementFinished();
}
private void CheckRestartColliders()
{
    if (restartColliders == null)
        return;

    Collider2D myCollider = GetComponent<Collider2D>();

    foreach (Collider2D col in restartColliders)
    {
        if (col == null)
            continue;

        if (myCollider.IsTouching(col))
        {
            GameManager.Instance.TriggerParadox();
            return;
        }
    }
}

protected virtual void OnMovementFinished()
{
    // Player overrides this to notify GameManager.
    // Clone doesn't need to do anything here.
}

protected virtual void OnCollisionEnter2D(Collision2D collision)
{
    Platform platform = collision.collider.GetComponent<Platform>();

    if (platform != null)
    {
        isGrounded = true;
    }

    // Trigger paradox only when Player and Clone collide
    if ((CompareTag("Player") && collision.collider.CompareTag("Clone")) ||
        (CompareTag("Clone") && collision.collider.CompareTag("Player")))
    {
        GameManager.Instance.TriggerParadox();
    }
}

protected virtual void OnCollisionStay2D(Collision2D collision)
{
    Platform platform = collision.collider.GetComponent<Platform>();

    if (platform == null)
        return;

    // Wait until almost stopped falling
    if (Mathf.Abs(rb.linearVelocity.y) > 0.05f)
        return;

    // Already standing on this platform
    if (currentPlatform == platform)
    {
        isGrounded = true;
        return;
    }

    currentPlatform = platform;

    rb.linearVelocity = Vector2.zero;

    transform.position = platform.centerPoint.position;

    isMoving = false;

    isGrounded = true;
}

protected virtual void OnCollisionExit2D(Collision2D collision)
{
    if (collision.collider.GetComponent<Platform>() != null)
    {
        isGrounded = false;
    }
}

protected void ForcePlatform(Platform platform)
{
    currentPlatform = platform;

    transform.position = platform.centerPoint.position;

    rb.linearVelocity = Vector2.zero;

    isGrounded = true;

    isMoving = false;
}

protected Platform CurrentPlatform => currentPlatform;

protected bool IsMoving => isMoving;

protected bool IsGrounded => isGrounded;
}
