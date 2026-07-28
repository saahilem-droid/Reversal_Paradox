using UnityEngine;
using System.Collections.Generic;

public enum SwitchMode
{
    Hold,
    Toggle
}

public class PressureSwitch : MonoBehaviour



{
    [Header("Switch Settings")]
[SerializeField] private SwitchMode switchMode = SwitchMode.Hold;
    [SerializeField] private List<DoorController> connectedDoors = new List<DoorController>();

    [Header("Visual")]
[SerializeField] private Transform switchCap;
[SerializeField] private float pressDistance = 0.08f;
[SerializeField] private float pressSpeed = 8f;

private Vector3 releasedPos;
private Vector3 pressedPos;

private bool isToggled;
private int objectsOnSwitch;    

private void Start()
{
    releasedPos = switchCap.localPosition;
    pressedPos = releasedPos + Vector3.down * pressDistance;
}

private void Update()
{
    if (switchCap == null)
        return;

    Vector3 target;

    if (switchMode == SwitchMode.Hold)
        target = objectsOnSwitch > 0 ? pressedPos : releasedPos;
    else
        target = isToggled ? pressedPos : releasedPos;

    switchCap.localPosition = Vector3.Lerp(
        switchCap.localPosition,
        target,
        pressSpeed * Time.deltaTime);
}
    

    private void OnTriggerEnter2D(Collider2D other)
{
    if (!other.CompareTag("Player") &&
        !other.CompareTag("Clone"))
        return;

    if (switchMode == SwitchMode.Hold)
    {
        objectsOnSwitch++;
        foreach (DoorController door in connectedDoors)
{
    if (door != null)
        door.OpenDoor();
}
    }
    else
{
    isToggled = !isToggled;

    foreach (DoorController door in connectedDoors)
    {
        if (door == null)
            continue;

        if (isToggled)
            door.OpenDoor();
        else
            door.CloseDoor();
    }
}
}

    private void OnTriggerExit2D(Collider2D other)
{
    if (!other.CompareTag("Player") &&
        !other.CompareTag("Clone"))
        return;

    if (switchMode == SwitchMode.Hold)
    {
        objectsOnSwitch--;

        if (objectsOnSwitch <= 0)
        {
            objectsOnSwitch = 0;
            foreach (DoorController door in connectedDoors)
{
    if (door != null)
        door.CloseDoor();
}
        }
    }

    // Toggle mode does nothing when leaving the switch.
}
    }
