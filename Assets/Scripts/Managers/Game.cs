using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{ 
    public static Game Instance;
    
    private List<AnimalAttributes> _animalAttributesList = new List<AnimalAttributes>();
    private PurseAttribute _purseAttribute;
    private string _logOutTimeText;

    public Action<float> OnPlayerLogin;
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
    }

    private void Start()
    {
        _animalAttributesList = AnimalManager.Instance.GetAllAnimalAttributes;
        _purseAttribute = PurseManager.Instance.GetPurseAttribute;
        
        LoadGame();
    }

    private void OnApplicationQuit()
    {
        DateTime closingDateTime = DateTime.Now;
        _logOutTimeText = closingDateTime.ToString(CultureInfo.InvariantCulture);
        
        SaveGame();
    }

    public void ChangeScene()
    {
        SaveGame();
        SceneManager.LoadScene(1);
    }

    public void LoadGameScene()
    {
        DateTime closingDateTime = DateTime.Now;
        _logOutTimeText = closingDateTime.ToString(CultureInfo.InvariantCulture);
        
        SaveGame();
        LoadGame();  
        SceneManager.LoadScene(0);
        Debug.Log("Game was loaded LoadGameScene");
    }
    
    public void SaveGame()
    {
        Save save = CreateSaveGameObject();
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        var json = JsonUtility.ToJson(save);
        bf.Serialize(file, json);
        file.Close();
    }
    
    private Save CreateSaveGameObject()
    {
        Save save = new Save();

        int animalAttributesCount = _animalAttributesList.Count;
        
        for (int i = 0; i < animalAttributesCount; i++)
        {
            save.AnimalAttributeList.Add(_animalAttributesList[i]);
        }

        save.CurrentPurceGold = _purseAttribute.CurrentResources;
        save.LogOutTimeText = _logOutTimeText;
        
        return save;
    }

    private void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = new Save();
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), save);
            file.Close();

            int animalAttributesCount = save.AnimalAttributeList.Count;
            
            for (int i = 0; i < animalAttributesCount; i++)
            {
                _animalAttributesList[i].CurrentResources = save.AnimalAttributeList[i].CurrentResources;
                _animalAttributesList[i].ResourceCapacity = save.AnimalAttributeList[i].ResourceCapacity;
                _animalAttributesList[i].ResourcePerCooldown = save.AnimalAttributeList[i].ResourcePerCooldown;
                _animalAttributesList[i].IsUnlocked = save.AnimalAttributeList[i].IsUnlocked;
                _animalAttributesList[i].CapacityIncrementalAmount = save.AnimalAttributeList[i].CapacityIncrementalAmount;
                _animalAttributesList[i].CapacityUpgradeCost = save.AnimalAttributeList[i].CapacityUpgradeCost;
                _animalAttributesList[i].PurchaseAnimalCost = save.AnimalAttributeList[i].PurchaseAnimalCost;
                _animalAttributesList[i].ResourceCooldownTime = save.AnimalAttributeList[i].ResourceCooldownTime;
                _animalAttributesList[i].ResourceIncreaseUpgradeCost = save.AnimalAttributeList[i].ResourceIncreaseUpgradeCost;
                _animalAttributesList[i].ResourceUpgradeIncrementalAmount = save.AnimalAttributeList[i].ResourceUpgradeIncrementalAmount;
                _animalAttributesList[i].InitialResourceCapacity = save.AnimalAttributeList[i].InitialResourceCapacity;
                _animalAttributesList[i].InitialResourcePerCooldown = save.AnimalAttributeList[i].InitialResourcePerCooldown;
                _animalAttributesList[i].InitialUnlockState = save.AnimalAttributeList[i].InitialUnlockState;
            }

            _purseAttribute.CurrentResources = save.CurrentPurceGold;

            DateTime openingDateTime = DateTime.Now;
            String lastPlayTimeText = save.LogOutTimeText;
            
            if (DateTime.TryParse(lastPlayTimeText, out DateTime lastPlayTime))
            {
                var deltaTimeSeconds = openingDateTime.Subtract(lastPlayTime).TotalSeconds;
                deltaTimeSeconds = Math.Floor(deltaTimeSeconds);
                OnPlayerLogin?.Invoke((float)deltaTimeSeconds);
            }
        }
    }

    public void Reset()
    {
        int animalAttributesCount = _animalAttributesList.Count;
            
        for (int i = 0; i < animalAttributesCount; i++)
        {
            _animalAttributesList[i].CurrentResources = 0f;
            _animalAttributesList[i].ResourceCapacity = _animalAttributesList[i].InitialResourceCapacity;
            _animalAttributesList[i].ResourcePerCooldown = _animalAttributesList[i].InitialResourcePerCooldown;
            _animalAttributesList[i].IsUnlocked = _animalAttributesList[i].InitialUnlockState;
        }

        _purseAttribute.CurrentResources = 0f;
        
        LoadGameScene();
    }
}
