using FMODUnity;
using UnityEngine;
using UnityEngine.Assertions;

public class AmbienceTrigger : MonoBehaviour
{
    [SerializeField] private EventReference farmAmbience;

    public static AmbienceTrigger Instance;

    private void Start()
    {
        Assert.IsNull(Instance, "AmbienceTrigger instance already exists!");
        Instance = this;
    }

    public void PlayFarmAmbience() => RuntimeManager.PlayOneShot(farmAmbience);
}
