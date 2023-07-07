using UnityEngine;
using Core.EventSystem;
using InteractiveMap.Events;
using Core.Component;
using Core.Controller;
using InteractiveMap.PlayerController;
using InteractiveMap.CameraSystem;
using AnimationSystem;

public class IM_Player : MonoBehaviour
{
    GameObject m_camera;
    IM_IDriveable m_Vehicle;
    private KSB_IController m_playerController;
    private KSB_IController m_cameraController;
    [SerializeField]AnimationHandler m_animationHandler;

    public void Initialize(GameObject aCamera)
    {
        m_camera = aCamera;

        m_playerController = new IM_PlayerController(this, m_camera.transform);
        m_cameraController = new IM_CameraController(GameUtil.EventManager,this, m_camera);

        m_camera.GetComponent<Core.CameraSystem.TPSFollowCamera>().SetPrimaryTarget(gameObject);

        GameUtil.EventManager.RegisterSubscriber(HandleMoveEvent, IM_MoveEvent.EventID);
    }

    private void Update()
    {
        if (m_Vehicle != null)
        {
            transform.position = m_Vehicle.GetDriverSeat().position;
            transform.forward = m_Vehicle.GetDriverSeat().forward;
        }

        if (m_animationHandler != null && m_animationHandler.IsAnimationPlaying("walking"))
        {
            if (GetComponent<Rigidbody>().velocity.sqrMagnitude < 4)
                m_animationHandler.StartAnimation("idle");
        }
    }

    public void HandleMoveEvent(KSB_IEvent gameEvent)
    {
        IM_MoveEvent moveEvent = (IM_MoveEvent)gameEvent;
        if (m_Vehicle != null)
        {
            m_Vehicle.Move(moveEvent.Direction);
        }
        else
        {
            GetComponent<MovementComponent>().SetMovementDirection(moveEvent.Direction);
            m_animationHandler.StartAnimation("walking");
        }
    }

    public void EquipVehicle(IM_IDriveable aVehicle)
    {
        //If not driving 
        if (m_Vehicle == null)
        {
            m_Vehicle = aVehicle;
            GetComponent<Rigidbody>().useGravity = false;
            m_animationHandler.StartAnimation("boating");
        }
        else
        {
            //If already driving,look for drop-off point and reset driving state and spawn at neared drop-off
            Vector3 dropOff = GetNearestDropOff();
            if (dropOff != Vector3.zero)
            {
                m_Vehicle = null;
                transform.position = dropOff;
                GetComponent<Rigidbody>().useGravity = true;
                m_animationHandler.StartAnimation("idle");
            }
        }
    }

    Vector3 GetNearestDropOff()
    {
        Vector3 nearestDropOff = Vector3.zero;
        LayerMask layerMask = LayerMask.GetMask("dropOff");

        Collider[] overlapColliders = Physics.OverlapSphere(transform.position, 3.0f, layerMask);
        if (overlapColliders.Length > 0)
        {
            nearestDropOff = overlapColliders[0].transform.position;
        }

        return nearestDropOff;
    }

    public void UnRegisterSubscribedEvent()
    {
        GameUtil.EventManager.UnRegisterSubscribers(HandleMoveEvent);
        m_playerController.UnRegisterToEventSystem();
        m_cameraController.UnRegisterToEventSystem();
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Debug.DrawLine(transform.position, transform.position + transform.forward * currHitDist);
    //    Gizmos.DrawWireSphere(transform.position + transform.forward * currHitDist, 1.0f);
    //}

}
