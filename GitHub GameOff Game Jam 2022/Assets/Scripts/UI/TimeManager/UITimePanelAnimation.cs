using FMODUnity;
using UnityEngine;

public class UITimePanelAnimation : MonoBehaviour
{
    [Header("Season End Animation Sounds")]
    [SerializeField] private EventReference endTurnPanelSlideEventName;
    [SerializeField] private EventReference endSeasonNotificationEventName;

    [Header("Season Sounds")]
    [SerializeField] private EventReference endTurnSpringSeasonEventName;
    [SerializeField] private EventReference endTurnSummerSeasonEventName;
    [SerializeField] private EventReference endTurnFallSeasonEventName;
    [SerializeField] private EventReference endTurnWinterSeasonEventName;

    public void PlayEndTurnPanelSlideEventName() => RuntimeManager.PlayOneShot(endTurnPanelSlideEventName);
    public void PlayEndSeasonNotificationEventName() => RuntimeManager.PlayOneShot(endSeasonNotificationEventName);

    public void PlayEndTurnSpringSeasonEventName() => RuntimeManager.PlayOneShot(endTurnSpringSeasonEventName);
    public void PlayEndTurnSummerSeasonEventName() => RuntimeManager.PlayOneShot(endTurnSummerSeasonEventName);
    public void PlayEndTurnFallSeasonEventName() => RuntimeManager.PlayOneShot(endTurnFallSeasonEventName);
    public void PlayEndTurnWinterSeasonEventName() => RuntimeManager.PlayOneShot(endTurnWinterSeasonEventName);
    
}
