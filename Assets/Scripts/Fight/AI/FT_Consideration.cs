using UnityEngine;

namespace Fight.AI
{
[System.Serializable] public struct FT_Consideration
{
    [Range(0.0f, 1.0f), SerializeField] private float Importance;
    [SerializeField] private AnimationCurve Curve;

    public float GetScore(float normalizedValue)
    {
        return Curve.Evaluate(Mathf.Clamp01(normalizedValue)) * Importance;
    }
}
}