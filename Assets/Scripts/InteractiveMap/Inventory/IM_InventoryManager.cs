using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ShopSystem;
using InteractiveMap.Events;
using InteractiveMap.ShopSystem;
using Core.EventSystem;
using Core;

namespace InteractiveMap.Inventory
{ 
public class IM_InventoryManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> ItemUIObjects;
    private GridLayoutGroup GridLayoutGroup;

    [SerializeField] private IM_ShopManager m_ShopManager;

    [Header("INVENTORY SCREEN PREFABS")]
    [SerializeField] private GameObject m_InventoryGridMenu;
    [SerializeField] private GameObject m_ShopItemPrefab;
    [SerializeField] private Transform m_InventoryContent;
    [SerializeField] private GameObject m_InventoryItemPopupUI;
    
    private void Start()
    {
        ItemUIObjects = new List<GameObject>();

        GridLayoutGroup = m_InventoryContent.GetComponent<GridLayoutGroup>();

        GameUtil.EventManager.RegisterSubscriber(HandleAddInventoryItemEvent, IM_AddInventoryItemEvent.EventID);
        GameUtil.EventManager.RegisterSubscriber(HandleToggleInventoryEvent, IM_ToggleInventoryEvent.EventID);
        GameUtil.EventManager.RegisterSubscriber(HandleOverDrawnCanvasEvent, KSB_EscapeEvent.EventID);
        GameUtil.EventManager.RegisterSubscriber(UnRegisterToEventSystem, KSB_EndSceneEvent.EventID);

        GenerateItems();
    }

    private void GenerateItems()
    {
        if(GameUtil.PlayerState.InventoryItems.Count > 0)
        { 
            foreach (InventoryItem i in GameUtil.PlayerState.InventoryItems)
            {
                GameObject item = Instantiate(m_ShopItemPrefab);
                i.UIObject = item;

                item.name = i.GetName();

                foreach (Transform child in item.transform)
                {
                    switch (child.gameObject.name)
                    {
                        case "ItemQuantity":
                            child.gameObject.GetComponent<Text>().text = i.GetQuantity().ToString();
                            break;

                        case "ItemImage":
                            child.gameObject.GetComponent<Image>().sprite = i.GetSprite();
                            child.gameObject.GetComponent<Image>().GetComponent<Button>().onClick.AddListener(() => OpenPopUp(i));
                            break;

                        case "ItemName":
                            child.gameObject.GetComponent<Text>().text = i.GetName();
                            break;
                    }
                }
                ItemUIObjects.Add(item);
            }
        }
    }

    private GameObject GenerateSingleItem(InventoryItem aItem)
    {
        GameObject item = Instantiate(m_ShopItemPrefab);
        aItem.UIObject = item;

        item.name = aItem.GetName();

        foreach (Transform child in item.transform)
        {
            switch (child.gameObject.name)
            {
                case "ItemQuantity":
                    child.gameObject.GetComponent<Text>().text = aItem.GetQuantity().ToString();
                    break;

                case "ItemImage":
                    child.gameObject.GetComponent<Image>().sprite = aItem.GetSprite();
                    child.gameObject.GetComponent<Image>().GetComponent<Button>().onClick.AddListener(() => OpenPopUp(aItem));
                    break;

                case "ItemName":
                    child.gameObject.GetComponent<Text>().text = aItem.GetName();
                    break;
            }
        }
        return item;
    }

    private void InitInventoryItems(List<InventoryItem> aInventoryItems)
    {
        if (ItemUIObjects.Count > 0)
        {
            foreach (GameObject aItem in ItemUIObjects)
            {
                aItem.transform.SetParent(GridLayoutGroup.transform);
            }
        }
    }

    private void UnInitInventoryItems()
    {
        foreach(Transform shopItem in m_InventoryContent)
        {
            Destroy(shopItem.gameObject);
        }
    }

    private void OpenInventory()
    {
        if (IM_Core.isAnyCanvasActive) { return; }

        //UnInitInventoryItems();
        GameUtil.SoundManager.PlaySound("ButtonPop");

        InitInventoryItems(GameUtil.PlayerState.InventoryItems);

        IM_Core.isAnyCanvasActive = true;
        m_InventoryGridMenu.SetActive(true);
    }

    private void CloseInventory()
    {
        ClosePopUp();
        GameUtil.SoundManager.PlaySound("ButtonPop");
        //IM_Core.isAnyCanvasActive = false;
        m_InventoryGridMenu.SetActive(false);
    }

    private void OpenPopUp(InventoryItem item)
    {
        if (!m_InventoryItemPopupUI.activeSelf)
            m_InventoryItemPopupUI.SetActive(true);

        foreach (Transform child in m_InventoryItemPopupUI.transform)
        {
            switch (child.gameObject.name)
            {
                case "ItemName":
                    child.gameObject.GetComponent<Text>().text = item.GetName();
                    break;

                case "ItemImage":
                    child.gameObject.GetComponent<Image>().sprite = item.GetSprite();
                    break;

                case "ItemDescp":
                    child.gameObject.GetComponent<Text>().text = item.GetDescription();
                    break;

                case "CloseButton":
                    child.gameObject.GetComponent<Button>().onClick.AddListener(() => ClosePopUp());
                    break;
            }
        }
    }

    private void ClosePopUp() => m_InventoryItemPopupUI.SetActive(false);

    private void HandleToggleInventoryEvent(KSB_IEvent gameEvent)
    {
        if(IM_Core.isAnyCanvasActive) { return; }

        OpenInventory();
        GameUtil.EventManager.AddEvent(new KSB_MouseStateEvent(true));
        GameUtil.EventManager.AddEvent(new KSB_InterruptGameplayInputEvent(true));
    }

    public void HandleOverDrawnCanvasEvent(KSB_IEvent gameEvent)
    {
        if (m_InventoryGridMenu.activeSelf == false) { return; }

        CloseInventory();
        IM_Core.isAnyCanvasActive = false;
        GameUtil.EventManager.AddEvent(new KSB_MouseStateEvent(false));
        GameUtil.EventManager.AddEvent(new KSB_InterruptGameplayInputEvent(false));
    }

    public void HandleAddInventoryItemEvent(KSB_IEvent gameEvent)
    {
        IM_AddInventoryItemEvent addItemEvent = (IM_AddInventoryItemEvent)gameEvent;
        
        InventoryItem itemToAdd = addItemEvent.Item;

        InventoryItem itemFound = GameUtil.PlayerState.InventoryItems.Find(item => item != null && 
                                                                           item.GetName() == itemToAdd.GetName());

        GameObject ItemUIObj = ItemUIObjects.Find(i => i != null && i.name == itemToAdd.GetName());


        YCLogger.Error("Handle Add Inventory Item Event called");

        //for item coming from shop/chest
        if(!addItemEvent.bItemFromLoadout)
        {
            if (itemFound != null)
                itemFound.ChangeQuantity(1);
            else
                GameUtil.PlayerState.InventoryItems.Add(addItemEvent.Item);

            //optimization 
            if(ItemUIObj != null)
            {
                foreach(Transform child in ItemUIObj.transform)
                {
                    if(child.gameObject.name == "ItemQuantity")
                    {
                        child.gameObject.GetComponent<Text>().text = itemFound.GetQuantity().ToString();
                    }
                }
            }
            else
            {
                ItemUIObjects.Add(GenerateSingleItem(itemToAdd));
            }
        }
        
        //for item coming back from loadout
        if(addItemEvent.bItemFromLoadout)
        {
            if (itemFound != null)
            { 
                itemFound.ChangeQuantity(addItemEvent.Item.GetQuantity());
            }
            else
            { 
                GameUtil.PlayerState.InventoryItems.Add(addItemEvent.Item);
            }    
            GameUtil.EventManager.AddEvent(new IM_UpdateLoadOutScreenEvent());
        }

        //decrement the price when item is added/bought from the shop
        GameUtil.PlayerState.FishBucks += addItemEvent.ItemPrice;
    }

    private void UnRegisterToEventSystem(KSB_IEvent gameEvent) =>
        GameUtil.EventManager.UnRegisterSubscribers(HandleToggleInventoryEvent, HandleOverDrawnCanvasEvent, HandleAddInventoryItemEvent);
}
}