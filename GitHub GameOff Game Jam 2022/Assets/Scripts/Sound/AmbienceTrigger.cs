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

    private void Start()
    {
        Assert.IsNull(Instance, "AmbienceTrigger instance already exists!");
        Instance = this;

        musicEventInstance = RuntimeManager.CreateInstance(music);
        musicEventInstance.start();
    }

    public void PlayFarmAmbience() => RuntimeManager.PlayOneShot(farmAmbience);

    public override void StartProcessingForComputerPhase(bool isComputerPhaseDuringGameInit)
    {
        if (!isComputerPhaseDuringGameInit && TimeManager.Instance.CurrentTime.IsFirstRoundOfSeason())
        {
            // The seasons should be 0 for spring, 1 for summer, 2 for autumn, 3 for winter (and 4 for the will)
            musicEventInstance.setParameterByName("Season", (int) TimeManager.Instance.CurrentTime.SeasonInYear);
        }

        OnFinishProcessing.Invoke();
    }

    protected override object[] CheckForMissingReferences()
    {
        return new object[0];
    }
}
