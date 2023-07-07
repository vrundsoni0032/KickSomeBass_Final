using ShopSystem;
using System.Collections.Generic;
using UnityEngine;

public class FT_UIItemDisplay : MonoBehaviour
{
    [SerializeField]List<FT_UIItem> m_items;    //TODO: Temp ,change to spawn on runtime

    public void Initialize(List<InventoryItem> aItems)
    {
        for (int i = 0; i < aItems.Count; i++)
        {
            m_items[i].SetItem(aItems[i].GetSprite(), aItems[i].GetName(), aItems[i].GetQuantity());
        }
    }
    public void ItemUsed(int aIndex, int aNewQuantity)
    {
        if (aNewQuantity <= 0)
        {
            m_items[aIndex].ClearItem();
        }
        else
        {
            m_items[aIndex].SetQuantity(aNewQuantity);
        }
    }
}
