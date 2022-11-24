namespace TimeManagement
{
    using System.Text;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Responsible for changing the time panel depending on the updated point in time
    /// </summary>
    /// <author>Ben + Gino</author>
    public class TimePanel : ComputerPhaseStep
    {
        [Header("Always Visible Panel")]
        [SerializeField] private TMP_Text currentSeasonText;
        [SerializeField] private TMP_Text footerText;
        [SerializeField] private Animator sliderAnimator;
        [SerializeField] private Image currentSeasonIconImage;
        [SerializeField] private Sprite[] seasonIcons;
        [SerializeField] private Button EndTurnButton;

        [Header("End of Season Animation")]
        [SerializeField] private TMP_Text animationSeasonText;
        [SerializeField] private Animator newSeasonAnimator;

        protected override object[] CheckForMissingReferences() => new object[] 
        {
            currentSeasonText, footerText, sliderAnimator, newSeasonAnimator, animationSeasonText, currentSeasonIconImage, EndTurnButton
        };

        private void UpdateUIElementsForNewTime(PointInTime time)
        {
            if (!time.IsStartingPointInTime())
            {
                sliderAnimator.SetTrigger("Next");
            }
            UpdateTextComponents();
        }

        public void OnEndTurnButtonClicked() {
            EndTurnButton.interactable = false;
            TimeManager.Instance.FinishPlayerTurnPhase();
        }

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
            string nextSeasonFormatted = nextSeason[0].ToString().ToUpper() + nextSeason[1..];
            int remainingRounds = time.GetRoundsRemainingInSeason() + 1;
            footerText.text =
                $"{remainingRounds} turn{(remainingRounds > 1 ? "s" : "")} until {nextSeasonFormatted}";
        }

        public void TriggerNewSeasonAnimation()
        {
            var seasonType = TimeManager.Instance.CurrentTime.SeasonInYear;
            var season = seasonType.ToString().ToLower();
            string seasonFormatted = season[0].ToString().ToUpper() + season[1..];
            string animationState = "UISeasonChangePanel" + seasonFormatted;
            newSeasonAnimator.Play(animationState);
            animationSeasonText.text = $"<b>New Season:</b>\n{seasonFormatted}";

            switch (seasonType)
            {
                case SeasonType.SPRING:
                    currentSeasonIconImage.sprite = seasonIcons[0];
                    break;
                case SeasonType.SUMMER:
                    currentSeasonIconImage.sprite = seasonIcons[1];
                    break;
                case SeasonType.FALL:
                    currentSeasonIconImage.sprite = seasonIcons[2];
                    break;
                case SeasonType.WINTER:
                    currentSeasonIconImage.sprite = seasonIcons[3];
                    break;
                default: throw new System.NotImplementedException($"Season type: {seasonType} is not implemented.");
            }
        }

        public void HandlePlayerTurnStart() {
            EndTurnButton.interactable = true;
        }

        public override void StartProcessingForComputerPhase(bool isComputerPhaseDuringGameInit) 
        {
            if (isComputerPhaseDuringGameInit) {
                TimeManager.OnStartPlayerTurn.AddListener(HandlePlayerTurnStart);
                EndTurnButton.interactable = false;
            }

            UpdateUIElementsForNewTime(TimeManager.Instance.CurrentTime);
            OnFinishProcessing.Invoke();
        }
    }
}
