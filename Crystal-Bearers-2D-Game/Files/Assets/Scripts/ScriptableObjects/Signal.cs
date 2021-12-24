using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Referenced from: https://www.youtube.com/watch?v=raQ3iHhE_Kk 
[CreateAssetMenu]
public class Signal : ScriptableObject
{
    // List of Signal Listeners
    public List<SignalListener> listeners = new List<SignalListener>();

    // Raise a Signal by looping through all 
    public void Raise()
    {
        // backwards to avoid out of range exceptions
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnSignalRaised();
        }
    }

    // Add and Remove from listeners list
    public void RegisterListener(SignalListener listener)
    {
        listeners.Add(listener);
    }
    public void DeRegisterListener(SignalListener listener)
    {
        listeners.Remove(listener);
    }

}
