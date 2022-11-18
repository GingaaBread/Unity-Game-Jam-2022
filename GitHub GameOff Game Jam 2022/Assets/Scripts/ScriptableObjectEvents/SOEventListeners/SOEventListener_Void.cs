namespace TimeManagement
{
    /////////////////
    /// Special case to handle the "void event" - it doesn't pass any payload, but we make it pass an instance of VS_SO_VoidStruct so we can have these 
    /// events use the same generics logic as the cases where a payload is passed.
    public class SOEventListener_Void : SOEventListener_Abstract<SOEventPayload_Void, SOEvent_Void, UnityEvent_Void> { }
}
