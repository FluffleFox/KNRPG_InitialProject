using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomCreator))]
public class RoomCreatorEditor : Editor
{
    private RoomCreator creator;
    private bool addButtonToggled = false;
    private bool removeButtonToggled = false;
    private bool addChecked = false;
    private bool removeChecked = false;
    private bool addObjFromPrefabToggled = false;
    private bool addObjFromPrefabChecked = false;

    private int oldPrefabChoice;
    private int prefabChoice;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        creator = (RoomCreator)target;

        var styleIntrolabel = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
        EditorGUILayout.LabelField("To interact with grid press SPACE, in case of ADD nodes use DOUBLE SPACE ", styleIntrolabel);

        // Mode buttons
        GUILayout.BeginHorizontal();

        var defaultGUIColor = GUI.backgroundColor;
        GUI.backgroundColor = Color.green;

        if (!addButtonToggled && !removeButtonToggled && !addObjFromPrefabToggled)
        {
            addChecked = false;
            removeChecked = false;
            addObjFromPrefabChecked = false;
            creator.SetMode(RoomCreator.RoomCreatorMode.DEFAULT);
        }

        addButtonToggled = GUILayout.Toggle(addButtonToggled, "ADD hex mode", "Button");
        if (addButtonToggled && addButtonToggled != addChecked)
        {
            addChecked = true;

            removeButtonToggled = false;
            removeChecked = false;
            addObjFromPrefabToggled = false;
            addObjFromPrefabChecked = false;
            Debug.Log("ADD mode ON !");

            creator.SetMode(RoomCreator.RoomCreatorMode.ADD);
        }
        else if (!addButtonToggled && addChecked)
        {
            addChecked = false;
            Debug.Log("Default mode !");

            creator.SetMode(RoomCreator.RoomCreatorMode.DEFAULT);
        }

        GUI.backgroundColor = Color.red;

        removeButtonToggled = GUILayout.Toggle(removeButtonToggled, "REMOVE hex mode", "Button");
        if (removeButtonToggled && removeButtonToggled != removeChecked)
        {
            removeChecked = true;

            addButtonToggled = false;
            addChecked = false;
            addObjFromPrefabToggled = false;
            addObjFromPrefabChecked = false;
            Debug.Log("REMOVE mode ON !");

            creator.SetMode(RoomCreator.RoomCreatorMode.REMOVE);
        }
        else if (!removeButtonToggled && removeChecked)
        {
            removeChecked = false;
            Debug.Log("Default mode !");

            creator.SetMode(RoomCreator.RoomCreatorMode.DEFAULT);
        }
        GUILayout.EndHorizontal();


        GUI.backgroundColor = Color.cyan;
        addObjFromPrefabToggled = GUILayout.Toggle(addObjFromPrefabToggled, "ADD OBJECT from prefab mode", "Button");
        if (addObjFromPrefabToggled && addObjFromPrefabToggled != addObjFromPrefabChecked)
        {
            addObjFromPrefabChecked = true;

            removeButtonToggled = false;
            removeChecked = false;
            addButtonToggled = false;
            addChecked = false;
            Debug.Log("ADD OBJ from prefab mode ON !");

            creator.SetMode(RoomCreator.RoomCreatorMode.ADDOBJ);
        }
        else if (!addObjFromPrefabToggled && addObjFromPrefabChecked)
        {
            addObjFromPrefabChecked = false;
            Debug.Log("Default mode !");

            creator.SetMode(RoomCreator.RoomCreatorMode.DEFAULT);
        }        

        GUI.backgroundColor = defaultGUIColor;

        // Get is SPACE down
        Event e = Event.current;
        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        switch (e.GetTypeForControl(controlID))
        {
            case EventType.KeyDown:
                if (e.keyCode == KeyCode.Space)
                {
                    // Actions
                    if (creator.Mode == RoomCreator.RoomCreatorMode.ADD)
                    {
                        creator.InteractWithGhosts();
                    }
                    else if (creator.Mode == RoomCreator.RoomCreatorMode.REMOVE)
                    {
                        creator.DeleteNode();
                    }
                    else if (creator.Mode == RoomCreator.RoomCreatorMode.ADDOBJ)
                    {
                        creator.SpawnObject();
                    }
                }
                break;

        }

        // Add drop menu in Add prefab mode
        if (creator.Mode == RoomCreator.RoomCreatorMode.ADDOBJ)
        {
            var stylePrefLabel = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
            stylePrefLabel.fontStyle = FontStyle.Bold;
            EditorGUILayout.LabelField("SELECT ROOM PREFAB", stylePrefLabel);
            creator.RoomEditorType = (EditorPrefabsScriptable.PrefabRoomEditorType)EditorGUILayout.EnumPopup("Room prefab type", creator.RoomEditorType);
            // select room prefab
            List<string> prefabNames = new List<string>();
            foreach (EditorPrefabsScriptable scriptable in EditorPrefabsScriptable.FindPrefabsByType(creator.RoomEditorType, creator.editorScriptables))
            {
                prefabNames.Add(scriptable.name);
            }
            prefabChoice = EditorGUILayout.Popup(prefabChoice, prefabNames.ToArray());

            creator.Rotation = (RoomCreator.RotationType)EditorGUILayout.EnumPopup("Rotation", creator.Rotation);
            GUI.enabled = false;
            creator.SelectedPrefab = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Selected Prefab", "Add object to fracture"), creator.SelectedPrefab, typeof(GameObject), false);
            GUI.enabled = true;

            SerializedObject so = new SerializedObject(target);
            SerializedProperty stringsProperty = so.FindProperty("editorScriptables");

            EditorGUILayout.PropertyField(stringsProperty, true); // True means show children
            so.ApplyModifiedProperties(); // Remember to apply modified properties
        }

        if (creator.SelectedScriptable == null && creator.editorScriptables.Length != 0)
        {
            creator.SelectedScriptable = creator.editorScriptables[0];
            if (creator.SelectedScriptable)
            {
                creator.SelectedPrefab = creator.SelectedScriptable.Prefab;
            }
        }
        // Change room prefab type
        else if (creator.editorScriptables.Length != 0 && (creator.SelectedScriptable.Type != creator.RoomEditorType))
        {
            prefabChoice = 0;
            creator.SelectedScriptable = EditorPrefabsScriptable.FindPrefabsByType(creator.RoomEditorType, creator.editorScriptables)[prefabChoice];
            if (creator.SelectedScriptable)
            {
                creator.SelectedPrefab = creator.SelectedScriptable.Prefab;
            }
        }
        // Change room prefab
        else if (creator.editorScriptables.Length != 0 && oldPrefabChoice != prefabChoice)
        {
            oldPrefabChoice = prefabChoice;
            creator.SelectedScriptable = EditorPrefabsScriptable.FindPrefabsByType(creator.RoomEditorType, creator.editorScriptables)[prefabChoice];
            if (creator.SelectedScriptable)
            {
                creator.SelectedPrefab = creator.SelectedScriptable.Prefab;
            }
        }
    }
}
