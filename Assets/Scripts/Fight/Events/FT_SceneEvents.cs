using System;
using Core.EventSystem;
using Fight.Fighter;

namespace Fight.Events
{
    public class FT_FighterLostEvent : KSB_IEvent
    {
        public readonly FT_Fighter Loser;
        public readonly FT_Fighter Winner;

        public FT_FighterLostEvent(FT_Fighter loser)
        {
            Loser = loser;
            Winner = FightUtil.GetOpponent(loser.GetID());
        }

        public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Four);
        public UInt64 GetEventID() { return EventID; }
        public string GetDebugName() { return "Fight Lost Event"; }
    }

    public class FT_RoundBeginEvent : KSB_IEvent
    {
        public FT_RoundBeginEvent() { }

        public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Four);
        public UInt64 GetEventID() { return EventID; }
        public string GetDebugName() { return "Fight Over Event"; }
    }

    public class FT_RoundOverEvent : KSB_IEvent
    {
        public readonly FT_Fighter Loser;
        public readonly FT_Fighter Winner;

        public FT_RoundOverEvent(FT_Fighter loser)
        {
            Loser = loser;
            Winner = FightUtil.GetOpponent(loser.GetID());
        }

        public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Four);
        public UInt64 GetEventID() { return EventID; }
        public string GetDebugName() { return "Fight Over Event"; }
    }

    public class FT_FightOverEvent : KSB_IEvent
    {
        public readonly int WinnerFighterId;

        public FT_FightOverEvent(int winnerId) => WinnerFighterId = winnerId;

        public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Four);
        public UInt64 GetEventID() { return EventID; }
        public string GetDebugName() { return "Fight Over Event"; }
    }

    public class FT_SceneEndEvent : KSB_IEvent
    {
        public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Four);
        public UInt64 GetEventID() { return EventID; }
        public string GetDebugName() { return "Fight Scene End Event"; }
    }
}