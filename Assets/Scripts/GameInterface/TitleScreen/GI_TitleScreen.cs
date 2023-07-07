using UnityEngine;
using GameInterface.Events;

namespace GameInterface.TitleScreen
{
[System.Serializable] public struct GI_TitleCanvasData
{
    public Canvas TitleCanvas;
    [Range(0.0f, 10.0f)] public float MinimumStayTimeInSeconds;
    [Range(0.0f, 10.0f)] public float MaximumStayTimeInSeconds;
}

public class GI_TitleScreen : MonoBehaviour
{
    [SerializeField] private GI_TitleCanvasData[] m_TitleCanvases;

    private int m_currentCanvasIndex = 0;
    private float m_currentScreenStayTime = 0;

    private void Start()
    {
        YCLogger.Assert("GI_TitleScreen", m_currentCanvasIndex < m_TitleCanvases.Length, "index is greater than GI_TitleCanvasData list length.");
        foreach(GI_TitleCanvasData canvasData in m_TitleCanvases) { canvasData.TitleCanvas.enabled = false; }
        m_TitleCanvases[m_currentCanvasIndex].TitleCanvas.enabled = true;

        IUITweener[] tweeners = m_TitleCanvases[m_currentCanvasIndex].TitleCanvas.GetComponents<IUITweener>();
        foreach(IUITweener tweener in tweeners) { tweener.AnimateUI(); }
    }

    private void Update()
    {
        m_currentScreenStayTime += Time.deltaTime;
        if(m_currentScreenStayTime >= m_TitleCanvases[m_currentCanvasIndex].MinimumStayTimeInSeconds)
        {
            if(Input.anyKeyDown) { SetNextTitleScreenElseExit(); }
            if(m_currentScreenStayTime >= m_TitleCanvases[m_currentCanvasIndex].MaximumStayTimeInSeconds) { SetNextTitleScreenElseExit(); }
        }
    }

    private void SetNextTitleScreenElseExit()
    {
        if(m_currentCanvasIndex + 1 >= m_TitleCanvases.Length) { GameUtil.EventManager.AddEvent(new GI_EndTitleScreenEvent()); }
        else
        {
            m_TitleCanvases[m_currentCanvasIndex].TitleCanvas.enabled = false;
            m_currentCanvasIndex++;
            m_currentScreenStayTime = 0.0f;
            YCLogger.Assert("GI_TitleScreen", m_currentCanvasIndex < m_TitleCanvases.Length, "index is greater than GI_SO_TitleScreen object list length.");

            m_TitleCanvases[m_currentCanvasIndex].TitleCanvas.enabled = true;
            IUITweener[] tweeners = m_TitleCanvases[m_currentCanvasIndex].TitleCanvas.GetComponents<IUITweener>();
            foreach(IUITweener tweener in tweeners) { tweener.AnimateUI(); }
        }
    }
}
}