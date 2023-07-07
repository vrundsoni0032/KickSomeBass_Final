using UnityEngine;
using Core.EventSystem;
using Core.Component;
using Fight.AbilitySystem;
using Fight.ComboSystem;
using Fight.Events;
using AnimationSystem;

namespace Fight.Fighter
{    
public abstract class FT_Fighter : MonoBehaviour
{
    [SerializeField] protected float m_staminaGenerationTime;
    [SerializeField] protected float m_StaminaGenerationAmount;

    [SerializeField] MeshRenderer m_meshRenderer;
    [SerializeField] protected SO_ComboList m_comboList;
    [SerializeField] protected AnimationHandler m_animationHandler;

    public float Health { protected set; get; }
    public float Stamina { protected set; get; }

    public KSB_IEventManager EventManager { private set; get; }
    public FT_AbilityUser AbilityUser { private set; get; }
    public FT_ComboHandler ComboHandler { private set; get; }
    public MovementComponent MovementComponent { private set; get; }

    //Disabled by default.
    private bool m_bFreezeMovement = true;
    private bool m_bFreezeJump = true;
    private bool m_bFreezeAbility = true;
    private bool m_bFreezeStaminaGain = true;
    private bool m_bFreezeHitSensor = true;
    private float m_staminaGenerationCounter;

    protected float m_MaxHealth;
    protected float m_MaxStamina;

    public bool bGodMode = false;

    private void Awake()
    {
        EventManager = GameUtil.EventManager;
        MovementComponent = GetComponent<MovementComponent>();
        AbilityUser = GetComponent<FT_AbilityUser>();

        EventManager.RegisterSubscriber(HandleMoveEvent, FT_MoveEvent.EventID);
        EventManager.RegisterSubscriber(HandleJumpEvent, FT_JumpEvent.EventID);
        EventManager.RegisterSubscriber(HandlePlayAnimationEvent, FT_PlayAnimationEvent.EventID);
        EventManager.RegisterSubscriber(HandleFreezeActionEvent, FT_FreezeFighterActionEvent.EventID);

        EventManager.RegisterSubscriber(HandleAbilityEvent, FT_AbilityEvent.EventID);

        EventManager.RegisterSubscriber(HandleChangeStaminaEvent, FT_ChangeStaminaEvent.EventID);

        m_MaxHealth = GameUtil.PlayerState.MaxHealth;
        m_MaxStamina = GameUtil.PlayerState.MaxStamina;

        Health = m_MaxHealth;
        Stamina = m_MaxStamina;
        m_staminaGenerationCounter = m_staminaGenerationTime;

        ComboHandler = new FT_ComboHandler(m_comboList.GetComboActionSequence(),this);

        OnStart();
    }

    private void Update()
    {
        if (!m_bFreezeStaminaGain)
        {
            m_staminaGenerationCounter -= Time.deltaTime;

            if (m_staminaGenerationCounter < 0.0f)
            {
                EventManager.AddEvent(new FT_ChangeStaminaEvent(this, m_StaminaGenerationAmount));
                m_staminaGenerationCounter = m_staminaGenerationTime;
            }
        }

        OnUpdate();
        ComboHandler.Update();
    }
    
    protected abstract void OnStart();

    protected virtual void OnUpdate()
    {
        if (m_animationHandler!=null && m_animationHandler.IsAnimationPlaying("running"))
        {
            if (GetComponent<Rigidbody>().velocity.sqrMagnitude < 4)
                m_animationHandler.StartAnimation("idle");
        }
    }

	protected void SetHealth(float changeAmount) { Health += changeAmount; Health = Mathf.Clamp(Health, 0, m_MaxHealth); }
    public float GetMaxHealth() { return m_MaxHealth; }

    protected void SetStamina(float changeAmount) { Stamina += changeAmount; Stamina = Mathf.Clamp(Stamina, 0, m_MaxStamina); }
    public float GetMaxStamina() { return m_MaxStamina; }

    public void AddForce(Vector3 forceDirection, float forceAmount)
    {
        MovementComponent.ApplyForce(forceDirection, forceAmount);
    }

    public Transform GetTransform() { return gameObject.transform; }

    public Bounds GetMeshBounds(){ return m_meshRenderer.bounds; }

    public abstract int GetID();

    public bool CanGetHit() { return !m_bFreezeHitSensor; }

    private void HandleMoveEvent(KSB_IEvent gameEvent)
    {
        if(m_bFreezeMovement) {return;}

        FT_MoveEvent moveEvent = (FT_MoveEvent)gameEvent;
        if(moveEvent.Fighter.GetID() != GetID()) { return; }

        MovementComponent.SetMovementDirection(moveEvent.Direction);
        if(m_animationHandler)
        m_animationHandler.StartAnimation("running");
    }

    private void HandleJumpEvent(KSB_IEvent gameEvent)
    {
        if(m_bFreezeJump) {return;}

        FT_JumpEvent moveEvent = (FT_JumpEvent)gameEvent;
        if(moveEvent.Fighter.GetID() != GetID()) { return; }

        //TODO: Move this somwhere it makes more sense
        GameUtil.SoundManager.PlaySoundAtPosition("Jump", gameObject.transform.position);
        
        MovementComponent.Jump();
    }

    private void HandleAbilityEvent(KSB_IEvent Event)
    {
        if(m_bFreezeAbility) {return;}
        //AbilityUser = GetComponent<FT_AbilityUser>();

        FT_AbilityEvent abilityEvent = (FT_AbilityEvent)Event;
          
        if (abilityEvent.Fighter.GetID() != GetID()) { return; }
        if(!AbilityUser.HasAbility(abilityEvent.AbilityName)) { return; }

        float staminaConsumption = AbilityUser.GetAbility(abilityEvent.AbilityName).StaminaCost;
        if (Stamina < staminaConsumption) { return; }
        
        AbilityUser.ExecuteAbility(abilityEvent.AbilityName, abilityEvent.AbilityDirection, abilityEvent.AbilityState);
        EventManager.AddEvent(new FT_ChangeStaminaEvent(this, -staminaConsumption));
    }

    private void HandleChangeStaminaEvent(KSB_IEvent gameEvent)
    {
        FT_ChangeStaminaEvent staminaChangeEvent = (FT_ChangeStaminaEvent)gameEvent;
        if(staminaChangeEvent.Fighter.GetID() != GetID()) { return; }

        SetStamina(staminaChangeEvent.ChangeAmount);
    }

    private void HandleFreezeActionEvent(KSB_IEvent gameEvent)
    {
        FT_FreezeFighterActionEvent freezeEvent = (FT_FreezeFighterActionEvent)gameEvent;

        if (freezeEvent.Fighter.GetID() != GetID()) { return; }

        m_bFreezeMovement = freezeEvent.bFreezeMovement;
        m_bFreezeJump = freezeEvent.bFreezeJump;
        m_bFreezeAbility = freezeEvent.bFreezeAbility;
        m_bFreezeStaminaGain = freezeEvent.bFreezeStaminaGain;
        m_bFreezeHitSensor=freezeEvent.bFreezeHitSensor;
    }

    private void HandlePlayAnimationEvent(KSB_IEvent gameEvent)
    {
        FT_PlayAnimationEvent animationEvent = (FT_PlayAnimationEvent)gameEvent;
        if (animationEvent.Fighter == this)
        {
            if (m_animationHandler!=null)
                m_animationHandler.StartAnimation(animationEvent.AnimationAction, animationEvent.KeyFrameCallback);
        }
    }


    public virtual void UnRegisterToEventSystem()
    {
        ComboHandler.UnRegisterToEventSystem();

        EventManager.UnRegisterSubscribers(
        HandleMoveEvent,
        HandleJumpEvent,
        HandleAbilityEvent,
        HandleChangeStaminaEvent,
        HandleFreezeActionEvent,
        HandlePlayAnimationEvent);
    }
}
}