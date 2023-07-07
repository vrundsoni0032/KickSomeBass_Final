using Core.Component;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IM_Boat : MonoBehaviour, IM_Interactable, IM_IDriveable
{
    Rigidbody m_Rigidbody;
    [SerializeField] float m_speed=50;
    [SerializeField] Transform m_driverSeat;
    bool m_isControlled;
    Vector3 moveDirection;
    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }
    public void TriggerInteraction()
    {
        m_isControlled = true;
    }
    void Update()
    {
        if (moveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDirection, Vector3.up), 1.5f * Time.deltaTime);
        }
        m_Rigidbody.velocity = moveDirection * m_speed;
        moveDirection=Vector3.zero;
    }
    public void Move(Vector3 aDirection)
    {
        moveDirection=aDirection;
        moveDirection.y = 0;

    }
    public Transform GetDriverSeat()
    {
        return m_driverSeat;
    }
}

public interface IM_IDriveable
{
    void Move(Vector3 aDirection);
    Transform GetDriverSeat();
}
