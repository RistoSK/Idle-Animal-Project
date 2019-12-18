using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class ShopAnimalHud : MonoBehaviour
{
    [SerializeField] private AnimalAttributes _animalAttribute;

    [SerializeField] private GameObject _lockedGameObject;
    [SerializeField] private GameObject _unlockedGameObject;
    
    [Header("Locked Texts")] 
    [SerializeField] private TextMeshProUGUI _animalUnlockCostText;
    [SerializeField] private TextMeshProUGUI _capacityAmount;
    [SerializeField] private TextMeshProUGUI _cooldownAmount;
    [SerializeField] private TextMeshProUGUI _resourceGainAmount;

    [Header("Unlocked Texts")] 
    [SerializeField] private TextMeshProUGUI _capacityUpgradeCostText;
    [SerializeField] private TextMeshProUGUI _resourceGainUpgradeCostText;
    [SerializeField] private TextMeshProUGUI _capacityUpgradeDescriptionText;
    [SerializeField] private TextMeshProUGUI _resourceGainUpgradeDescriptionText;
    
    public Action<AnimalAttributes> OnPurchaseButtonPressed;
    public Action<AnimalAttributes> OnCapacityUpgradeButtonPressed;
    public Action<AnimalAttributes> OnGoldPerUpgradeButtonPressed;

    public AnimalAttributes AnimalAttribute => _animalAttribute;

    private void Start()
    {
        UpdateShopAnimalHud();

        Game.Instance.OnAnimalShopHudUpdated += UpdateShopAnimalHud;
    }

    private void UpdateShopAnimalHud()
    {
        _animalUnlockCostText.text = _animalAttribute.PurchaseAnimalCost.ToString();
        _capacityAmount.text = _animalAttribute.ResourceCapacity.ToString(CultureInfo.InvariantCulture);
        _cooldownAmount.text = _animalAttribute.ResourceCooldownTime.ToString(CultureInfo.InvariantCulture);
        _resourceGainAmount.text = _animalAttribute.ResourcePerCooldown.ToString(CultureInfo.InvariantCulture);

        UpgradeShopHudCapacityValues();
        UpgradeShopHudGoldPerValues();

        if (_animalAttribute.IsUnlocked)
        {
            _unlockedGameObject.SetActive(true);
            _lockedGameObject.SetActive(false);
        }
        else
        {
            _unlockedGameObject.SetActive(false);
            _lockedGameObject.SetActive(true);
        }
    }

    public void PurchaseAnimal()
    {
        OnPurchaseButtonPressed?.Invoke(_animalAttribute);
    }

    public void PurchaseCapacityUpgrade()
    {
        OnCapacityUpgradeButtonPressed?.Invoke(_animalAttribute);
    }

    public void PurchaseGoldPerUpgrade()
    {
        OnGoldPerUpgradeButtonPressed?.Invoke(_animalAttribute);
    }
    
    public void UnlockAnimal()
    {
        _unlockedGameObject.SetActive(true);
        _lockedGameObject.SetActive(false);
    }

    public void LockAnimal()
    {
        _unlockedGameObject.SetActive(false);
        _lockedGameObject.SetActive(true);
    }
    

    public void UpgradeShopHudCapacityValues()
    {
        float upgradedCapacityAmount = _animalAttribute.ResourceCapacity + _animalAttribute.CapacityIncrementalAmount;
        _capacityUpgradeDescriptionText.text = $"{_animalAttribute.ResourceCapacity} -> {upgradedCapacityAmount}";
        _capacityUpgradeCostText.text = _animalAttribute.CapacityUpgradeCost.ToString();
    }
    
    public void UpgradeShopHudGoldPerValues()
    {
        float upgradedResourceAmount = _animalAttribute.ResourcePerCooldown + _animalAttribute.ResourceUpgradeIncrementalAmount;
        _resourceGainUpgradeDescriptionText.text = $"{_animalAttribute.ResourcePerCooldown} -> {upgradedResourceAmount}";
        _resourceGainUpgradeCostText.text = _animalAttribute.ResourceIncreaseUpgradeCost.ToString();
    }
}
