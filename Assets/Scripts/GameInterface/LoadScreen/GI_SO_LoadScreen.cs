using UnityEngine;
using System.Collections.Generic;

namespace GameInterface.LoadScreen
{
[CreateAssetMenu(fileName = "GI_NewLoadScreen", menuName = "KSBUtilities/GameInterface/LoadScreen")]
public class GI_SO_LoadScreen : ScriptableObject
{
    [SerializeField] private List<Sprite> m_loadScreens;
    [SerializeField] private bool m_RandomizeLoadScreens = true;

    [SerializeField] private bool m_ShowTip = true;
    [SerializeField] private List<string> m_hints;
    [SerializeField] private bool m_RandomizeHints = true;

    [SerializeField] private bool m_autoSkipOnLoad = true;
    //[Range(0.0f, 5.0f)][SerializeField] private float m_minimumStayTimeInSeconds = 1.5f;

    public Sprite GetRandomLoadScreen() { return m_loadScreens[Random.Range(0, m_loadScreens.Count)]; }
    public string GetRandomHint() { return m_hints[Random.Range(0, m_hints.Count)]; }

    public Sprite GetLoadScreen(uint index) 
    {
        YCLogger.Assert("GI_SO_LoadScreen", index < m_loadScreens.Count, "index is greater than Sprites list length.");
        return m_loadScreens[(int)index];
    }

    public string GetHint(uint index) 
    {
        YCLogger.Assert("GI_SO_LoadScreen", index < m_hints.Count, "index is greater than hints list length."); 
        return m_hints[(int)index];
    }

    public bool IsLoadScreenRandomize() { return m_RandomizeLoadScreens; }
    public bool IsTipRandomize() { return m_RandomizeHints; }
    public bool IsTipEnable() { return m_ShowTip; }

    public bool AutoSkipOnLoad() { return m_autoSkipOnLoad; }
}
}