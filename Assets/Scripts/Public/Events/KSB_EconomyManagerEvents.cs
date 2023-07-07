using Core.EventSystem;
using System;

namespace PublicEvents
{ 
public class AddPlayerXpPointsEvent : KSB_IEvent
{
    public readonly int XpPoints;

    public AddPlayerXpPointsEvent(int pts) => XpPoints = pts;

    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Six);
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "Add Player Xp Points Event"; }
}
}
