using UnityEngine;
using Core;
using Fight.AbilitySystem;

namespace Fight
{
    [CreateAssetMenu(menuName = "KSBUtilities/FightCoreData")]
    public class FT_CoreData : KSB_CoreData
    {
        public FightIndex FightIndex;
        public GameObject Player;
        public GameObject AI;
        public GameObject Arena;
        public FT_AbilityRegistry abilityRegistry;

        [Header ("Post Fight Win/Lose Rewards")]
        public int WinXpPoints;
        public int LoseXpPoints;
        
        public int WinSkillPoints;
        public int LoseSkillPoints;

        public int WinFishBucks;
        public int LoseFishBucks;
    }

    public enum FightIndex { TutorialFight, Area2Fight, Area3Fight, LighthouseFight }
}