using System;
using System.Collections.Generic;
using UnityEngine;

namespace CinematicSystem
{
#region ShotEnums
[Serializable] public enum ShotDistance { CloseUp, Medium, Long }
[Serializable] public enum VerticalOrientation { Low3_4, Low1_4, Straight, Top1_4, Top3_4 }
[Serializable] public enum HorizontalOrientation { Front, Left, Right, Back }
[Serializable] public enum ShotMotion { ZoomIn, ZoomOut, PanLeft, PanRight }
#endregion

public static class CinematicShotHelper
{
    static Dictionary<ShotDistance, float> m_shotDistance;
    static Dictionary<VerticalOrientation, float> m_verticalOrientationDirection;
    static Dictionary<HorizontalOrientation, Vector3> m_horizontalOrientationDirection;
    static Dictionary<ShotMotion, Vector3> m_shotMotionDirection;

    public static void Inititalize()
    {
        m_shotDistance = new Dictionary<ShotDistance, float>();
        m_verticalOrientationDirection = new Dictionary<VerticalOrientation, float>();
        m_horizontalOrientationDirection = new Dictionary<HorizontalOrientation, Vector3>();
        m_shotMotionDirection = new Dictionary<ShotMotion, Vector3>();
        m_shotDistance.Add(ShotDistance.CloseUp, 3);
        m_shotDistance.Add(ShotDistance.Medium, 8);
        m_shotDistance.Add(ShotDistance.Long, 15);

        m_verticalOrientationDirection.Add(VerticalOrientation.Low3_4, -0.75f);
        m_verticalOrientationDirection.Add(VerticalOrientation.Low1_4, -0.50f);
        m_verticalOrientationDirection.Add(VerticalOrientation.Straight, 0.0f);
        m_verticalOrientationDirection.Add(VerticalOrientation.Top1_4, 0.50f);
        m_verticalOrientationDirection.Add(VerticalOrientation.Top3_4, 0.75f);

        m_horizontalOrientationDirection.Add(HorizontalOrientation.Front, Vector3.forward);
        m_horizontalOrientationDirection.Add(HorizontalOrientation.Back, Vector3.back);
        m_horizontalOrientationDirection.Add(HorizontalOrientation.Left, Vector3.left);
        m_horizontalOrientationDirection.Add(HorizontalOrientation.Right, Vector3.right);

        m_shotMotionDirection.Add(ShotMotion.ZoomIn, Vector3.back);
        m_shotMotionDirection.Add(ShotMotion.ZoomOut, Vector3.forward);
        m_shotMotionDirection.Add(ShotMotion.PanLeft, Vector3.right);
        m_shotMotionDirection.Add(ShotMotion.PanRight, Vector3.left);
    }

    public static Vector3 GetShotHorizontalDirection(HorizontalOrientation orientation)
    {
        Vector3 direction = new Vector3();
        m_horizontalOrientationDirection.TryGetValue(orientation, out direction);
        return direction;
    }
    public static float GetShotVerticalAmount(VerticalOrientation orientation)
    {
        float amount = 0;
        m_verticalOrientationDirection.TryGetValue(orientation, out amount);
        return amount;
    }
    public static Vector3 GetShotMotionDirection(ShotMotion aMoiton)
    {
        Vector3 direction = new Vector3();
        m_shotMotionDirection.TryGetValue(aMoiton, out direction);
        return direction;
    }

    public static float GetShotDistance(ShotDistance shotDistanceType)
    {
        float distance;
        m_shotDistance.TryGetValue(shotDistanceType, out distance);
        return distance;
    }
}
}
