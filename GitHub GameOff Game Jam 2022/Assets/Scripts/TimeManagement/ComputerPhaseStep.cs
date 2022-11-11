namespace TimeManagement
{
    public abstract class ComputerPhaseStep : InspectorReferenceChecker
    {

        public abstract void DoProcessingForComputerPhaseDuringGameInit();
        public abstract void DoProcessingForComputerPhase();

    }
}
