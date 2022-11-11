namespace TimeManagement
{
    using UnityEngine;

    public class UITimeSlider : InspectorReferenceChecker
    {
        [SerializeField] private TimePanel timePanel;

        public void TriggerNewSeason() => timePanel.TriggerNewSeasonAnimation();

        public void UpdateTimeTextComponents() => timePanel.UpdateTextComponents();

        protected override object[] CheckForMissingReferences() => new object[] { timePanel };

    }
}
