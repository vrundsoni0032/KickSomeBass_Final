using UnityEngine;

public class IM_AreaBlock : MonoBehaviour
{
    [SerializeField] private PlayerStoryEvent m_destroyCondition;

    // Should use events instead.
    private void Update()
    {
        if (GameUtil.PlayerState.GetPlayerStoryEvent(m_destroyCondition))
        {
            Destroy(gameObject);
        }
    }
}