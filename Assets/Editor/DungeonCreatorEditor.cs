using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DungeonCreator))]
public class DungeonCreatorEditor : Editor
{
    private DungeonCreator creator;
    private bool addButtonToggled;
    private bool removeButtonToggled;
    private int prefabChoice;
    private int doorChoice;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        creator = (DungeonCreator)target;

        // Mode buttons
        GUILayout.BeginHorizontal();

        var defaultGUIColor = GUI.backgroundColor;
        GUI.backgroundColor = Color.green;

        addButtonToggled = GUILayout.Toggle(addButtonToggled, "ADD room mode", "Button");
        if (addButtonToggled && creator.Mode != DungeonCreator.DungeonCreatorMode.ADD)
        {
            Debug.Log("ADD mode ON !");
            creator.SetMode(DungeonCreator.DungeonCreatorMode.ADD);
            removeButtonToggled = false;
        }
        else if (!addButtonToggled && creator.Mode == DungeonCreator.DungeonCreatorMode.ADD)
        {
            Debug.Log("Default mode ON !");
            creator.SetMode(DungeonCreator.DungeonCreatorMode.DEFAULT);
            addButtonToggled = false;
            removeButtonToggled = false;
        }


        GUI.backgroundColor = Color.red;

        removeButtonToggled = GUILayout.Toggle(removeButtonToggled, "REMOVE room mode", "Button");
        if (removeButtonToggled && creator.Mode != DungeonCreator.DungeonCreatorMode.REMOVE)
        {
            Debug.Log("REMOVE mode ON !");
            creator.SetMode(DungeonCreator.DungeonCreatorMode.REMOVE);
            addButtonToggled = false;
        }
        else if (!removeButtonToggled && creator.Mode == DungeonCreator.DungeonCreatorMode.REMOVE)
        {
            Debug.Log("Default mode ON !");
            creator.SetMode(DungeonCreator.DungeonCreatorMode.DEFAULT);
            addButtonToggled = false;
            removeButtonToggled = false;
        }

        GUI.backgroundColor = defaultGUIColor;
        GUILayout.EndHorizontal();

        // Handle input (! input key - SPACE !)
        Event e = Event.current;
        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        switch (e.GetTypeForControl(controlID))
        {
            case EventType.KeyDown:
                if (e.keyCode == KeyCode.Space)
                {
                    // Actions
                    if (creator.Mode == DungeonCreator.DungeonCreatorMode.ADD)
                    {
                        creator.SpawnRoom();
                    }
                    else if (creator.Mode == DungeonCreator.DungeonCreatorMode.REMOVE)
                    {
                        creator.DeleteRoom();
                    }
                }
                break;

        }

        // Add drop menu in Add room mode
        if (creator.Mode == DungeonCreator.DungeonCreatorMode.ADD)
        {
            if (creator.RoomPrefabs == null)
            {
                Debug.Log("error");
                return;
            }

            var stylePrefLabel = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
            stylePrefLabel.fontStyle = FontStyle.Bold;
            EditorGUILayout.LabelField("SELECT ROOM PREFAB", stylePrefLabel);
            // Select Room
            List<string> prefabNames = new List<string>();
            foreach (GameObject roomPrefab in creator.RoomPrefabs)
            {
                if (roomPrefab)
                {
                    prefabNames.Add(roomPrefab.name);
                }
            }
            prefabChoice = EditorGUILayout.Popup(prefabChoice, prefabNames.ToArray());
            // Selected Room prefab
            GUI.enabled = false;
            creator.SelectedRoomPrefab = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Selected Prefab", "Add object to fracture"), creator.SelectedRoomPrefab, typeof(GameObject), false);
            GUI.enabled = true;

            if (creator.SelectedRoomPrefab)
            {
                // Select Door
                List<string> doorNames = new List<string>();
                foreach (Door door in creator.SelectedRoomPrefab.GetComponent<Room>().Doors)
                {
                    if (door)
                    {
                        doorNames.Add(door.gameObject.name);
                    }
                }
                doorChoice = EditorGUILayout.Popup(doorChoice, doorNames.ToArray());
            }

            // Add new Room prefabs
            SerializedObject so = new SerializedObject(target);
            SerializedProperty stringsProperty = so.FindProperty("RoomPrefabs");

            EditorGUILayout.PropertyField(stringsProperty, true); // True means show children
            so.ApplyModifiedProperties(); // Remember to apply modified properties

            // Switch Room prefab
            if (creator.SelectedRoomPrefab == null && creator.RoomPrefabs.Length != 0)
            {
                creator.SelectedRoomPrefab = creator.RoomPrefabs[prefabChoice];
            }

            else if (creator.SelectedRoomPrefab != creator.RoomPrefabs[prefabChoice])
            {
                creator.SelectedRoomPrefab = creator.RoomPrefabs[prefabChoice];
            }

            // Switch Door
            try
            {
                if (creator.SelectedRoomDoor == null && creator.SelectedRoomPrefab.GetComponent<Room>().Doors.Length != 0)
                {
                    creator.SelectedRoomDoor = creator.SelectedRoomPrefab.GetComponent<Room>().Doors[doorChoice].gameObject;
                }

                else if (creator.SelectedRoomDoor != creator.SelectedRoomPrefab.GetComponent<Room>().Doors[doorChoice])
                {
                    creator.SelectedRoomDoor = creator.SelectedRoomPrefab.GetComponent<Room>().Doors[doorChoice].gameObject;
                }
            }

            catch (System.IndexOutOfRangeException ex) { }

            // ReinitRoom if selected door is changed, change door in inspector -> affects on room ghost
            if (creator.GhostRoom && creator.LastMouseHoverDoor && creator.OldSelectedRoomDoorName != creator.SelectedRoomDoor.name)
            {
                creator.OldSelectedRoomDoorName = creator.SelectedRoomDoor.name;
                creator.RemoveGhostRoom();
                creator.InitGhostRoom(creator.LastMouseHoverDoor);
            }
        }
    }
}
