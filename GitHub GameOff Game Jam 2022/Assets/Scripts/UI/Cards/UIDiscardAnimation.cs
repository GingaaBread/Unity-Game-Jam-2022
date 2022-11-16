using UIManagement;
using UnityEngine;

/// <summary>
/// Attached to the two discard animation containers
/// to be able to use animation events
/// </summary>
/// <author>Gino</author>
public class UIDiscardAnimation : MonoBehaviour
{
    [SerializeField] private Animator cardScaleAnimator;

    /// <summary>
    /// Played when the discard animation has finished.
    /// Plays the pop-out animation of the card panel
    /// </summary>
    public void PlayPanelPopOutAnimation() => cardScaleAnimator.Play("PopOut");

    /// <summary>
    /// Played when the pop-out animation has finished.
    /// Informs the CardManager about having confirmed the discard choice.
    /// Also tells the FeedbackManager to carry on with the next queue elements
    /// if there are ones left.
    /// </summary>
    public void ConfirmDiscardInCardManager()
    {
        CardManager.Instance.ConfirmDiscard();
        FeedbackPanelManager.Instance.InitiateNextPanel();
    }
}
