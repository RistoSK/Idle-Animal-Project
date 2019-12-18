using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    public static AnimalManager Instance;
    
    [SerializeField] private List<AnimalHud> _animalHudList;

    private readonly List<AnimalAttributes> _animalAttributes = new List<AnimalAttributes>();
    private readonly Dictionary<AnimalAttributes, AnimalHud> _animalHuds = new Dictionary<AnimalAttributes, AnimalHud>();

    public Action<float> OnResourceCollected;

    public List<AnimalAttributes> GetAllAnimalAttributes => _animalAttributes;
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        int animalHudsCount = _animalHudList.Count;

        for (int i = 0; i < animalHudsCount; i++)
        {
            var animalHud = _animalHudList[i];
            animalHud.OnCollectButtonPressed += AnimalHudResourcesCollected;

            CheckIfAnimalHasBeenUnlocked(animalHud);

            _animalHuds.Add(animalHud.AnimalAttribute, animalHud);
            _animalAttributes.Add(animalHud.AnimalAttribute);
        }
    }

    private void Start()
    {
        Game.Instance.OnPlayerLogin += UpdateAnimalHudsCooldowns;
    }

    private void UpdateAnimalHudsCooldowns(float deltaTimeSeconds)
    {
        int animalHudCount = _animalHudList.Count;

        for (int i = 0; i < animalHudCount; i++)
        {
            if (_animalHudList[i].AnimalAttribute.IsUnlocked)
            {
                _animalHudList[i].ReduceCurrentHarvestCooldown(deltaTimeSeconds);
            }
        }
    }

    private void CheckIfAnimalHasBeenUnlocked(AnimalHud animalHud)
    {
        animalHud.gameObject.SetActive(animalHud.AnimalAttribute.IsUnlocked);
    }

    private void AnimalHudResourcesCollected(AnimalAttributes animalAttributes)
    {
        OnResourceCollected?.Invoke(animalAttributes.CurrentResources);
        animalAttributes.CurrentResources = 0;
        _animalHuds[animalAttributes].ResetHudValues();
    }

    public void OnAnimalPurchasedSuccessfully(AnimalAttributes animalAttributes)
    {
        if (!_animalHuds.ContainsKey(animalAttributes))
        {
            Debug.LogError($"Tried to purchase {animalAttributes.name} but it hasn't been assigned to our initialization list");
            return;
        }

        _animalHuds[animalAttributes].ActivateHud();
    }
}
