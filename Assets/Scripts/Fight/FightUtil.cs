using Core;
using UnityEngine;
using Core.CameraSystem;
using Fight.AbilitySystem;
using Fight.AI;
using Fight.Arena;
using Fight.Fighter;
using Fight.Player;

namespace Fight
{
public enum FT_MoveDirection { Toward, Away, Left, Right }

public static class FightUtil
{
    public static FT_Core FightCore { private set; get; }
    public static FT_CoreData CoreData { private set; get; }
    public static FT_FightManager FightManager { private set; get; }
    public static FT_Arena Arena { private set; get; }
    public static BaseCamera Camera { private set; get; }

    public static FT_AbilityUser PlayerAbilityUser { private set; get; }

    public static FT_AbilityUser AIAbilityUser { private set; get; }
    public static FT_Brain AIBrain { private set; get; }

    private static FT_Fighter[] m_Fighters;

    public static void Initialize(FT_Core fightCore, FT_CoreData fightCoreData, FT_FightManager fightManager, FT_Player player, FT_AI ai, FT_Arena arena, BaseCamera camera)
    {
        FightCore = fightCore;
        CoreData = fightCoreData;
        Arena = arena;
        Camera = camera;
        FightManager = fightManager;

        m_Fighters = new FT_Fighter[]{ player, ai };
        PlayerAbilityUser = player.GetComponent<FT_AbilityUser>();

        AIAbilityUser = ai.GetComponent<FT_AbilityUser>();
        AIBrain = ai.GetComponent<FT_Brain>();
    }

    public static FT_Player GetPlayer() => (FT_Player)m_Fighters[0];
    public static FT_AI GetAI() => (FT_AI)m_Fighters[1];

    public static int GetPlayerID() => m_Fighters[0].GetID();
    public static int GetAIID() => m_Fighters[1].GetID();

    public static FT_Fighter GetFighter(int fighterId) => m_Fighters[fighterId];
    public static FT_Fighter GetOpponent(int fighterId) => m_Fighters[(fighterId + 1) % 2];


    public static float GetDistance() => (GetAI().transform.position - GetPlayer().transform.position).magnitude;

    public static Vector3 GetRelativeDirection(FT_MoveDirection direction, int fighterId)
    {
        switch (direction)
        {
            case FT_MoveDirection.Toward:
                return (GetOpponent(fighterId).transform.position - GetFighter(fighterId).transform.position).normalized;

            case FT_MoveDirection.Away:
                return (GetFighter(fighterId).transform.position - GetOpponent(fighterId).transform.position).normalized;

            case FT_MoveDirection.Right:
            {
                Vector3 forward = GetOpponent(fighterId).transform.position - GetFighter(fighterId).transform.position;
                return new Vector3(forward.z, forward.y, -forward.x).normalized;
            }

            case FT_MoveDirection.Left:
            {
                Vector3 backward = GetFighter(fighterId).transform.position - GetOpponent(fighterId).transform.position;
                return new Vector3(backward.z, backward.y, -backward.x).normalized;
            }
        }

        return Vector3.zero;
    }

    //public bool CheckForObstacle(Vector3 direction)
    //{
    //    //Debug.DrawRay(m_AITransform.position, direction * ((FT_AI)m_AI).GetObstacleDetectionRange());
    //
    //    RaycastHit hit;
    //    Ray ray = new Ray(AITransform.position, direction);
    //    
    //    if (Physics.Raycast(ray, out hit, ((FT_AI)AI).GetObstacleDetectionRange(), Arena.GetObstacleMask()))
    //    {
    //        //YCLogger.Info("AI Helper","Hit");
    //        //Debug.DrawLine(ray.origin, hit.point, Color.black, 2);
    //        return true;
    //    }
    //
    //    return false;
    //}
    }
}