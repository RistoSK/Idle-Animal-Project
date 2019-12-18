using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private List<ShopAnimalHud> _purchasableAnimalList;

    private readonly Dictionary<AnimalAttributes, ShopAnimalHud> _purchasableAnimals = new Dictionary<AnimalAttributes, ShopAnimalHud>();
    
    private void Awake()
    {
        int purchasableAnimalsCount = _purchasableAnimalList.Count;

        for (int i = 0; i < purchasableAnimalsCount; i++)
        {
            var shopAnimalHud = _purchasableAnimalList[i];
            
            shopAnimalHud.OnPurchaseButtonPressed += TryToPurchaseNewAnimal;
            shopAnimalHud.OnCapacityUpgradeButtonPressed += TryToPurchaseCapacityUpgrade;
            shopAnimalHud.OnGoldPerUpgradeButtonPressed += TryToPurchaseGoldPerUpgrade;
            
            _purchasableAnimals.Add(shopAnimalHud.AnimalAttribute, shopAnimalHud);
        }
    }

    private void TryToPurchaseCapacityUpgrade(AnimalAttributes animalAttribute)
    {
        if (PurseManager.Instance.TryToPurchase(animalAttribute.CapacityUpgradeCost))
        {
            CapacityUpgradePurchaseSuccessful(animalAttribute);
        }
    }

    private void TryToPurchaseGoldPerUpgrade(AnimalAttributes animalAttribute)
    {
        if (PurseManager.Instance.TryToPurchase(animalAttribute.ResourceIncreaseUpgradeCost))
        {
            GoldPerUpgradePurchaseSuccessful(animalAttribute);
        }
    }

    private void TryToPurchaseNewAnimal(AnimalAttributes animalAttribute)
    {
        if (PurseManager.Instance.TryToPurchase(animalAttribute.PurchaseAnimalCost))
        {
            AnimalManager.Instance.OnAnimalPurchasedSuccessfully(animalAttribute);
            
            animalAttribute.IsUnlocked = true;
            _purchasableAnimals[animalAttribute].UnlockAnimal();
        }
    }
    
    private void CapacityUpgradePurchaseSuccessful(AnimalAttributes animalAttribute)
    {
        animalAttribute.ResourceCapacity += animalAttribute.CapacityIncrementalAmount;
        
        if (_purchasableAnimals.TryGetValue(animalAttribute, out var purchasableAnimal))
        {
            purchasableAnimal.UpgradeShopHudCapacityValues();
        }
        else
        {
            Debug.LogError($"Tried to upgrade capacity shop description of {animalAttribute.name} but it hasn't been assigned to our initialization list");
        }
    }

    private void GoldPerUpgradePurchaseSuccessful(AnimalAttributes animalAttribute)
    {
        animalAttribute.ResourcePerCooldown += animalAttribute.ResourceUpgradeIncrementalAmount;
        
        if (_purchasableAnimals.TryGetValue(animalAttribute, out var purchasableAnimal))
        {
            purchasableAnimal.UpgradeShopHudGoldPerValues();
        }
        else
        {
            Debug.LogError($"Tried to upgrade Resource per second shop description of {animalAttribute.name} but it hasn't been assigned to our initialization list");
        }
    }
}
