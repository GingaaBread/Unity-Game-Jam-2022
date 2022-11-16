using UnityEngine;

/// <summary>
/// Attached to the discard animation container
/// to be able to use animation events
/// </summary>
/// <author>Gino</author>
public class UIDiscardAnimation : MonoBehaviour
{
    /// <summary>
    /// Played when the discard animation has finished.
    /// Informs the CardManager about having confirmed the discard choice.
    /// </summary>
    public void ConfirmDiscardInCardManager() => CardManager.Instance.ConfirmDiscard();
}
