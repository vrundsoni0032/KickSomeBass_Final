using Core;
using UnityEngine;
using UnityEngine.UI;
using Core.EventSystem;
using InteractiveMap.Events;
using ShopSystem;
using System.Collections.Generic;

namespace InteractiveMap.ShopSystem
{
public class IM_ShopManager : MonoBehaviour
{
    public List<GameObject> ItemUIObjects;

    private GridLayoutGroup GridLayoutGroup;

    [SerializeField] private InventoryItem[] m_ShopItems;

    [Header("SHOP SCREEN PREFABS")]
    [SerializeField] private GameObject m_ShopBG;
    [SerializeField] private GameObject m_ShopGridMenu;
    [SerializeField] private GameObject m_ShopItemPrefab;
    [SerializeField] private Transform m_ShopContent;
    [SerializeField] private GameObject m_ShopItemPopupUI;

    private void Start()
    {
        YCLogger.Assert(this, m_ShopItems.Length < 15, "Items currently must not exceed count of *15*!");
        GameUtil.EventManager.RegisterSubscriber(HandleToggleShopEvent, IM_ToggleShopEvent.EventID);
        GameUtil.EventManager.RegisterSubscriber(HandleOverDrawnCanvasEvent, KSB_EscapeEvent.EventID);
        GameUtil.EventManager.RegisterSubscriber(UnRegisterToEventSystem, KSB_EndSceneEvent.EventID);

        GridLayoutGroup = m_ShopContent.GetComponent<GridLayoutGroup>();

        GenerateShopItemObjects();

        InitShopItems();
    }

    private void GenerateShopItemObjects()
    {
        foreach(InventoryItem i in m_ShopItems)
        {
            GameObject item = Instantiate(m_ShopItemPrefab);
            i.UIObject = item;
            
            item.transform.localScale = new Vector3 (1.05f, 1.05f, 1.05f);
            item.name = i.GetName();

            foreach (Transform child in item.transform)
            {
                switch (child.gameObject.name)
                {
                    case "ItemQuantity":
                        child.gameObject.SetActive(false);
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

    private void InitShopItems() //put them in the grid layout group
    {
        foreach (GameObject item in ItemUIObjects)
        {
            item.transform.SetParent(GridLayoutGroup.transform);
        }
    }

    private void OpenShopScreen()
    {
        m_ShopBG.SetActive(true);
        m_ShopGridMenu.SetActive(true);
    }

    public void CloseShopScreen()
    {
        m_ShopBG.SetActive(false);
        m_ShopGridMenu.SetActive(false);
        ClosePopUp();
    }

    private void OpenPopUp(InventoryItem item)
    {
        if (!m_ShopItemPopupUI.activeSelf)
            m_ShopItemPopupUI.SetActive(true);

        foreach (Transform child in m_ShopItemPopupUI.transform)
        {
            switch (child.gameObject.name)
            {
                case "ItemName":
                    child.gameObject.GetComponent<Text>().text = item.GetName();
                    break;

                case "ItemImage":
                    child.gameObject.GetComponent<Image>().sprite = item.GetSprite();
                    break;
                        
                case "ItemPrice":
                    child.gameObject.GetComponent<Text>().text = item.GetPrice().ToString();
                    break;

                case "ItemDescp":
                    child.gameObject.GetComponent<Text>().text = item.GetDescription();
                    break;

                case "BuyButton":
                    child.gameObject.GetComponent<Button>().onClick.RemoveAllListeners(); //unbind any existing listeners first
                    child.gameObject.GetComponent<Button>().onClick.AddListener(() => BuyItem(item));
                    break;

                case "CloseButton":
                    child.gameObject.GetComponent<Button>().onClick.AddListener(() => ClosePopUp());
                    break;
            }
        }
    }

    private void ClosePopUp()
    {
        m_ShopItemPopupUI.SetActive(false);

        foreach (Transform child in m_ShopItemPopupUI.transform)
        {
            if (child.gameObject.name == "BuyButton")
            {
                child.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            }
        }
    }

    private void BuyItem(InventoryItem item)
    {
        if (GameUtil.PlayerState.FishBucks < item.GetPrice()) return;

        // "GetChild(0)" - 0th child is the quantity text in the item prefab
        item.UIObject.transform.GetChild(0).GetComponent<Text>().text = item.GetQuantity().ToString();
        GameUtil.EventManager.AddEvent(new IM_AddInventoryItemEvent(item, -item.GetPrice()));
    }

    public void HandleToggleShopEvent(KSB_IEvent gameEvent)
    {
        if (IM_Core.isAnyCanvasActive) { return; }

        OpenShopScreen();
        GameUtil.EventManager.AddEvent(new KSB_MouseStateEvent(true));
        GameUtil.EventManager.AddEvent(new KSB_InterruptGameplayInputEvent(true));
        IM_Core.isAnyCanvasActive = true;

    }

    private void HandleOverDrawnCanvasEvent(KSB_IEvent gameEvent)
    {
        if(m_ShopGridMenu.activeSelf == false) { return; }

        CloseShopScreen();
        GameUtil.EventManager.AddEvent(new KSB_MouseStateEvent(false));
        GameUtil.EventManager.AddEvent(new KSB_InterruptGameplayInputEvent(false));
        IM_Core.isAnyCanvasActive = false;
        }

    private void UnRegisterToEventSystem(KSB_IEvent gameEvent) =>
        GameUtil.EventManager.UnRegisterSubscribers(HandleToggleShopEvent, HandleOverDrawnCanvasEvent);
}
}