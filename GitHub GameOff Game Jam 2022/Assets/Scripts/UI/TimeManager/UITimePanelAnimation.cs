using FMODUnity;
using UnityEngine;

public class UITimePanelAnimation : MonoBehaviour
{
    [Header("Season End Animation Sounds")]
    [SerializeField] private EventReference endTurnPanelSlideEventName;
    [SerializeField] private EventReference endSeasonNotificationEventName;
    [SerializeField] private EventReference endTurnSeasonEventName;

    public void PlayEndTurnPanelSlideEventName() => RuntimeManager.PlayOneShot(endTurnPanelSlideEventName);
    public void PlayEndSeasonNotificationEventName() => RuntimeManager.PlayOneShot(endSeasonNotificationEventName);
    public void PlayEndTurnSeasonEventName() => RuntimeManager.PlayOneShot(endTurnSeasonEventName);

}
