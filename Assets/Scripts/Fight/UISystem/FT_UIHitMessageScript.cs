using UnityEngine;

public class FT_UIHitMessageScript : MonoBehaviour
{
    public float DestroyTime = 3.0f;

    void Start()
    {
        Destroy(gameObject, DestroyTime);
    }
}