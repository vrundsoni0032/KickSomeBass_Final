using UnityEngine;

namespace Fight.Loadout.LoadoutItem
{
[CreateAssetMenu(fileName = "ItemTunaCan", menuName = "KSBUtilities/Fight/Items/TunaCan")]
public class Item_TunaCan : Item
{
    [UnityEngine.SerializeField] float healthAmount;

    public override void Consume(PlayerState fighterState)
    {
        GameUtil.EventManager.AddEvent(new Events.FT_ChangeHealthEvent(FightUtil.GetPlayer(), healthAmount));
    }
}
}
