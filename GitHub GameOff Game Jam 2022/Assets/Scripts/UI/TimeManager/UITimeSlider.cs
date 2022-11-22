namespace TimeManagement
{
    using FMODUnity;
    using UnityEngine;

    public class UITimeSlider : InspectorReferenceChecker
    {
        [SerializeField] private TimePanel timePanel;

        [Header("Slider Sounds")]
        [SerializeField] private EventReference turnProgressMeterEventName;
        [SerializeField] private EventReference endTurnNotificationChimeEventName;

        public void TriggerNewSeason() => timePanel.TriggerNewSeasonAnimation();

        public void UpdateTimeTextComponents() => timePanel.UpdateTextComponents();

        // Sound Methods
        public void PlayTurnProgressMeterSound() => RuntimeManager.PlayOneShot(turnProgressMeterEventName);
        public void PlayEndTurnNotificationChimeSound() => RuntimeManager.PlayOneShot(endTurnNotificationChimeEventName);

        protected override object[] CheckForMissingReferences() => new object[] { timePanel };

    }
}