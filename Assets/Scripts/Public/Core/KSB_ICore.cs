using UnityEngine;

namespace Core
{
public enum KSB_CoreType
{
    Null,
    InteractiveMap,
    Fight,
}

public interface KSB_ICore
{
    public bool InitializeSceneDataAssets(KSB_CoreData coreData);
    public bool LateInitialize();
    public void UpdateSceneDataAssets();
    public bool CleanUpSceneDataAssets();
    public void SetCanvasActive(bool enabled);
    public bool IsAnyCanvasActive();
    public KSB_CoreType GetCoreType();
}

public class KSB_CoreData : ScriptableObject { }
}