using UnityEngine;

public class UIScaleTweener : IUITweener
{
    [SerializeField] private Vector2 m_InitialScale;
    [SerializeField] private Vector2 m_FinalScale;

    protected override void SetInitialState()
    {
        transform.localScale = new Vector3(m_InitialScale.x, m_InitialScale.y, transform.localScale.z);
    }

    protected override LTDescr LeanTweenAnimation()
    {
        return LeanTween.scale(m_gameObjectToAnimate, m_FinalScale, m_animationDuration);
    }
}
