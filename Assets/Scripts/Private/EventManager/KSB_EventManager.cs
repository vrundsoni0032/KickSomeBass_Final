using System.Collections.Generic;
using Core.EventSystem;

namespace PrivateCore
{
public class KSB_EventManager : KSB_IEventManager
{
    private MultiValueDictionary<System.UInt64, OnNotificationDelegate> m_eventSubscribers;
    private List<KSB_IEvent>[] m_eventQueues;
    
    private const uint  m_TotalEventQueues = 2;
    private uint m_activeEventQueueIndex = 0;

    private bool m_ForceStopEventSystem = false;
    
    public KSB_EventManager()
    {
        m_eventSubscribers = new MultiValueDictionary<System.UInt64, OnNotificationDelegate>();

        m_eventQueues = new List<KSB_IEvent>[(int)m_TotalEventQueues];
        m_eventQueues[0] = new List<KSB_IEvent>();
        m_eventQueues[1] = new List<KSB_IEvent>();
    }

    ~KSB_EventManager() { if(Cleanup()) { YCLogger.Info("FT_EventManager", "Event Manager exited successfully!"); } }

    public void RegisterSubscriber(OnNotificationDelegate SubscribedFunctionPtr, params System.UInt64[] eventIDs)
    {
        foreach (System.UInt64 eventID in eventIDs)
        {
            YCLogger.Assert("FT_EventManager", !m_eventSubscribers.CheckValueExist(eventID, SubscribedFunctionPtr),
                            SubscribedFunctionPtr.ToString() + " subscriber function is already registered for " + eventID + ".");

            //if(m_eventSubscribers.CheckValueExist(eventID, SubscribedFunctionPtr)) { continue; }

            //YCLogger.Info("FT_EventManager", SubscribedFunctionPtr.Method.Name + " subscriber function registered for " + eventID + ".");

            m_eventSubscribers.AddValue(eventID, SubscribedFunctionPtr);
        }
    }

    public void UnRegisterSubscriberEvents(OnNotificationDelegate SubscribedFunctionPtr, params System.UInt64[] eventIDs)
    {
        foreach (System.UInt64 eventID in eventIDs)
        {
            bool bRemoved = m_eventSubscribers.RemoveValue(eventID, SubscribedFunctionPtr);

            YCLogger.Assert("FT_EventManager", bRemoved, 
                SubscribedFunctionPtr.ToString() + " couldn't be removed successfully for " + eventID + ".");
        }
    }

    public void UnRegisterSubscribers(params OnNotificationDelegate[] SubscribedFunctionPtrs)
    {
        foreach(OnNotificationDelegate subscribedFunctionPtr in SubscribedFunctionPtrs) { m_eventSubscribers.RemoveValueForAllKeys(subscribedFunctionPtr); }
    }

    public void UnRegisterEvents(params System.UInt64[] eventIDs)
    {
        foreach (System.UInt64 eventID in eventIDs) { m_eventSubscribers.RemoveKey(eventID);}
    }

    public bool Cleanup()
    {
        m_eventSubscribers.Clear();
        m_eventQueues[0].Clear();
        m_eventQueues[1].Clear();
        
        return true;
    }

    public bool AddEvent(KSB_IEvent gameEvent)
    {
        if(m_ForceStopEventSystem) { return false; }

        YCLogger.Assert("FT_EventManager", m_activeEventQueueIndex >= 0, "Event Queue index is out of scope.");
        YCLogger.Assert("FT_EventManager", m_activeEventQueueIndex < m_TotalEventQueues, "Event Queue index is out of scope.");
        YCLogger.Assert("FT_EventManager", m_eventSubscribers.CheckKeyExist(gameEvent.GetEventID()), 
                        gameEvent.GetDebugName() + " is not registered with any subscriber delegate.");

        m_eventQueues[m_activeEventQueueIndex].Add(gameEvent);

        return true;
    }

    public void AbortAllEventOfTypeFromActiveQueue(System.UInt64 eventID)
    {
        m_eventQueues[m_activeEventQueueIndex].RemoveAll(item => item.GetEventID() == eventID);
    }

    public bool NotifySubscribers(float MaxDispatchTimeAllowedInMilliseconds)
    {
        if(m_ForceStopEventSystem) { return false; }

        //Swap the active Queue and clear it.
        m_activeEventQueueIndex = (m_activeEventQueueIndex + 1) % m_TotalEventQueues;
        m_eventQueues[m_activeEventQueueIndex].Clear();

        bool bSuccessfullyPushedAllEvents = false;

        bSuccessfullyPushedAllEvents = TimedExecutionHelper.ExitActionIfExceedsTimeLimit(MaxDispatchTimeAllowedInMilliseconds, Internal_NotifySubscribers);

        if(!bSuccessfullyPushedAllEvents)
        {
            uint PreviousActiveQueueIndex = (m_activeEventQueueIndex + 1) % m_TotalEventQueues;
        
            while(m_eventQueues[PreviousActiveQueueIndex].Count > 0)
            {
                 KSB_IEvent currentEvent = m_eventQueues[PreviousActiveQueueIndex][m_eventQueues[PreviousActiveQueueIndex].Count - 1];
                 m_eventQueues[PreviousActiveQueueIndex].RemoveAt(m_eventQueues[PreviousActiveQueueIndex].Count - 1);
        
                m_eventQueues[m_activeEventQueueIndex].Insert(0, currentEvent);
            }
        }
        return true;
    }

    private bool Internal_NotifySubscribers()
    {
         uint PreviousActiveQueueIndex = (m_activeEventQueueIndex + 1) % m_TotalEventQueues;

        if(m_eventQueues[PreviousActiveQueueIndex].Count > 0)
        {
            KSB_IEvent CurrentEvent = m_eventQueues[PreviousActiveQueueIndex][0];
            m_eventQueues[PreviousActiveQueueIndex].RemoveAt(0);

            System.UInt64 CurrentEventID = CurrentEvent.GetEventID();

            YCLogger.Assert("FT_EventManager", m_eventSubscribers.CheckKeyExist(CurrentEventID),
                            CurrentEventID + " is not registered for any subscriber Functions.");

            foreach(OnNotificationDelegate delegateFunctions in m_eventSubscribers[CurrentEventID])
            {
                delegateFunctions.Invoke(CurrentEvent);
            }
        }

        if(m_eventQueues[PreviousActiveQueueIndex].Count > 0) { return false; } //Events yet left to be dispatched.
        return true;
    }

    public bool FastForwardEventToNotifySubscribersImmediately(KSB_IEvent gameEvent)
    {
        System.UInt64 gameEventID = gameEvent.GetEventID();

        YCLogger.Assert("FT_EventManager", m_eventSubscribers.CheckKeyExist(gameEventID),
                        gameEventID + " is not registered for any subscriber Functions.");

        foreach(OnNotificationDelegate delegateFunctions in m_eventSubscribers[gameEventID]) { delegateFunctions.Invoke(gameEvent);}

        return true;
    }

    public void PauseEventSystem() { m_ForceStopEventSystem = true; }

    public void ResumeEventSystem() { m_ForceStopEventSystem = false; }

    public uint GetActiveEventCount() { return (uint)m_eventQueues[m_activeEventQueueIndex].Count; }

    public void ForceClearAllTheEvents()
    {
        m_eventQueues[0].Clear();
        m_eventQueues[1].Clear();
        m_activeEventQueueIndex = 0;
    }

    public void ForceClearEventOfType(uint appendedID)
    {
        m_eventQueues[0].RemoveAll(item => item.GetEventID().ToString().StartsWith(appendedID.ToString()));
        m_eventQueues[1].RemoveAll(item => item.GetEventID().ToString().StartsWith(appendedID.ToString()));
    }
}
}