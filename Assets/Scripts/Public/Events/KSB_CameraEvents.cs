using System;
using UnityEngine;
using Core.EventSystem;
using CinematicSystem;

namespace Events
{
public class PlayCutSceneEvent : KSB_IEvent
{
    private readonly GameObject m_target;
    private readonly CinematicClip m_cinematicClip;
    private readonly bool m_switchCameraBack;

    public PlayCutSceneEvent(GameObject target, CinematicClip cinematicClip,bool aSwitchCameraBack=false) { m_target = target; m_cinematicClip = cinematicClip; m_switchCameraBack = aSwitchCameraBack; }

    public GameObject GetGameObject() { return m_target; }
    public CinematicClip GetCinematicClip() { return m_cinematicClip; }
    public bool SwitchCamereaBack() { return m_switchCameraBack; }

    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Four);
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "Play CutScene Event"; }

}

public class InitializeCinematicCamera : KSB_IEvent
{
    private readonly Vector3 m_position;
    private readonly Vector3 m_lookDirection;
    
    public InitializeCinematicCamera(Vector3 aPosition, Vector3 aRotation) { m_position = aPosition; m_lookDirection = aRotation; }

    public Vector3 GetPosition() { return m_position; }
    public Vector3 GetRotation() { return m_lookDirection; }

    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Four);
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "Initialize Cinematic Camera Event"; }
}
public class UpdateCinematicCamera : KSB_IEvent
{
    private readonly Vector3 m_moveVelocity;
    private readonly Vector3 m_lookDirection;
    
    public UpdateCinematicCamera(Vector3 moveDirection, Vector3 rotation) { m_moveVelocity = moveDirection; m_lookDirection = rotation; }

    public Vector3 GetMoveVelocity() { return m_moveVelocity; }
    public Vector3 GetRotation() { return m_lookDirection; }

    public static readonly UInt64 EventID = GuidGenerator.GenerateUInt64Guid(GuidAppendedID.Four);
    public UInt64 GetEventID() { return EventID; }
    public string GetDebugName() { return "Update Cinematic Camera Event"; }
}
}