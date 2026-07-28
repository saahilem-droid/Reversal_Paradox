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

    [SerializeField]
protected Collider2D[] restartColliders;

    public void SetRestartColliders(Collider2D[] colliders)
{
    restartColliders = colliders;
}



protected virtual void Start()
{
    

    ForcePlatform(startingPlatform);

    currentPlatform = startingPlatform;

    if (currentPlatform != null)
    {
        rb.position = currentPlatform.centerPoint.position;
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
    Debug.Log("ClimbDown Called");

    if (!CanMove())
        return;

    if (currentPlatform == null)
        return;

    Debug.Log("Current Platform = " + currentPlatform.name);

Debug.Log("Ladder Down = " + currentPlatform.ladderDown);

if (currentPlatform.ladderDown == null)
{
    Debug.Log("No ladder down");
    return;
}

Debug.Log("Bottom Platform = " + currentPlatform.ladderDown.bottomPlatform.name);

    Platform targetPlatform = currentPlatform.ladderDown.bottomPlatform;

Collider2D currentPlatformCollider = currentPlatform.GetComponent<Collider2D>();

if (currentPlatformCollider != null)
{
    StartCoroutine(
        DisablePlatformTemporarily(
            currentPlatformCollider,
            0.5f));
}

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

    Vector2 target = targetPosition;

    while (Vector2.Distance(rb.position, target) > 0.01f)
    {
        rb.MovePosition(
            Vector2.MoveTowards(
                rb.position,
                target,
                moveSpeed * Time.fixedDeltaTime));

        yield return new WaitForFixedUpdate();
    }

    rb.position = target;

    if (targetPlatform != null)
        currentPlatform = targetPlatform;


    isMoving = false;

    OnMovementFinished();
}

private IEnumerator DisablePlatformTemporarily(Collider2D platformCollider, float duration)
{
    platformCollider.enabled = false;

    yield return new WaitForSeconds(duration);

    platformCollider.enabled = true;
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
            GameManager.Instance.PlayerDied();
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
        GameManager.Instance.PlayerDied();
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

    if (currentPlatform != null &&
    currentPlatform != platform &&
    currentPlatform.occupant == this)
{
    currentPlatform.occupant = null;
}

currentPlatform = platform;
currentPlatform.occupant = this;

    rb.linearVelocity = Vector2.zero;

    transform.position = platform.centerPoint.position;

    isMoving = false;

    isGrounded = true;
}

protected virtual void OnCollisionExit2D(Collision2D collision)
{
    Platform platform = collision.collider.GetComponent<Platform>();

    if (platform == null)
        return;

    if (platform.occupant == this)
        platform.occupant = null;

    isGrounded = false;
}



protected void ForcePlatform(Platform platform)
{
    if (currentPlatform != null &&
        currentPlatform.occupant == this)
    {
        currentPlatform.occupant = null;
    }

    currentPlatform = platform;

    currentPlatform.occupant = this;

    transform.position = platform.centerPoint.position;

    rb.linearVelocity = Vector2.zero;

    isGrounded = true;
    isMoving = false;
}

public void ForceFall()
{
    if (currentPlatform != null &&
        currentPlatform.occupant == this)
    {
        currentPlatform.occupant = null;
    }

    currentPlatform = null;

    isGrounded = false;

    rb.linearVelocity = new Vector2(0f, -0.1f);
}
protected Platform CurrentPlatform => currentPlatform;

protected bool IsMoving => isMoving;

protected bool IsGrounded => isGrounded;
}
