using System.Collections.Generic;
using ShopSystem;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerState", menuName = "KSBUtilities/PlayerState")]
public class PlayerState : ScriptableObject
{
    // Player level, Skill points & Experience points

    [SerializeField] private int PlayerLevel = 1;
    public int SkillPoints = 0;

    public int ExperiencePoints = 0;
    public int MaxExperiencePoints = 100;
    [SerializeField] private float m_experienceGainPercent = 100;

    public void IncrementPlayerLevel()
    {
        PlayerLevel++;
        MaxExperiencePoints += (int)(MaxExperiencePoints * (m_experienceGainPercent / 100.0f));
    }
    public int GetPlayerLevel() => PlayerLevel;



    // Fish bucks, inventory, loadout

    public List<InventoryItem> InventoryItems;
    public List<InventoryItem> LoadoutItems;

    public int FishBucks = 200;



    // Player Spawn Area, story events
    
    [SerializeField] public Vector3 PlayerMapSpawnPosition = new Vector3(-26.0f, -1.6f, -7.0f);
    [SerializeField] public Vector3 PlayerMapSpawnLookRotation = new Vector3(0.0f, 0.0f, 0.0f);

    public void SetSpawnLocation(Transform newSpawnPoint)
    {
        PlayerMapSpawnPosition = newSpawnPoint.position;
        PlayerMapSpawnLookRotation = newSpawnPoint.forward;
    }

    [SerializeField] private bool[] CurrentPlayerStoryEvent = new bool[(int)PlayerStoryEvent.MaxNumber];

    public void SetPlayerStoryEvent(PlayerStoryEvent storyEvent, bool value = true) =>
        CurrentPlayerStoryEvent[(int)storyEvent] = value;

    public bool GetPlayerStoryEvent(PlayerStoryEvent storyEvent) => CurrentPlayerStoryEvent[(int)storyEvent];



    // Player Fight health, stamina, speed, strength, damage, Ability

    public float MaxHealth = 100;
    public float MaxStamina = 100;
    public float MaxSpeed = 10;
    public float MaxStrength = 100;
    public float ReducedDamage; //Amount of damage sheiled in %"

    public List<string> AbilityList = new List<string>{ "Punch", "Dash", "BootThrow", "StaminaGain"};

    public void AddReducedDamage(float aPercent) { ReducedDamage = Mathf.Clamp(ReducedDamage + aPercent, 0, 1); }
    public float GetReduceDamagePercent() => ReducedDamage;
}