using System.Collections;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] protected float moveSpeed = 6f;
    [SerializeField] protected float horizontalMoveDistance = 1.45f;

    [Header("Visuals")]
[SerializeField] private Transform graphicsTransform;
[SerializeField] private SpriteRenderer spriteRenderer;
[SerializeField] private float moveTiltAngle = 8f;
[SerializeField] private float tiltSpeed = 12f;

private float targetRotation;

    

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

   

    protected bool MoveLeft()
    {
        if (!CanMove())
    return false;
        Vector2 target =
            rb.position +
            Vector2.left * horizontalMoveDistance;

        StartCoroutine(
            MoveRoutine(
                target,
                null));
                return true;
    }

    protected bool MoveRight()
    {
        if (!CanMove())
            return false;   

        Vector2 target =
            rb.position +
            Vector2.right * horizontalMoveDistance;

        StartCoroutine(
            MoveRoutine(
                target,
                null));
                return true;
    }

    protected bool CanMove()
    {
        if (isMoving)
            return false;

        if (!isGrounded)
            return false;

        return true;
    }

    protected void FaceDirection(int direction)
{
    if (direction == 0)
        return;
Debug.Log("Facing: " + direction);
    spriteRenderer.flipX = direction < 0;

    targetRotation = direction < 0
        ? moveTiltAngle
        : -moveTiltAngle;
}

    protected bool ClimbUp()
{
    if (!CanMove())
        return false;

    if (currentPlatform == null)
        return false;

    if (currentPlatform.ladderUp == null)
        return false;

    Platform targetPlatform = currentPlatform.ladderUp.topPlatform;
    Collider2D targetPlatformCollider = targetPlatform.GetComponent<Collider2D>();

if (targetPlatformCollider != null)
{
    StartCoroutine(
        IgnorePlatformTemporarily(
            targetPlatformCollider,
            0.5f));
}

    StartCoroutine(
    MoveRoutine(
        targetPlatform.centerPoint.position,
        targetPlatform,
        true));
        return true;
}

protected bool ClimbDown()
{
    Debug.Log("ClimbDown Called");

    if (!CanMove())
        return false;

    if (currentPlatform == null)
        return false;

    Debug.Log("Current Platform = " + currentPlatform.name);

Debug.Log("Ladder Down = " + currentPlatform.ladderDown);

if (currentPlatform.ladderDown == null)
{
    Debug.Log("No ladder down");
    return false;
}

Debug.Log("Bottom Platform = " + currentPlatform.ladderDown.bottomPlatform.name);

    Platform targetPlatform = currentPlatform.ladderDown.bottomPlatform;

Collider2D currentPlatformCollider = currentPlatform.GetComponent<Collider2D>();

if (currentPlatformCollider != null)
{
    StartCoroutine(
        IgnorePlatformTemporarily(
            currentPlatformCollider,
            0.5f));
}

    StartCoroutine(
    MoveRoutine(
        targetPlatform.centerPoint.position,
        targetPlatform,
        true));
        return true;
}

protected bool MoveHorizontal(int direction)
{
    if (invertControls)
        direction *= -1;

    FaceDirection(direction);

    if (direction < 0)
        return MoveLeft();
    else
        return MoveRight();
}

protected bool MoveVertical(int direction)
{
    if (invertControls)
        direction *= -1;

    if (direction > 0)
        return ClimbUp();
    else
        return ClimbDown();
}


    protected void UpdateVisuals()
{
    
    float current = graphicsTransform.localEulerAngles.z;

    if (current > 180f)
        current -= 360f;

    float angle = Mathf.LerpAngle(
        current,
        targetRotation,
        Time.deltaTime * tiltSpeed);

    graphicsTransform.localRotation = Quaternion.Euler(0, 0, angle);
}





private void LateUpdate()
{
    UpdateVisuals();
    CheckRestartColliders();
}

protected IEnumerator MoveRoutine(
    Vector3 targetPosition,
    Platform targetPlatform,
    bool snapToPlatform = false)
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
{
    currentPlatform = targetPlatform;
    currentPlatform.occupant = this;

    if (snapToPlatform)
    {
        rb.position = currentPlatform.centerPoint.position;
    }
}

isMoving = false;

OnMovementFinished();

}

private IEnumerator IgnorePlatformTemporarily(Collider2D platformCollider, float duration)
{
    Collider2D myCollider = GetComponent<Collider2D>();

    Physics2D.IgnoreCollision(myCollider, platformCollider, true);

    yield return new WaitForSeconds(duration);

    Physics2D.IgnoreCollision(myCollider, platformCollider, false);
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
    targetRotation = 0f;
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



    rb.linearVelocity = Vector2.zero;

// Only snap when landing on a DIFFERENT platform.
if (currentPlatform != platform)
{
    currentPlatform = platform;
    currentPlatform.occupant = this;

    //rb.position = platform.centerPoint.position;
}

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

    rb.position = platform.centerPoint.position;

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
