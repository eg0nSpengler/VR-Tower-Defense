using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Modifiers : MonoBehaviour
{
    [System.Flags]
    public enum WaveModifiers : int
    {
        None = 0,
        Healthy = 1, // 25% more hp
        Tough = 2, // 50% more hp
        Robust = 4,// double hp
        Large = 8, // 50% larger wave
        Horde = 16, // double wave size
        Army = 32 // triple
    }
    // Loot drop table that contains items that can spawn
    
    public GenericLootDropTableInt waveRarities;
    public GenericLootDropTableModifier modRarities;
    // How many mods do we wanna generate?
    public int numItemsToDrop;
    public void GetWaveModifiers()
    {
        GenericLootDropItem<int> selectedRarity = waveRarities.PickLootDropItem();
    }

    public void Start()
    {
        
    }
    [System.Serializable]
    public class Modifier
    {
        public int id;
        public string name;
        public string description;
    }
    public class ModifierInfo
    {

    }
}
