using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrefabData", menuName = "ScriptableObjects/RoomsEditor/PrefabData", order = 2)]
public class EditorPrefabsScriptable : ScriptableObject
{
    public enum PrefabRoomEditorType
    {
        DOOR,
        WALL
    }


    [SerializeField] private GameObject prefab;
    public GameObject Prefab { get { return prefab; } }
    [SerializeField] private PrefabRoomEditorType type;
    public PrefabRoomEditorType Type { get { return type; } }
    [SerializeField] private bool isNodeOccupied;
    public bool IsNodeOccupied { get { return isNodeOccupied; } }
    [SerializeField] private Vector3 instanitiateOffset;
    public Vector3 InstanitiateOffset { get { return instanitiateOffset; } }

    public static EditorPrefabsScriptable FindObjectByType(EditorPrefabsScriptable.PrefabRoomEditorType type, EditorPrefabsScriptable[] prefabs)
    {
        foreach (EditorPrefabsScriptable prefab in prefabs)
        {
            if (prefab.Type == type)
            {
                return prefab;
            }
        }
        return null;
    }
}

