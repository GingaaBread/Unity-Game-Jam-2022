namespace TimeManagement
{
    using System;
    using UnityEngine.Events;

    [Serializable] public class UnityEvent_Void : UnityEvent<SOEventPayload_Void> { };
    [Serializable] public class UnityEvent_Int : UnityEvent<int> { };
    [Serializable] public class UnityEvent_Float : UnityEvent<float> { };
    [Serializable] public class UnityEvent_PointInTime : UnityEvent<PointInTime> { };
}
