using UnityEngine;

public class UIOpacityTweener : IUITweener
{
    [Range(0.0f, 1.0f)] [SerializeField] private float m_InitialOpacity;
    [Range(0.0f, 1.0f)] [SerializeField] private float m_FinalOpacity;

    protected override void SetInitialState()
    {
        m_gameObjectToAnimate.GetComponent<CanvasGroup>().alpha = m_InitialOpacity;
    }

    protected override LTDescr LeanTweenAnimation()
    {
        return m_gameObjectToAnimate.GetComponent<CanvasGroup>().LeanAlpha(m_FinalOpacity, m_animationDuration);
    }
}
