using UnityEngine;

namespace Fight.Loadout.LoadoutItem
{
[CreateAssetMenu(fileName = "ItemRainCoat", menuName = "KSBUtilities/Fight/Items/RainCoat")]
public class Item_RainCoat : Item
{
    [UnityEngine.Tooltip("Reduced damage amount in % 0-1")]
    [UnityEngine.SerializeField] float reducedDamage;
    [UnityEngine.SerializeField] bool isItemTemporary;
    [UnityEngine.SerializeField] float itemDuration = 1;

    public override void Consume(PlayerState fighterState)
    {
        fighterState.AddReducedDamage(reducedDamage);
        Debug.Log(fighterState.GetReduceDamagePercent());
        if (isItemTemporary)
        {
        }
    }

    void ResetItemsEffect(PlayerState aPlayerState)
    {
        aPlayerState.AddReducedDamage(-reducedDamage);
    }

}
}
