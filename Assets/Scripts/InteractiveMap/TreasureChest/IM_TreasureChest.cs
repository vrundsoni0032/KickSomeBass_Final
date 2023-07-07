using Core;
using InteractiveMap.Events;
using UnityEngine;
using ShopSystem;
using UnityEngine.UI;
using Core.EventSystem;

public enum TreasureType { FishBucks, Item }

public class IM_TreasureChest : MonoBehaviour, IM_Interactable
{
    [SerializeField] private TreasureType m_TreasureType;
    
    [SerializeField] private InventoryItem m_TreasureItem;
    [SerializeField] private int m_TreasureFishBucks;

    [SerializeField] private GameObject ChestItem;

    private void Start()
    {
        if (m_TreasureType == TreasureType.Item) { m_TreasureFishBucks = 0; }
        else if (m_TreasureType == TreasureType.FishBucks) { m_TreasureItem = null; }

        GameUtil.EventManager.RegisterSubscriber(HandleOverDrawnCanvasEvent, KSB_EscapeEvent.EventID);
        GameUtil.EventManager.RegisterSubscriber(UnRegisterToEventSystem, KSB_EndSceneEvent.EventID);
    }

    public void TriggerInteraction()
    {
        if(m_TreasureType == TreasureType.Item)
        {
            m_TreasureItem.ChangeQuantity(1);
            GameUtil.EventManager.AddEvent(new IM_AddInventoryItemEvent(m_TreasureItem));
        }
        else if(m_TreasureType == TreasureType.FishBucks)
        {
            GameUtil.PlayerState.FishBucks += m_TreasureFishBucks;
        }
        GameUtil.SoundManager.PlaySound("LevelUp");
        gameObject.SetActive(false);
        OpenItemPopup();
    }

    private void OpenItemPopup()
    {
        IM_Core.isAnyCanvasActive = true;
        GameUtil.EventManager.AddEvent(new KSB_MouseStateEvent(true));
        GameUtil.EventManager.AddEvent(new KSB_InterruptGameplayInputEvent(true));

        ChestItem.SetActive(true);

        if(m_TreasureType == TreasureType.Item)
        {
            foreach (Transform child in ChestItem.transform)
            {
                switch (child.gameObject.name)
                {
                    case "ItemImage":
                        child.gameObject.SetActive(true);
                        child.GetComponent<Image>().sprite = m_TreasureItem.GetSprite();
                        break;

                    case "ItemName":
                        child.gameObject.SetActive(true);
                        child.gameObject.GetComponent<Text>().text = m_TreasureItem.GetName();
                        break;

                    case "FishBucks":
                        child.gameObject.SetActive(false);
                        break;

                    case "CloseButton":
                        IM_Core.isAnyCanvasActive = false;
                        child.gameObject.GetComponent<Button>().onClick.AddListener(ClosePopup);
                        break;
                }
            }
        }

        if(m_TreasureType == TreasureType.FishBucks)
        {
            foreach (Transform child in ChestItem.transform)
            {
                switch (child.gameObject.name)
                {
                    case "ItemImage":
                        child.gameObject.SetActive(false);
                        break;

                    case "ItemName":
                        child.gameObject.SetActive(false);
                        break;

                    case "FishBucks":
                        child.gameObject.SetActive(true);
                        child.gameObject.GetComponent<Text>().text = m_TreasureFishBucks + "$";
                        break;

                    case "CloseButton":
                        child.gameObject.GetComponent<Button>().onClick.AddListener(ClosePopup);
                        break;
                }
            }
        }
    }

    private void ClosePopup()
    {
        if(ChestItem != null)
        {
            ChestItem.SetActive(false);
            GameUtil.EventManager.AddEvent(new KSB_MouseStateEvent(false));
            GameUtil.EventManager.AddEvent(new KSB_InterruptGameplayInputEvent(false));
        }
    }

    private void HandleOverDrawnCanvasEvent(KSB_IEvent gameEvent) => ClosePopup();

    private void UnRegisterToEventSystem(KSB_IEvent gameEvent) =>
        GameUtil.EventManager.UnRegisterSubscribers(HandleOverDrawnCanvasEvent);
}
