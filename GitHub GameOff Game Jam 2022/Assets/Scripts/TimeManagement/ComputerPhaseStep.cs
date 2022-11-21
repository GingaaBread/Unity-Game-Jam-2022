using UnityEngine.Events;

namespace TimeManagement
{
    public abstract class ComputerPhaseStep : InspectorReferenceChecker
    {

        public UnityEvent OnFinishProcessing = new UnityEvent();

        public abstract void StartProcessingForComputerPhase(bool isComputerPhaseDuringGameInit);

    }
}
