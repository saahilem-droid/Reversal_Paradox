using UnityEngine;

public class Platform : MonoBehaviour
{
    [Header("Player Standing Position")]
    public Transform centerPoint;

    [Header("Ladders")]
    public Ladder ladderUp;
    public Ladder ladderDown;

    [HideInInspector]
public CharacterMovement occupant;
}