using UnityEngine;
using Core.CameraSystem;
using Fight.PlayerController;
using Fight.AI;
using Fight.Player;
using Fight.UISystem;
using Fight.Arena;
using Core.EventSystem;
using Fight.Events;
using Fight;
using Fight.AbilitySystem;

namespace Core
{
public class FT_Core : KSB_ICore
{
    private FT_FightManager m_fightManager;
    private FT_UIManager m_UIManager;
    private CameraManager m_cameraManager;
    private FT_AIController m_AIController;

    private GameObject m_level, m_camera;

    private FT_CoreData m_CoreData;

    public bool InitializeSceneDataAssets(KSB_CoreData coreData)
    {
        m_CoreData = (FT_CoreData)coreData;
        m_cameraManager = new CameraManager();
        GameUtil.EventManager.RegisterSubscriber(HandleEndSceneEvent, FT_SceneEndEvent.EventID);

        return true;
    }

    public bool LateInitialize()
    {
            
        FT_Arena arena = GameUtil.InstantiatePrefabInActiveScene(m_CoreData.Arena).GetComponent<FT_Arena>();
        m_level = arena.gameObject;
        Vector3[] arenaSpawnPoints = arena.GetRandomSpawnPoints();
        
        FT_Player player = GameUtil.InstantiatePrefabInActiveScene(m_CoreData.Player, arenaSpawnPoints[0], Quaternion.Euler(0, Quaternion.LookRotation(arena.GetArenaCenter()- arenaSpawnPoints[0]).eulerAngles.y,0)).GetComponent<FT_Player>();
        player.GetComponent<FT_AbilityUser>().InitAbilityList(m_CoreData.abilityRegistry.GetAbilityList(GameUtil.PlayerState.AbilityList));
        //Player controller in FT_Player

        m_camera = GameUtil.InstantiatePrefabInActiveScene("Fight/Camera/FT_Camera",player.transform.forward, Quaternion.identity);

        FT_AI AI = GameUtil.InstantiatePrefabInActiveScene(m_CoreData.AI, arenaSpawnPoints[1], Quaternion.Euler(0, Quaternion.LookRotation(arena.GetArenaCenter() - arenaSpawnPoints[1], Vector3.up).eulerAngles.y,0)).GetComponent<FT_AI>();
        AI.GetComponent<FT_AbilityUser>().InitAbilityList(m_CoreData.abilityRegistry.GetAbilityList(AI.abilityList));
        m_AIController = new FT_AIController(AI);
        
        m_UIManager = GameUtil.InstantiatePrefabInActiveScene("Fight/UI/FighterHUD", Vector3.zero, Quaternion.identity).GetComponent<FT_UIManager>();
        m_fightManager = new FT_FightManager(3, player.GetID(), AI.GetID(), m_CoreData);

        //set the win and lose rewards for the current fight.
        m_fightManager.SetWinLoseXpPoints( m_CoreData.WinXpPoints , m_CoreData.LoseXpPoints );  
        m_fightManager.SetWinLoseFishBucks( m_CoreData.WinFishBucks , m_CoreData.LoseFishBucks );    
        m_fightManager.SetWinLoseSkillPoints( m_CoreData.WinSkillPoints , m_CoreData.LoseSkillPoints );   

        player.Initialize(m_camera);
        FightUtil.Initialize(this, m_CoreData, m_fightManager, player, AI, arena, m_camera.GetComponent<TPSFollowCamera>());
        m_UIManager.StartFight();
        return true;
    }

    public void UpdateSceneDataAssets()
    {
        m_AIController.UpdateMovement();
    }

    public bool CleanUpSceneDataAssets()
    {
        GameUtil.EventManager.ForceClearEventOfType((int)GuidAppendedID.Four);
        FightUtil.GetPlayer().UnRegisterToEventSystem();
        FightUtil.GetAI().UnRegisterToEventSystem();
        m_AIController.UnRegisterToEventSystem();
        m_UIManager.UnRegisterToEventSystem();
        m_fightManager.UnRegisterToEventSystem();

        GameUtil.EventManager.UnRegisterSubscribers(HandleEndSceneEvent);
        return true;
    }

    private void HandleEndSceneEvent(KSB_IEvent gameEvent)
    {
        CleanUpSceneDataAssets();
        GameUtil.EventManager.AddEvent(new KSB_EndSceneEvent("IM_Scene", GameUtil.MapCore, null));
    }

    public KSB_CoreType GetCoreType() => KSB_CoreType.Fight;

    public static bool isAnyCanvasActive = false;
    public bool IsAnyCanvasActive() => isAnyCanvasActive;
    public void SetCanvasActive(bool enabled) => isAnyCanvasActive = enabled;
}
}