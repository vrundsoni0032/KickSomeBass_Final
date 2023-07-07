using System.Collections.Generic;
using Core;
using Core.InputSystem;
using GameInterface.LoadScreen;

namespace PrivateCore.CoreHelper
{
public struct KSB_CoreLevelRelationList
{
    public KSB_CoreType CoreType;
    public GI_LoadScreenType LoadScreenType;
    public List<string> LevelNames;
    public KSB_InputPhase InputPhase;

    public KSB_CoreLevelRelationList(KSB_CoreType coreType, GI_LoadScreenType screenType, KSB_InputPhase inputPhase, params string[] levelNames)
    {
        CoreType =  coreType;
        LoadScreenType = screenType;
        LevelNames = new List<string>(levelNames);
        InputPhase = inputPhase;
    }
}

public static class KSB_CoreHelper
{
    private static KSB_CoreLevelRelationList[] m_CoreRelationList =
    {
        new KSB_CoreLevelRelationList(KSB_CoreType.InteractiveMap, GI_LoadScreenType.MapLoadScreen, KSB_InputPhase.InteractiveMap, "IM_Scene"),
        new KSB_CoreLevelRelationList(KSB_CoreType.Fight, GI_LoadScreenType.FightLoadScreen, KSB_InputPhase.Fight, "FT_Arena01"),
    };

    //Not Optimized //Try to avoid using
    public static KSB_CoreType GetCoreType(string LevelName)
    {
        foreach(KSB_CoreLevelRelationList coreRelationList in m_CoreRelationList)
        {
            if(coreRelationList.LevelNames.Contains(LevelName)) { return coreRelationList.CoreType; }
        }

        YCLogger.Assert("KSB_CoreHelper", false, LevelName + "is not in the LevelRelationList.");
        return KSB_CoreType.Null;
    }
    
    //Not Optimized //Try to avoid using
    public static GI_LoadScreenType GetLoadScreenType(string LevelName)
    {
        foreach(KSB_CoreLevelRelationList coreRelationList in m_CoreRelationList)
        {
            if(coreRelationList.LevelNames.Contains(LevelName)) { return coreRelationList.LoadScreenType; }
        }

        YCLogger.Assert("KSB_CoreHelper", false, LevelName + "is not in the LevelRelationList.");
        return GI_LoadScreenType.NullScreen;
    }

    public static GI_LoadScreenType GetLoadScreenType(KSB_CoreType coreType)
    {
        foreach(KSB_CoreLevelRelationList coreRelationList in m_CoreRelationList)
        {
            if(coreRelationList.CoreType == coreType) { return coreRelationList.LoadScreenType; }
        }

        YCLogger.Assert("KSB_CoreHelper", false, coreType + "is not in the LevelRelationList.");
        return GI_LoadScreenType.NullScreen;
    }

    public static KSB_InputPhase GetInputPhase(KSB_CoreType coreType)
    {
        foreach(KSB_CoreLevelRelationList coreRelationList in m_CoreRelationList)
        {
            if(coreRelationList.CoreType == coreType) { return coreRelationList.InputPhase; }
        }

        YCLogger.Assert("KSB_CoreHelper", false, coreType + "is not in the LevelRelationList.");
        return KSB_InputPhase.Null;
    }
}
}