using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimalHud : MonoBehaviour
{
    [SerializeField] private AnimalAttributes _animalAttribute;
    [SerializeField] private Transform _capacityBarTransform;
    [SerializeField] private Image _harvestCooldownImage;
    [SerializeField] private TextMeshPro _currentPoints;
    [SerializeField] private TextMeshProUGUI _resourceCooldownTime;
    
    private float _harvestCooldown;
    private bool _maximumCapacityReached;
    
    public Action<AnimalAttributes> OnCollectButtonPressed;
    public AnimalAttributes AnimalAttribute => _animalAttribute;

    private void Start()
    {
        _currentPoints.text =  _animalAttribute.CurrentResources.ToString();
        _capacityBarTransform.localScale = new Vector3(1 - ( _animalAttribute.CurrentResources / _animalAttribute.ResourceCapacity), 1, 0);
        _harvestCooldownImage.fillAmount = _harvestCooldown / _animalAttribute.ResourceCooldownTime;
        _resourceCooldownTime.text = _animalAttribute.ResourceCooldownTime.ToString(CultureInfo.InvariantCulture);
    }
    
    private void Update()
    {
        if (_maximumCapacityReached)
        {
            return;
        }
        
        if (_harvestCooldown > _animalAttribute.ResourceCooldownTime)
        {
            _animalAttribute.CurrentResources += _animalAttribute.ResourcePerCooldown;
            _harvestCooldown = 0;
            
            if (_animalAttribute.CurrentResources >= _animalAttribute.ResourceCapacity)
            {
                AllResourcesGathered();
                return;
            }
            
            _currentPoints.text =  _animalAttribute.CurrentResources.ToString(CultureInfo.InvariantCulture);
            _capacityBarTransform.localScale = new Vector3(1 -  (_animalAttribute.CurrentResources / _animalAttribute.ResourceCapacity), 1, 0);
        }
        
        _harvestCooldownImage.fillAmount = _harvestCooldown / _animalAttribute.ResourceCooldownTime;
        _harvestCooldown += Time.deltaTime;
    }

    public void ReduceCurrentHarvestCooldown(float timeToAdd)
    {
        float rounds = Mathf.Floor(timeToAdd / _animalAttribute.ResourceCooldownTime);
        float timeRemaining = timeToAdd % _animalAttribute.ResourceCooldownTime;

        _animalAttribute.CurrentResources += _animalAttribute.ResourcePerCooldown * rounds;
        
        if (_animalAttribute.CurrentResources > _animalAttribute.ResourceCapacity)
        {
            AllResourcesGathered();
        }
        else
        {
            _harvestCooldown += timeRemaining;
            
            _currentPoints.text =  _animalAttribute.CurrentResources.ToString(CultureInfo.InvariantCulture);
            _capacityBarTransform.localScale = new Vector3(1 -  (_animalAttribute.CurrentResources / _animalAttribute.ResourceCapacity), 1, 0);
            _harvestCooldownImage.fillAmount = _harvestCooldown / _animalAttribute.ResourceCooldownTime;
        }
    }

    private void AllResourcesGathered()
    {
        _animalAttribute.CurrentResources = _animalAttribute.ResourceCapacity;
        _currentPoints.text = _animalAttribute.ResourceCapacity.ToString();
        _capacityBarTransform.localScale = new Vector3(0, 1, 0);
        _harvestCooldownImage.fillAmount = 1f;
        _maximumCapacityReached = true;
    }

    public void OnPointsCollected()
    {
        OnCollectButtonPressed?.Invoke(_animalAttribute);
    }

    public void ResetHudValues()
    {
        _capacityBarTransform.localScale = new Vector3(1f , 1f, 0f);
        _currentPoints.text = "0";
        _maximumCapacityReached = false;
    }

    public void ActivateHud()
    {
        gameObject.SetActive(true);
    }
}
