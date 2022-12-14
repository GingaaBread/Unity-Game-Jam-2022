using UnityEngine;
using UnityEngine.EventSystems;

public class UIDetailedCard : MonoBehaviour, IPointerExitHandler
{
    [HideInInspector] public int handcardIndex;

    private void Start()
    {
        UIMainPanel.Instance.HideDetailedCard();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!CardPlayManager.Instance.PlayIsInProgress())
            UIMainPanel.Instance.HideDetailedCard();
    }

    public void OpenDiscardPanel() => CardManager.Instance.ConsiderCardDiscard(handcardIndex);

    private void Update()
    {
        if (Input.GetButtonDown("OpenDetailPanel") && !UIMainPanel.Instance.InDetailCardPanel())
        {
            UIMainPanel.Instance.HideDetailedCard();
            UIMainPanel.Instance.DisplayDetailCardPanel(CardManager.Instance.GetHandcardAt(handcardIndex), handcardIndex);
        }
    }
}
