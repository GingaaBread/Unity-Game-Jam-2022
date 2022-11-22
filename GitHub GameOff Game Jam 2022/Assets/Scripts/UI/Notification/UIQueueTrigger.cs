namespace UIManagement
{
    using UnityEngine;

    /// <summary>
    /// This class is used to equip animated or button UI elements with the ability to initiate
    /// the next panel in the FeedbackPanelManager as soon as their animation ends or the click
    /// event is emitted
    /// </summary>
    /// <author>Gino</author>
    public class UIQueueTrigger : MonoBehaviour
    {
        /// <summary>
        /// This method should be called on the animation or click event
        /// </summary>
        public void TriggerNextElement() => FeedbackPanelManager.Instance.InitiateNextPanel();
    }
}
