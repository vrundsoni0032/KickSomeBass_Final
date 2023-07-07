using UnityEngine;

namespace Fight.Loadout.LoadoutItem
{
[CreateAssetMenu(fileName = "ItemFishOil", menuName = "KSBUtilities/Fight/Items/FishOil")]
public class Item_FishOil : Item
{
    [UnityEngine.SerializeField] float healthAmount;

    public override void Consume(PlayerState fighterState)
    {
        GameUtil.EventManager.AddEvent(new Events.FT_ChangeHealthEvent(FightUtil.GetPlayer(), healthAmount));
    }
}
}
