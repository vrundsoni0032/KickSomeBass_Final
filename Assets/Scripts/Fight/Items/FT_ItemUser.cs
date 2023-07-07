using ShopSystem;
using System.Collections.Generic;
using UnityEngine;

namespace Fight.Loadout.LoadoutItem
{
public class FT_ItemUser : MonoBehaviour
{
    Fighter.FT_Fighter m_fighter;

    [SerializeField] List<Item> m_SOItemList;

    private void Start()
    {
        m_fighter = GetComponent<Fighter.FT_Fighter>();
    }
    public void UseItem(string aItemName)
    {
        Item item = m_SOItemList.Find(i => i.GetName().Equals(aItemName));

        if (item == null) return;

        item.Consume(GameUtil.PlayerState);
    }
}
}