using UnityEngine;
using UnityEngine.SceneManagement;
using Core;
using Core.EventSystem;
using Core.AudioSystem;

public static class GameUtil
{
    public delegate KSB_ICore GetCurrentCoreDelegate();

    public static KSB_ICore MapCore { private set; get; }
    public static KSB_ICore FightCore { private set; get; }
    public static KSB_IEventManager EventManager { private set; get; }
    public static TriggerTimer TriggerTimer { private set; get; }
    public static KSB_SO_SoundManager SoundManager { private set; get; }
    public static GetCurrentCoreDelegate GetCurrentCore { private set; get; }
    public static PlayerState PlayerState { set; get; }

    public static void Initialize(KSB_ICore mapCore, KSB_ICore fightCore, KSB_IEventManager eventManager, KSB_SO_SoundManager soundManager, TriggerTimer triggerTimer, 
        GetCurrentCoreDelegate getCurrentCoreFunc, PlayerState playerState)
    {
        MapCore = mapCore;
        FightCore = fightCore;
        EventManager = eventManager;
        SoundManager = soundManager;
        TriggerTimer = triggerTimer;
        GetCurrentCore = getCurrentCoreFunc;
        PlayerState = playerState;
    }

    public static KSB_ICore GetCore(KSB_CoreType coreType)
    {
        switch (coreType)
        {
            case KSB_CoreType.InteractiveMap: return MapCore;
            case KSB_CoreType.Fight: return FightCore;
        }
        return null;
    }

    public static Scene GetActiveScene() { return SceneManager.GetActiveScene(); }

    public static GameObject InstantiatePrefabInActiveScene(GameObject prefab)
    {
        GameObject gameObject = GameObject.Instantiate(prefab);
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());

        return gameObject;
    }

    public static GameObject InstantiatePrefabInActiveScene(string PrefabPathRelativeToResources)
    {
        GameObject gameObject = GameObject.Instantiate(Resources.Load(PrefabPathRelativeToResources) as GameObject);
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());

        return gameObject;
    }

    public static GameObject InstantiatePrefabInActiveScene(string PrefabPathRelativeToResources, Vector3 position, Quaternion rotation)
    {
        GameObject gameObject = GameObject.Instantiate(Resources.Load(PrefabPathRelativeToResources) as GameObject, position, rotation);
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());

        return gameObject;
    }

    public static GameObject InstantiatePrefabInActiveScene(string PrefabPathRelativeToResources, Transform transform)
    {
        GameObject gameObject = GameObject.Instantiate(Resources.Load(PrefabPathRelativeToResources) as GameObject, transform);
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());

        return gameObject;
    }

    public static GameObject InstantiatePrefabInActiveScene(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject gameObject = GameObject.Instantiate(prefab, position, rotation);
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());

        return gameObject;
    }
}
