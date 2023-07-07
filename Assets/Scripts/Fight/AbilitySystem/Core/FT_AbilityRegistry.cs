using System.Collections.Generic;
using UnityEngine;

namespace Fight.AbilitySystem
{
    [CreateAssetMenu(fileName = "AbilityRegistry", menuName = "KSBUtilities/Fight/Ability/Registry")]
    public class FT_AbilityRegistry : ScriptableObject
    {
        [SerializeField] private FT_Ability[] m_registry;

        public List<FT_Ability> GetAbilityList(List<string> abilityNames)
        {
            List<FT_Ability> abilitylist = new List<FT_Ability>();

            foreach (var abilityName in abilityNames)
            {
                foreach (FT_Ability ability in m_registry)
                {
                    if (ability.Name == abilityName)
                    {
                        abilitylist.Add(ability);
                        break;
                    }
                }
            }
            return abilitylist;
        }
    }
}