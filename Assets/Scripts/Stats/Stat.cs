using UnityEngine;

[CreateAssetMenu(fileName = "UnitStats", menuName = "ScriptableObjects/UnitStats", order = 2)]
public class Stat : ScriptableObject
{
    public enum UnitTypes
    {
        Warrior,
        Mage,
        Hunter
    }
    public UnitTypes unitType;
    public string unitName;
    public uint unitArmor;
    public uint unitMaxHealth;
    public uint unitStrength;
    public uint unitAgility;
    public uint unitIntelligence;

    public string[] unitAbilities;
}
