using TMPro;
using UnityEngine;

public class UIDiscardPanel : InspectorReferenceChecker
{
    [SerializeField] private UICardPanel[] handcardPrefabs;
    [SerializeField] private UICardPanel cardToDiscard;
    [SerializeField] private TMP_Text cardToDiscardTitle;

    /// <summary>
    /// After this GameObject has been activated, sets up the UI components
    /// of the panel (the title and card to be displayed)
    /// </summary>
    /// <param name="consideredCard">Which card is considered to be discarded?</param>
    public void DisplaySelf(ActionCardSO consideredCard)
    {
        cardToDiscardTitle.text = $"Discard <i>{consideredCard.cardTitle}</i>?";
        cardToDiscard.CardToDisplay = consideredCard;
        cardToDiscard.Render();
    }

    /// <summary>
    /// When the player clicks on the confirm discard button,
    /// it well tell the card manager to perform the logic behind that action
    /// </summary>
    public void Discard() => CardManager.Instance.ConfirmDiscard();

    /// <summary>
    /// Reorders the sibling index and deactivates the card
    /// </summary>
    /// <param name="cardIndex">The index of the discarded card</param>
    public void HandleUIDiscard(int cardIndex)
    {
        handcardPrefabs[cardIndex].transform.SetAsLastSibling();
        handcardPrefabs[cardIndex].gameObject.SetActive(false);
    }

    protected override object[] CheckForMissingReferences() => new object[]
    {
        handcardPrefabs, cardToDiscard, cardToDiscardTitle
    };
}
