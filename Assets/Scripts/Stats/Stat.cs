using UnityEngine;

[CreateAssetMenu(fileName = "UnitStats", menuName = "ScriptableObjects/UnitStats", order = 2)]
public class Stat : ScriptableObject
{
    public string unit_name;
    public uint unit_armor;
    public uint unit_maxhealth;
    public uint unit_strength;
    public uint unit_agility;
    public uint unit_intelligence;

    public string[] unit_abilities;
}
