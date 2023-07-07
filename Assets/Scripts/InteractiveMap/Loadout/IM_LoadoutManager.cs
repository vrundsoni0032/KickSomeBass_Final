using Core;
using ShopSystem;
using UnityEngine;
using UnityEngine.UI;
using Core.EventSystem;
using InteractiveMap.Events;
using System.Collections.Generic;

public class IM_LoadoutManager : MonoBehaviour
{
    /* To open/close loadout screen you'll need to deal with both Inventory prefabs & 
     * Loadout prefabs, because I'm not smart enough to handle Unity's UI elements properly.
     * 
     * Sorry for the inconvinience.
     *
     * -Vrund Soni. */

    [Header("INVENTORY PREFABS")]
    [SerializeField] private GameObject m_InventoryGridMenu;
    [SerializeField] private GameObject m_ItemPrefab;
    [SerializeField] private Transform m_InventoryContent;

    [Header("LOADOUT PREFABS")]
    [SerializeField] private GameObject m_LoadoutGridMenu;
    [SerializeField] private GameObject m_LoadOutItemPrefab;
    [SerializeField] private Transform m_LoadOutContent;

    private KSB_CoreData FightCoreData;
    private int LoadOutIndex = 0;

    private void Start()
    {
        GameUtil.EventManager.RegisterSubscriber(HandleToggleLoadOutEvent, IM_ToggleLoadoutEvent.EventID);
        GameUtil.EventManager.RegisterSubscriber(HandleUpdateLoadoutScreenEvent, IM_UpdateLoadOutScreenEvent.EventID);

        GameUtil.EventManager.RegisterSubscriber(HandleOverDrawnCanvasEvent, KSB_EscapeEvent.EventID);
        GameUtil.EventManager.RegisterSubscriber(UnRegisterToEventSystem, KSB_EndSceneEvent.EventID);
    }
   
    private void OpenLoadOutScreen()
    {
        if (IM_Core.isAnyCanvasActive) { return; }

        UnInitInventoryItems();
        InitInventoryItems(GameUtil.PlayerState.InventoryItems);

        UnInitLoadOutItems();
        InitLoadOutItems(GameUtil.PlayerState.LoadoutItems);

        IM_Core.isAnyCanvasActive = true;

        m_InventoryGridMenu.SetActive(true);
        m_LoadoutGridMenu.SetActive(true);
    }

    private void CloseLoadOutScreen()
    {
        m_InventoryGridMenu.SetActive(false);
        m_LoadoutGridMenu.SetActive(false);

        //before clearing the loadout items, add all the items back to the inventory
        foreach (InventoryItem item in GameUtil.PlayerState.LoadoutItems)
        {
            GameUtil.EventManager.AddEvent(new IM_AddInventoryItemEvent(item));
        }

        foreach (Transform item in m_LoadOutContent)
        {
            Destroy(item.gameObject);
        }

        GameUtil.PlayerState.LoadoutItems.Clear();
        LoadOutIndex = 0;
    }

    private void UnInitInventoryItems()
    {
        foreach (Transform shopItem in m_InventoryContent)
        {
            Destroy(shopItem.gameObject);
        }
    }

    private void InitInventoryItems(List<InventoryItem> aInventoryItems)
    {
        foreach (InventoryItem i in aInventoryItems)
        {
            GameObject item = Instantiate(m_ItemPrefab, m_InventoryContent);
            i.UIObject = item;

            foreach (Transform child in item.transform)
            {
                switch (child.gameObject.name)
                {
                    case "ItemQuantity":
                        child.gameObject.GetComponent<Text>().text = i.GetQuantity().ToString();
                        break;

                    case "ItemImage":
                        child.gameObject.GetComponent<Image>().sprite = i.GetSprite();
                        child.gameObject.GetComponent<Image>().GetComponent<Button>().onClick.AddListener(() => AddItemToLoadout(i));
                        break;

                    case "ItemName":
                        child.gameObject.GetComponent<Text>().text = i.GetName();
                        break;
                }
            }
        }
    }

    private void UnInitLoadOutItems()
    {
        foreach (Transform item in m_LoadOutContent)
        {
            Destroy(item.gameObject);
        }
    }

    private void InitLoadOutItems(List<InventoryItem> aLoadoutItems)
    {
        foreach (InventoryItem i in aLoadoutItems)
        {
            GameObject item = Instantiate(m_LoadOutItemPrefab, m_LoadOutContent);
            i.UIObject = item;

            foreach (Transform child in item.transform)
            {
                switch (child.gameObject.name)
                {
                    case "ItemQuantity":
                        child.gameObject.GetComponent<Text>().text = i.GetQuantity().ToString();
                        break;

                    case "ItemImage":
                        child.gameObject.GetComponent<Image>().sprite = i.GetSprite();
                        child.gameObject.GetComponent<Image>().GetComponent<Button>().onClick.AddListener(() => AddItemBackToInventory(i));
                        break;

                    case "ItemName":
                        child.gameObject.GetComponent<Text>().text = i.GetName();
                        break;
                }
            }
        }
    }

    private void AddItemToLoadout(InventoryItem aItem)
    {
        if (aItem.GetQuantity() > 0)
        {
            InventoryItem itemInLoadout = GameUtil.PlayerState.LoadoutItems.Find(x => x.GetName() == aItem.GetName());
            InventoryItem itemInInventory = GameUtil.PlayerState.InventoryItems.Find(x => x.GetName() == aItem.GetName());

            //Check if already have it
            if (itemInLoadout != null)
            {
                itemInInventory.ChangeQuantity(-1);
                itemInLoadout.ChangeQuantity(1);
            }
            else
            {
                if(LoadOutIndex <= 3)
                {
                    itemInInventory.ChangeQuantity(-1);

                    InventoryItem tempItem = new InventoryItem(aItem);
                    
                    tempItem.SetIndexInLoadout(LoadOutIndex); //1st index starts at 0.
                    tempItem.ResetQuantity();
                    tempItem.ChangeQuantity(1);
                    
                    GameUtil.PlayerState.LoadoutItems.Add(tempItem);
                    LoadOutIndex++;
                }
            }
            RemoveItemFromInventory(aItem);
            UpdateLoadOutScreen();
        }
    }

    private void RemoveItemFromInventory(InventoryItem aItem)
    {
        if (aItem != null && aItem.GetQuantity() <= 0)
        {
            GameUtil.PlayerState.InventoryItems.Remove(aItem);
        }
    }

    public void AddItemBackToInventory(InventoryItem aItem)
    {
        GameUtil.EventManager.AddEvent(new IM_AddInventoryItemEvent(aItem, 0, true));
        GameUtil.PlayerState.LoadoutItems.Remove(aItem);

        if (LoadOutIndex > 0)
            LoadOutIndex--;
    }

    private void UpdateLoadOutScreen()
    {
        UnInitInventoryItems();
        InitInventoryItems(GameUtil.PlayerState.InventoryItems);

        UnInitLoadOutItems();
        InitLoadOutItems(GameUtil.PlayerState.LoadoutItems);
    }

    public void GoFight()
    {
        IM_Core.isAnyCanvasActive = false;

        GameUtil.PlayerState.SetSpawnLocation(((IM_Core)GameUtil.MapCore).Player.transform);
        GameUtil.EventManager.AddEvent(new KSB_EndSceneEvent("FT_Arena01", GameUtil.FightCore, FightCoreData));
    }

    private void HandleUpdateLoadoutScreenEvent(KSB_IEvent gameEvent)
    {
        UpdateLoadOutScreen();
    }

    private void HandleToggleLoadOutEvent(KSB_IEvent gameEvent)
    {
        if (IM_Core.isAnyCanvasActive) { return; }

        IM_ToggleLoadoutEvent loadoutEvent = (IM_ToggleLoadoutEvent) gameEvent;

        FightCoreData = loadoutEvent.CoreData;

        OpenLoadOutScreen();
        GameUtil.EventManager.AddEvent(new KSB_MouseStateEvent(true));
        GameUtil.EventManager.AddEvent(new KSB_InterruptGameplayInputEvent(true));
    }


    public void HandleOverDrawnCanvasEvent(KSB_IEvent gameEvent)
    {
        if(m_LoadoutGridMenu.activeSelf == false) { return; }

        CloseLoadOutScreen();
        IM_Core.isAnyCanvasActive = false;
        GameUtil.EventManager.AddEvent(new KSB_MouseStateEvent(false));
        GameUtil.EventManager.AddEvent(new KSB_InterruptGameplayInputEvent(false));
    }

    private void UnRegisterToEventSystem(KSB_IEvent gameEvent) =>
    GameUtil.EventManager.UnRegisterSubscribers(HandleToggleLoadOutEvent, HandleUpdateLoadoutScreenEvent, HandleOverDrawnCanvasEvent);
}