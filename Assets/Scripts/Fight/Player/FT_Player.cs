using Core.Controller;
using Core.EventSystem;
using Fight.CameraSystem;
using Fight.Events;
using Fight.Fighter;
using Fight.Loadout.LoadoutItem;
using Fight.PlayerController;
using UnityEngine;

namespace Fight.Player
{
public class FT_Player : FT_Fighter
{
    GameObject m_camera;

    private KSB_IController m_playerController;
    private KSB_IController m_cameraController;

    public void Initialize(GameObject aCamera)
    {
        m_camera = aCamera;

        m_playerController = new FT_PlayerController(this, m_camera.transform);
        m_cameraController = new FT_CameraController(GameUtil.EventManager, this, m_camera);

        m_camera.GetComponent<Core.CameraSystem.TPSFollowCamera>().SetPrimaryTarget(gameObject);
    }

    protected override void OnStart()
    {
        EventManager.RegisterSubscriber(HandleChangeHealthEvent, FT_ChangeHealthEvent.EventID);
        EventManager.RegisterSubscriber(HandleUseItemEvent, FT_UseItemEvent.EventID);
    }

    public override int GetID() => 0;

    void HandleUseItemEvent(KSB_IEvent aEvent)
    {
        FT_UseItemEvent itemEvent = (FT_UseItemEvent)aEvent;

        if (GameUtil.PlayerState.LoadoutItems.Count-1 < itemEvent.ItemIndex) return;

        string itemName = GameUtil.PlayerState.LoadoutItems[itemEvent.ItemIndex].GetName();

        if (itemName != null)
        {
            if (GameUtil.PlayerState.LoadoutItems[itemEvent.ItemIndex].GetQuantity() > 0)
            {
                GetComponent<FT_ItemUser>().UseItem(itemName);
                GameUtil.PlayerState.LoadoutItems[itemEvent.ItemIndex].ChangeQuantity(-1);

                GameUtil.EventManager.AddEvent(new FT_ItemUsedEvent(itemEvent.ItemIndex, GameUtil.PlayerState.LoadoutItems[itemEvent.ItemIndex].GetQuantity()));
            }
        }
        else
        {
            YCLogger.Error("Player","Item not found");
        }
    }

    private void HandleChangeHealthEvent(KSB_IEvent gameEvent)
    {
        FT_ChangeHealthEvent healthChangeEvent = (FT_ChangeHealthEvent)gameEvent;
        if (healthChangeEvent.Fighter.GetID() != GetID()) { return; }

        float ChangeAmount = healthChangeEvent.ChangeAmount;

        //If health is being deducted check if player's health is being shielded

        if (healthChangeEvent.ChangeAmount < 0 && GetID() == 0)//TODO Temp
        {
            if (GameUtil.PlayerState.GetReduceDamagePercent() > 0)
            {
                float amountReduced = ChangeAmount * GameUtil.PlayerState.GetReduceDamagePercent();
                ChangeAmount += amountReduced;
            }
        }

        SetHealth(ChangeAmount);

        if (Health <= 0.0f && !bGodMode) 
        {
            if (m_animationHandler != null)
            {
                m_animationHandler.StartAnimation("idle");
            }
            EventManager.AddEvent(new FT_FighterLostEvent(this)); 
        }
    }

    public override void UnRegisterToEventSystem()
    {
        m_playerController.UnRegisterToEventSystem();
        m_cameraController.UnRegisterToEventSystem();
        EventManager.UnRegisterSubscribers(HandleUseItemEvent, HandleChangeHealthEvent);
    }
}
}