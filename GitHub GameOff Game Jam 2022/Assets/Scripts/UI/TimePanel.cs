namespace TimeManagement
{
    using System.Text;
    using TMPro;
    using UnityEngine;

    /// <summary>
    /// Responsible for changing the time panel depending on the updated point in time
    /// </summary>
    /// <author>Ben + Gino</author>
    public class TimePanel : ComputerPhaseStep
    {
        [SerializeField] private TMP_Text currentSeasonText;
        [SerializeField] private TMP_Text footerText;
        [SerializeField] private Animator sliderAnimator;
        [SerializeField] private Animator newSeasonAnimator;
                
        protected override object[] CheckForMissingReferences() => new object[] 
        {
            currentSeasonText, footerText, sliderAnimator, newSeasonAnimator
        };

        private void UpdateUIElementsForNewTime(PointInTime time)
        {
            if (!time.IsStartingPointInTime())
            {
                sliderAnimator.SetTrigger("Next");
            }
        }

        public void OnEndTurnButtonClicked() => TimeManager.Instance.FinishPlayerTurnPhase();

        public void UpdateTextComponents()
        {
            var time = TimeManager.Instance.CurrentTime;
            StringBuilder seasonFormatted = new StringBuilder();
            seasonFormatted.Append("<b>");
            seasonFormatted.Append(time.SeasonInYear.ToString());
            seasonFormatted.Append("</b> | Year ");
            seasonFormatted.Append(time.Year);
            currentSeasonText.text = seasonFormatted.ToString();

            string nextSeason = time.GetNextSeason().ToString().ToLower();
            string nextSeasonFormatted = nextSeason[0].ToString().ToUpper() + nextSeason.Substring(1);
            int remainingRounds = time.GetRoundsRemainingInSeason() + 1;
            footerText.text =
                $"{remainingRounds} turn{(remainingRounds > 1 ? "s" : "")} until {nextSeasonFormatted}";
        }

        public void TriggerNewSeasonAnimation()
        {
            newSeasonAnimator.Play("UISeasonChangePanelSpring");
            // TODO: Differentiate seasons
        }

        public override void StartProcessingForComputerPhase(bool isComputerPhaseDuringGameInit) 
        {
            UpdateUIElementsForNewTime(TimeManager.Instance.CurrentTime);
            OnFinishProcessing.Invoke();
        }
    }
}
