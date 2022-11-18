using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Void Event", menuName = "SOEvents/Void")]
public class SOEvent_Void : SOEvent_Abstract<SOEventPayload_Void> { 
    public void Raise() => Raise(new SOEventPayload_Void()); 
}
