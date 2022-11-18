using UnityEngine;
using UnityEngine.EventSystems;

public class UIDetailedCard : MonoBehaviour, IPointerExitHandler
{
    public void OnPointerExit(PointerEventData eventData)
    {
        UIMainPanel.Instance.HideDetailedCard();
    }
}
