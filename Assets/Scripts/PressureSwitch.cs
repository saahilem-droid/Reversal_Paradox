using UnityEngine;

public class PressureSwitch : MonoBehaviour
{
    [SerializeField] private DoorController connectedDoor;

    private int objectsOnSwitch = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Clone"))
        {
            objectsOnSwitch++;
            connectedDoor.OpenDoor();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Clone"))
        {
            objectsOnSwitch--;

            if (objectsOnSwitch <= 0)
            {
                objectsOnSwitch = 0;
                connectedDoor.CloseDoor();
            }
        }
    }
}