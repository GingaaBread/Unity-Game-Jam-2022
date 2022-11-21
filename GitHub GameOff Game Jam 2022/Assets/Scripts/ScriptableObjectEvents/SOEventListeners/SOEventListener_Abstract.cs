using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

/// <typeparam name="PT">payload type - what gets passed when an event is fired</typeparam>
/// <typeparam name="SOE">event - the type of SOGameEvent this listener will listen to</typeparam>
/// <typeparam name="UER">unity event respose - the type of the unity event which this listener will invoke for downstreams to listen to</typeparam>
public abstract class SOEventListener_Abstract<PT, SOE, UER> : MonoBehaviour, 
    SOEventListenerInterface<PT> where SOE : SOEvent_Abstract<PT> where UER : UnityEvent<PT> 
{
    public SOE GameEvent { get { return gameEvent; } set { gameEvent = value; } }
    [SerializeField]    private SOE gameEvent;
    [SerializeField]    private UER unityEventResponse;

    private void OnEnable() {
        Assert.IsNotNull(GameEvent, $"SOEvent listener expected an SOEvent to be set on gameobject {gameObject.name}");
        GameEvent.RegisterListener(this);
    }

    private void OnDisable() => GameEvent.UnregisterListener(this);
    public void OnEventRaised(PT payload) {
        unityEventResponse?.Invoke(payload);
    }
}





