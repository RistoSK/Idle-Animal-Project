using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Attributes", menuName = "Animal/Attributes")]
public class AnimalAttributes : ScriptableObject
{
    [Header("Initial Values")] 
    public int InitialResourceCapacity;
    public int InitialResourcePerCooldown;
    public bool InitialUnlockState;
    
    [Header("Unlockable Settings")] 
    public int PurchaseAnimalCost;
    public bool IsUnlocked;
    
    [Header("Harvesting Settings")] 
    public float CurrentResources;
    public float ResourceCapacity;
    public float ResourceCooldownTime;
    public int ResourcePerCooldown;

    [Header("Upgrading Settings")] 
    public int ResourceUpgradeIncrementalAmount;
    public int CapacityIncrementalAmount;

    [Header("Upgrading Costs")] 
    public int ResourceIncreaseUpgradeCost;
    public int CapacityUpgradeCost;
}
