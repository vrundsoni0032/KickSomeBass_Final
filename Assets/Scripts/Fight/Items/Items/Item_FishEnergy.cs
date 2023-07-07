using UnityEngine;

namespace Fight.Loadout.LoadoutItem
{
[CreateAssetMenu(fileName = "ItemFishEnergy", menuName = "KSBUtilities/Fight/Items/FishEnergy")]
public class Item_FishEnergy : Item
{
    public override void Consume(PlayerState fighterState)
    {
        GameUtil.EventManager.AddEvent(new Events.FT_ChangeStaminaEvent(FightUtil.GetPlayer(), fighterState.MaxStamina));
    }
}
}