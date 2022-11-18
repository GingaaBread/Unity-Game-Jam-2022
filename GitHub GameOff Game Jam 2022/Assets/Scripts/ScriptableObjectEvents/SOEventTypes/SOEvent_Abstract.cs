using System.Collections.Generic;
using UnityEngine;

public abstract class SOEvent_Abstract<T> : ScriptableObject
{
    private readonly List<SOEventListenerInterface<T>> listeners = new List<SOEventListenerInterface<T>>();
    public void Raise(T item) {
        for (int i = listeners.Count -1; i >= 0; i--)
            listeners[i].OnEventRaised(item);
    }
    public void RegisterListener(SOEventListenerInterface<T> listener) { if (!listeners.Contains(listener)) listeners.Add(listener); }
    public void UnregisterListener(SOEventListenerInterface<T> listener) { if (listeners.Contains(listener)) listeners.Remove(listener); }
}
