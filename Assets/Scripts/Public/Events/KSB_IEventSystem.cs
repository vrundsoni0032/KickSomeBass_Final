namespace Core.EventSystem
{
//All the events will implement FT_IEvent Interface.
public interface KSB_IEvent { public System.UInt64 GetEventID(); public string GetDebugName(); }


//All the class functions interested in listening to any events must implement OnNotificationDelegate function signature.
//All the subcribed function must follow this function signature.
public delegate void OnNotificationDelegate(KSB_IEvent gameEvent);


//The main fight Event manager inherits from this class.
public interface KSB_IEventManager
{
    //To Register/UnRegister the function for the particular event types for an EventSubscriber.
    public void RegisterSubscriber(OnNotificationDelegate SubscribedFunctionPtr, params System.UInt64[] eventIDs);
    public void UnRegisterSubscriberEvents(OnNotificationDelegate SubscribedFunctionPtr, params System.UInt64[] eventIDs);
    public void UnRegisterSubscribers(params OnNotificationDelegate[] SubscribedFunctionPtrs);
    public void UnRegisterEvents(params System.UInt64[] eventIDs);

    //To add a new event to the queue.
    public bool AddEvent(KSB_IEvent gameEvent);
    public void AbortAllEventOfTypeFromActiveQueue(System.UInt64 eventID);

    public bool NotifySubscribers(float MaxDispatchTimeAllowedInMilliseconds);
    public bool FastForwardEventToNotifySubscribersImmediately(KSB_IEvent gameEvent);

    public void PauseEventSystem();
    public void ResumeEventSystem();

    public void ForceClearAllTheEvents();
    public void ForceClearEventOfType(uint appendedID);

    public uint GetActiveEventCount();
    public bool Cleanup();
}
}