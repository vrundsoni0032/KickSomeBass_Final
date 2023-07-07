using UnityEngine;
using Core.CameraSystem;

namespace Core
{
public class IM_Core : KSB_ICore
{
    private CameraManager m_cameraManager;
    private GameObject m_level, m_camera;
    public GameObject SkillTree { private set; get; }
    public IM_Player Player { private set; get; }

    public bool InitializeSceneDataAssets(KSB_CoreData coreData)
    {
        m_cameraManager = new CameraManager();
        return true;
    }

    public bool LateInitialize()
    {
        m_level = GameUtil.InstantiatePrefabInActiveScene("InteractiveMap/Level", Vector3.zero, Quaternion.identity);
        Player = GameUtil.InstantiatePrefabInActiveScene("InteractiveMap/Player", GameUtil.PlayerState.PlayerMapSpawnPosition,
            Quaternion.LookRotation(GameUtil.PlayerState.PlayerMapSpawnLookRotation)).GetComponent<IM_Player>();
        m_camera = GameUtil.InstantiatePrefabInActiveScene("InteractiveMap/Camera",Player.transform.position - Player.transform.forward, Quaternion.identity);
        SkillTree = GameUtil.InstantiatePrefabInActiveScene("InteractiveMap/SkillTree/Prb_SkillTree");

        Player.Initialize(m_camera);

        //Should instantiate the object.
        GameObject.Find("FirstFightTrigger").SetActive(GameUtil.PlayerState.GetPlayerStoryEvent(PlayerStoryEvent.AttemptedFirstFight));
        GameObject.Find("SecondFightTrigger").SetActive(GameUtil.PlayerState.GetPlayerStoryEvent(PlayerStoryEvent.AttemptedSecondFight));
        GameObject.Find("ThirdFightTrigger").SetActive(GameUtil.PlayerState.GetPlayerStoryEvent(PlayerStoryEvent.AttemptedThirdFight));
        GameObject.Find("FourthFightTrigger").SetActive(GameUtil.PlayerState.GetPlayerStoryEvent(PlayerStoryEvent.AttemptedFourthFight));

        return true;
    }

    public void UpdateSceneDataAssets() { }
    
    public bool CleanUpSceneDataAssets()
    {
        GameUtil.EventManager.ForceClearEventOfType((int)GuidAppendedID.Two);
        Player.UnRegisterSubscribedEvent();
        return true;
    }

    public KSB_CoreType GetCoreType() => KSB_CoreType.InteractiveMap;

    public static bool isAnyCanvasActive = false;
    public bool IsAnyCanvasActive() => isAnyCanvasActive;

    public void SetCanvasActive(bool enabled) => isAnyCanvasActive = enabled;
}
}