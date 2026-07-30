using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonAudio : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        FindFirstObjectByType<AudioManager>()?.PlayButtonHover();
    }
}