using TMPro;
using UIManagement;
using UnityEngine;

public class UIDiscardPanel : InspectorReferenceChecker
{
    [SerializeField] private TMP_Text cardToDiscardTitle;
    [SerializeField] private Animator discardAnimator;
    [SerializeField] private Animator scaleAnimator;

    /// <summary>
    /// After this GameObject has been activated, sets up the UI components
    /// of the panel (the title and card to be displayed)
    /// </summary>
    /// <param name="consideredCard">Which card is considered to be discarded?</param>
    public void DisplaySelf(ActionCardSO consideredCard)
    {
        cardToDiscardTitle.text = $"Discard <i>{consideredCard.cardTitle}</i>?";
        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// When the player clicks on the confirm discard button,
    /// it plays the animation (which will inform the CardManager after that)
    /// </summary>
    public void Discard()
    {
        discardAnimator.Play("UIDiscardSuccess");
        UIMainPanel.Instance.LockAllHandcards();
    }

    /// <summary>
    /// Plays the neutral pop out animation which will initiate the next element
    /// </summary>
    public void Cancel() => scaleAnimator.Play("PopOutNeutral");

    /// <summary>
    /// Currently only informs the MainPanel and disables self
    /// </summary>
    /// <param name="cardIndex">The index of the discarded card</param>
    public void HandleUIDiscard(int cardIndex)
    {
        UIMainPanel.Instance.DestroyCard(cardIndex);
        UIMainPanel.Instance.HideDetailedCard();
        gameObject.SetActive(false);
    }

    protected override object[] CheckForMissingReferences() => new object[]
    {
        cardToDiscardTitle, discardAnimator
    };
}
