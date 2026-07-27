using UnityEngine;

public class Ladder : MonoBehaviour
{
    public Platform topPlatform;
    public Platform bottomPlatform;

    private void Awake()
    {
        if (topPlatform != null)
            topPlatform.ladderDown = this;

        if (bottomPlatform != null)
            bottomPlatform.ladderUp = this;
    }
}