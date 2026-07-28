using System.Collections;
using UnityEngine;

public enum DoorMode
{
    Move,
    EnableDisable
}

public class DoorController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Vector3 openOffset;
    [SerializeField] private float moveSpeed = 3f;

    [Header("Door Settings")]
[SerializeField] private DoorMode doorMode = DoorMode.Move;

[Header("Enable / Disable")]
[SerializeField] private Platform[] platformsToToggle;
[SerializeField] private bool enableWhenOpen = false;

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
    if (doorMode == DoorMode.Move)
    {
        StopAllCoroutines();
        StartCoroutine(MoveDoor(closedPosition + openOffset));
    }
    else
    {
        foreach (Platform platform in platformsToToggle)
        {
            if (platform == null)
                continue;

            if (platform.occupant != null)
            {
                platform.occupant.ForceFall();
            }

            platform.gameObject.SetActive(enableWhenOpen);
        }
    }
}

    public void CloseDoor()
{
    if (doorMode == DoorMode.Move)
    {
        StopAllCoroutines();
        StartCoroutine(MoveDoor(closedPosition));
    }
    else
    {
        foreach (Platform platform in platformsToToggle)
        {
            if (platform == null)
                continue;

            if (platform.occupant != null)
            {
                platform.occupant.ForceFall();
            }

            platform.gameObject.SetActive(!enableWhenOpen);
        }
    }
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