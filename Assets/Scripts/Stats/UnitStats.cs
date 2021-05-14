using UnityEngine;

public class UnitStats : MonoBehaviour
{
    [SerializeField] private Stat stats;
    public uint health;
    public uint armor;
    public uint strength;
    public uint agility;
    public uint intelligence;
    void Start()
    {
        gameObject.name = stats.unit_name;
        health = stats.unit_maxhealth;
        armor = stats.unit_armor;
        strength = stats.unit_strength;
        agility = stats.unit_agility;
        intelligence = stats.unit_intelligence;
        //TODO 
        //load abilities on start
        Debug.Log
        (
            "My stats are: " +
                  " name: " + gameObject.name +
                  " max health: " + health +
                  " armor: " + armor + 
                  " strength: " + strength  +
                  " agility: " + agility  +
                  " intelligence: " + intelligence
        );
    }
}
