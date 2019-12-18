using System.Collections.Generic;

[System.Serializable]
public class Save
{
    public List<AnimalAttributes> AnimalAttributeList = new List<AnimalAttributes>();
    public float CurrentPurceGold;
    public string LogOutTimeText;
}
