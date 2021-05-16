using UnityEngine;

public class UnitStats : MonoBehaviour
{
    [SerializeField] private Stat stats;
    [SerializeField] private Stat.UnitTypes classType;
    public uint health;
    public uint armor;
    public uint strength;
    public uint agility;
    public uint intelligence;
    private void Start()
    {
        classType = stats.unitType;
        gameObject.name = stats.unitName;
        health = stats.unitMaxHealth;
        armor = stats.unitArmor;
        strength = stats.unitStrength;
        agility = stats.unitAgility;
        intelligence = stats.unitIntelligence;
        //TODO 
        //load abilities on start
        Debug.Log
        (
            "My class is: " + classType +
            " My stats are: " +
                  " name: " + gameObject.name +
                  " max health: " + health +
                  " armor: " + armor + 
                  " strength: " + strength  +
                  " agility: " + agility  +
                  " intelligence: " + intelligence
        );
    }

    public void UpdateStats()
    {
        // Update statistics?
    }
}
