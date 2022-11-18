using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Equips UI elements with the ability to play animations depending
/// on whether the user is hovering over it or not
/// </summary>
/// <author>Gino</author>
public class UIHoverElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Animator hoverAnimator;
    [SerializeField] private string hoverTriggerName;
    [SerializeField] private string dehoverTriggerName;

    public void OnPointerEnter(PointerEventData e) => hoverAnimator.SetTrigger(hoverTriggerName);
    public void OnPointerExit(PointerEventData e) => hoverAnimator.SetTrigger(dehoverTriggerName);
}
