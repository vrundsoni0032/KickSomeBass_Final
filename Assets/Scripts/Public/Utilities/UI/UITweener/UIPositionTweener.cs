using UnityEngine;

public class UIPositionTweener : IUITweener
{
    [SerializeField] private Vector2 m_InitialPosition;
    [SerializeField] private Vector2 m_OffsetAmount;

    protected override void SetInitialState()
    {
        transform.position = new Vector3(m_InitialPosition.x, m_InitialPosition.y, transform.position.z);
    }

    protected override LTDescr LeanTweenAnimation()
    {
        return LeanTween.moveLocal(m_gameObjectToAnimate, m_OffsetAmount, m_animationDuration);
    }
}
