using System;
using System.Collections.Generic;
using Core.EventSystem;
using UnityEngine;

public class ActionToTriggerData
{
    public Action ActionToTrigger;
    public float TriggerCountdown;
    public bool DestroyOnNewLoad;
    public uint UniqueID = 0;

    public ActionToTriggerData(Action actionToTigger, float triggerCountdown, bool destroyOnNewLoad, uint uniqueID = 0)
    {
        ActionToTrigger = actionToTigger;
        TriggerCountdown = triggerCountdown;
        DestroyOnNewLoad = destroyOnNewLoad;
        UniqueID = uniqueID;
    }
}

public class TriggerTimer
{
    private List<ActionToTriggerData> m_ActionToTriggerList;

    public TriggerTimer(KSB_IEventManager eventManager)
    {
        m_ActionToTriggerList = new List<ActionToTriggerData>();
        eventManager.RegisterSubscriber(HandleClearTimerEvent, KSB_ClearTimerEvent.EventID);
    }

    ~TriggerTimer() { m_ActionToTriggerList.Clear(); }

    public void CreateTimer(Action actionToTrigger, float countdownTimer, bool detroyOnNewLoad = true, uint uniqueID = 0)
    {
        m_ActionToTriggerList.Add(new ActionToTriggerData(actionToTrigger, countdownTimer, detroyOnNewLoad, uniqueID));
    }

    //This removes the timer marked as true for destroyOnNewLoad on scene end.
    public void HandleClearTimerEvent(KSB_IEvent gameEvent) { m_ActionToTriggerList.RemoveAll(item => item.DestroyOnNewLoad); }

    public void UpdateTimers()
    {
        for(int i = 0; i < m_ActionToTriggerList.Count; i++)
        {
            m_ActionToTriggerList[i].TriggerCountdown -= Time.deltaTime;

            if (m_ActionToTriggerList[i].TriggerCountdown <= 0.0f) { m_ActionToTriggerList[i].ActionToTrigger.Invoke(); }
        }

        m_ActionToTriggerList.RemoveAll(item => item.TriggerCountdown <= 0.0f);
    }

    public void RemoveAllTimerWithID(uint uniqueID)
    {
        m_ActionToTriggerList.RemoveAll(item => item.UniqueID == uniqueID);
    }
}