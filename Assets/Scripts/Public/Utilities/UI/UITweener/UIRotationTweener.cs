using UnityEngine;

public class UIRotationTweener : IUITweener
{
    [SerializeField] private Vector2 m_InitialRotation;
    [SerializeField] private Vector2 m_rotationAxis;
    [SerializeField] private float m_OffsetAmount;

    protected override void SetInitialState()
    {
        transform.localScale = new Vector3(m_InitialRotation.x, m_InitialRotation.y, transform.localScale.z);
    }

    protected override LTDescr LeanTweenAnimation()
    {
        return LeanTween.rotateAroundLocal(m_gameObjectToAnimate, m_rotationAxis, m_OffsetAmount, m_animationDuration);
    }
}
