using UnityEngine;

public abstract class IUITweener : MonoBehaviour 
{
    [SerializeField] protected GameObject m_gameObjectToAnimate;

    [SerializeField] protected bool m_animateOnStart;

    [SerializeField] protected LeanTweenType m_easeType;
    [SerializeField] protected bool m_loopAnimation;
    [SerializeField] protected bool m_pingPongLoopAnimation;

    [SerializeField] protected float m_animationDuration;
    [SerializeField] protected float m_animationDelay;

    [SerializeField] protected bool m_destroyOnAnimationEnd;

    [SerializeField] protected bool m_UseCurrentStateAsInitial;

    private void Start() { if (m_animateOnStart) { AnimateUI(); } }

    public void AnimateUI()
    {
        if (!m_UseCurrentStateAsInitial)
        {
            GameUtil.TriggerTimer.CreateTimer(SetInitialState, m_animationDelay);
        }

        Internal_AnimateUI();
    }

    private void Internal_AnimateUI()
    {
        LTDescr ltDescr = LeanTweenAnimation();

        ltDescr.setEase(m_easeType);
        ltDescr.setDelay(m_animationDelay);
        if (m_loopAnimation) { ltDescr.setLoopCount(-1); }
        if (m_pingPongLoopAnimation) { ltDescr.setLoopPingPong(); }
    }
    protected abstract void SetInitialState();
    protected abstract LTDescr LeanTweenAnimation();
}
