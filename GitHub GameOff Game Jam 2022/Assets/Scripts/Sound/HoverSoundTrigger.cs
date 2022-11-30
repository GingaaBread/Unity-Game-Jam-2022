using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverSoundTrigger : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private EventReference hoverSound;

    public void OnPointerEnter(PointerEventData eventData)
    {
        RuntimeManager.PlayOneShot(hoverSound);
    }
}
