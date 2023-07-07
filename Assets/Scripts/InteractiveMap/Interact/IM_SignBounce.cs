using UnityEngine;

namespace InteractiveMap.Socky
{
public class IM_SignBounce : MonoBehaviour
{
    [SerializeField] private float m_jumpForce = 0.5f;
    [SerializeField] private float m_jumpInterval = 2.0f;

    private Rigidbody m_rigidBody;
    private float m_jumpTimer;
    private Vector3 m_jumpUpForce;

    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_jumpTimer = m_jumpInterval;
        m_jumpUpForce = m_jumpForce * Vector3.up;
    }

    private void Update()
    {
        m_jumpTimer -= Time.deltaTime;
        if (m_jumpTimer > 0.0f) { return; }

        m_rigidBody.velocity = m_jumpUpForce;
        m_jumpTimer = m_jumpInterval;
    }
}
}