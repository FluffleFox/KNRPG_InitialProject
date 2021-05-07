using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractableType
{
    PICKABLE,    // e.g. Sword
    DESTROYABLE, // Barrel
    LOOTABLE     // Chest
}

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData", order = 2)]
public class ItemData : ScriptableObject
{
    [SerializeField] private InteractableType type;
    public InteractableType Type { get { return type; } }
    [SerializeField] private int rangeToInteract; // in hex units (e.g. 0 - same hex)
    public int RangeToInteract { get { return rangeToInteract; } }
}
