using Fight.Loadout.LoadoutItem;
using ShopSystem;
using UnityEngine;

namespace Fight.LoadoutItem
{
[CreateAssetMenu(fileName = "new AbilityItem", menuName = "KSBUtilities/Fight/Items/AbilityItem")]
public class AbilityItem : Item
{
    [UnityEngine.SerializeField] string ItemAbilityName;
    public string GetAbilityName() { return ItemAbilityName; }

    public override void Consume(PlayerState fighterState)
    {
        GameUtil.EventManager.AddEvent(new Events.FT_AbilityEvent(FightUtil.GetPlayer(), ItemAbilityName,
                                       FightUtil.GetRelativeDirection(FT_MoveDirection.Toward, FightUtil.GetPlayer().GetID())));
    }
}
}