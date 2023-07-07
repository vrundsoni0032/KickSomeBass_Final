using Core.EventSystem;
using Events;
using UnityEngine;

namespace Core.CameraSystem
{
public class CinematicCamera : BaseCamera
{
    private void Start()
    {
        base.Start();

        GameUtil.EventManager.RegisterSubscriber(HandleInitializeCinematicCamera, InitializeCinematicCamera.EventID);
        GameUtil.EventManager.RegisterSubscriber(HandleUpdateCinematicCamera, UpdateCinematicCamera.EventID);
    }

    private void HandleUpdateCinematicCamera(KSB_IEvent aEvent)
    {
        UpdateCinematicCamera cameraEvent = (UpdateCinematicCamera)aEvent;

        transform.position += cameraEvent.GetMoveVelocity()*Time.deltaTime;
        
        if (cameraEvent.GetRotation() != null)
        {
            transform.rotation = UnityEngine.Quaternion.LookRotation(cameraEvent.GetRotation());
        }
    }

    private void HandleInitializeCinematicCamera(KSB_IEvent aEvent)
    {
        InitializeCinematicCamera cameraEvent = (InitializeCinematicCamera)aEvent;
        transform.position = cameraEvent.GetPosition();
        transform.rotation = UnityEngine.Quaternion.LookRotation(cameraEvent.GetRotation());
    }

}
}