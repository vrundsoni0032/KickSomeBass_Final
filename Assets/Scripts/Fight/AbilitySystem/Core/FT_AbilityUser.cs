using UnityEngine;
using System.Collections.Generic;
using Fight.Events;

namespace Fight.AbilitySystem
{
public class FT_AbilityUser : MonoBehaviour
{
    private Dictionary<string, FT_Ability> m_abilities;

    public FT_Ability CurrentAbility { private set; get; }
    public FT_AbilityState CurrentAbilityState { private set; get; }

    public void InitAbilityList(List<FT_Ability> abilities)
    {
        m_abilities = new Dictionary<string, FT_Ability>();
        foreach (var ability in abilities) 
        { 
            m_abilities.Add(ability.name, ability);
            if(ability.GetActionSpherePrefab()!=null)
            ObjectPoolManager.CreateObjectPool(ability.GetActionSpherePrefab(), 10);
        }
    }

    //used for animations, might be moved/ changed if ineffective
    void Start()
    {
        GameObject modelObject = transform.Find("Mesh").gameObject;

        m_Velocity = Vector3.zero;

        m_RigidBody = this.gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (CurrentAbility == null) { return; }
        if (CurrentAbilityState == FT_AbilityState.None) { return; }
      
        switch (CurrentAbilityState)
        {
            case FT_AbilityState.Begin:
                CurrentAbility.Begin(this);
                break;

            case FT_AbilityState.InProcess:
                CurrentAbility.InProcess(this);
                break;

            case FT_AbilityState.End:
                //Last ability was successful so notify success of the ability
                //TODO: to make this more accurate,have the actionsphere callback on succesful hit and then add this event
                GameUtil.EventManager.AddEvent(new FT_AbilitySucessEvent(GetComponent<Fight.Fighter.FT_Fighter>(),CurrentAbility.Name));

                CurrentAbility.End(this);
                break;
        }

    }

    public void ExecuteAbility(string abilityName, Vector3 abilityDirection, FT_AbilityState abilityState = FT_AbilityState.Begin)
    {
        //Check if there is ability being executed and exit if it is not interruptable
        if (CurrentAbility != null && abilityName=="StaminaGain") { return; }

        //check if there has been enough time between abilities
        //if (AbilityTimer > 0) { return; }
        CurrentAbilityState = abilityState;
        CurrentAbility = m_abilities[abilityName];
        CurrentAbility.Direction = abilityDirection;
        //AbilityTimer = CurrentAbility.Dealy;
    }

    public void SetState(FT_AbilityState abilityState) 
    {
        CurrentAbilityState = abilityState;
        if(abilityState == FT_AbilityState.None) { CurrentAbility = null; }
    }

    public bool HasAbility(string abilityName) => m_abilities.ContainsKey(abilityName);
        public FT_Ability GetAbility(string abilityName)
        {
            if(HasAbility(abilityName))
           return m_abilities[abilityName];
            else
                return null;
        }
// used to for anims
    Vector3 m_Velocity;
    Rigidbody m_RigidBody;

    public float MaxOnGroundAnimSpeed = 10.0f;
}
}