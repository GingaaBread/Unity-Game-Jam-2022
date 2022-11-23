using TimeManagement;

public class TransitionInManager : ComputerPhaseStep
{
    public override void StartProcessingForComputerPhase(bool isComputerPhaseDuringGameInit) {

        if (isComputerPhaseDuringGameInit) {
            OnFinishProcessing.Invoke();
            return;
        }

        if (TimeManager.Instance.CurrentTime.RoundInSeason == 1) {
            TransitionUI.Instance.OnAnimationComplete.AddListener(CallbackWhenAnimationCompletes);
            TransitionUI.Instance.TransitionIn();
            // don't immediately tell time manager we're done, because the show
            // animation takes time and will callback to CallbackWhenAnimationCompletes()
        } else {
            OnFinishProcessing.Invoke();
        }

    }

    public void CallbackWhenAnimationCompletes() {
        TransitionUI.Instance.OnAnimationComplete.RemoveListener(CallbackWhenAnimationCompletes);
        OnFinishProcessing.Invoke();
    }

    protected override object[] CheckForMissingReferences() => new object[0];
}
