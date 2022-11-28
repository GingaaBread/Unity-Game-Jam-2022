using FMOD.Studio;
using FMODUnity;
using TimeManagement;
using UnityEngine;
using UnityEngine.Assertions;

public class AmbienceTrigger : ComputerPhaseStep
{
    [SerializeField] private EventReference farmAmbience;
    [SerializeField] private EventReference music;

    public static AmbienceTrigger Instance;

    private EventInstance musicEventInstance;
    private bool hasBeenExecuted;

    private void Start()
    {
        Assert.IsNull(Instance, "AmbienceTrigger instance already exists!");
        Instance = this;

        musicEventInstance = RuntimeManager.CreateInstance(music);
        musicEventInstance.start();
    }

    public void PlayFarmAmbience()
    {
        if (!hasBeenExecuted)
        {
            RuntimeManager.PlayOneShot(farmAmbience);

            // After the will music jump to spring
            musicEventInstance.setParameterByName("Season", 1);
            
            hasBeenExecuted = true;
        }
    }

    public override void StartProcessingForComputerPhase(bool isComputerPhaseDuringGameInit)
    {
        if (!isComputerPhaseDuringGameInit && TimeManager.Instance.CurrentTime.IsFirstRoundOfSeason())
        {
            // The seasons should be 1 for spring, 2 for summer, 3 for autumn, 4 for winter (and 0 for the will)
            musicEventInstance.setParameterByName("Season", (int) TimeManager.Instance.CurrentTime.SeasonInYear + 1);
        }

        OnFinishProcessing.Invoke();
    }

    protected override object[] CheckForMissingReferences()
    {
        return new object[0];
    }
}
