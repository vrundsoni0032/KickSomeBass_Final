using UnityEngine;
using Fight.Fighter;
using CinematicSystem;
using Events;

namespace Fight.AbilitySystem
{

    [CreateAssetMenu(fileName = "SpinThrow", menuName = "KSBUtilities/Fight/Ability/SpinThrowAbility")]
    class FT_SpinThrow : FT_ComboAbility
    {
        bool throwOpponent = false;

        public override void Begin(FT_AbilityUser abilityUser)
        {
            FT_Fighter opponent = FightUtil.GetOpponent(abilityUser.GetComponent<FT_Fighter>().GetID());

            throwOpponent = false;

            GameUtil.TriggerTimer.CreateTimer(StopAbility, 4);

            opponent.GetComponent<Core.Component.MovementComponent>().StopMovement();
            abilityUser.GetComponent<Core.Component.MovementComponent>().StopMovement();

            Vector3 opponentTailPosition=opponent.GetMeshBounds().center;

            opponentTailPosition-=opponent.transform.forward* opponent.GetMeshBounds().size.z/2;

            
            abilityUser.transform.position = opponentTailPosition;
            abilityUser.transform.forward = opponent.transform.forward;
            
            GameUtil.EventManager.AddEvent(new PlayCutSceneEvent(opponent.gameObject, m_cinematicClip,true));

            abilityUser.SetState(FT_AbilityState.InProcess);
        }

        public override void InProcess(FT_AbilityUser abilityUser)
        {
            if (!throwOpponent)
            {
                FT_Fighter opponent = FightUtil.GetOpponent(abilityUser.GetComponent<FT_Fighter>().GetID());

                Vector3 customPivot = abilityUser.transform.position;
                opponent.GetTransform().RotateAround(customPivot, Vector3.up, 250 * Time.deltaTime);

                abilityUser.transform.Rotate(Vector3.up * 250 * Time.deltaTime);

                GameUtil.EventManager.AddEvent(new Fight.Events.FT_FreezeFighterActionEvent(opponent, true, true, true, true));
                GameUtil.EventManager.AddEvent(new Fight.Events.FT_FreezeFighterActionEvent(abilityUser.GetComponent<FT_Fighter>(), true, true, true, true));

                return;
            }


            abilityUser.SetState(FT_AbilityState.End);
        }

        public override void End(FT_AbilityUser abilityUser)
        {
            LayerMask mask = ~0;
            FT_Fighter opponent = FightUtil.GetOpponent(abilityUser.GetComponent<FT_Fighter>().GetID());

            opponent.GetTransform().GetComponent<Rigidbody>().AddForce((abilityUser.transform.transform.forward+Vector3.up)*200,ForceMode.VelocityChange);

            GameUtil.EventManager.AddEvent(new Fight.Events.FT_FreezeFighterActionEvent(opponent, false, false, false, false));
            GameUtil.EventManager.AddEvent(new Fight.Events.FT_FreezeFighterActionEvent(abilityUser.GetComponent<FT_Fighter>(), false, false, false, false));

            abilityUser.SetState(FT_AbilityState.None);
        }

        void StopAbility()
        {
            throwOpponent = true;
        }
    }
}