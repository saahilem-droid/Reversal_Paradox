using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Vector3 openOffset;
    [SerializeField] private float moveSpeed = 3f;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    private Coroutine moveRoutine;

    private void Awake()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + openOffset;
    }

    public void OpenDoor()
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(MoveDoor(openPosition));
    }

    public void CloseDoor()
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(MoveDoor(closedPosition));
    }

    private IEnumerator MoveDoor(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                moveSpeed * Time.deltaTime);

            yield return null;
        }

        transform.position = target;
    }
}