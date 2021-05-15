using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GridCreator))]
public class GridCreatorEditor : Editor
{
    private bool addButtonToggled = false;
    private bool removeButtonToggled = false;
    private bool addChecked = false;
    private bool removeChecked = false;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GridCreator creator = (GridCreator)target;

        GUILayout.BeginHorizontal();

        var defaultGUIColor = GUI.backgroundColor;
        GUI.backgroundColor = Color.green;

        if (!addButtonToggled && !removeButtonToggled)
        {
            addChecked = false;
            removeChecked = false;
            creator.SetMode(GridCreator.CreatorMode.DEFAULT);
        }

        addButtonToggled = GUILayout.Toggle(addButtonToggled, "ADD hex mode", "Button");
        if (addButtonToggled && addButtonToggled != addChecked)
        {
            addChecked = true;
            removeButtonToggled = false;
            removeChecked = false;
            Debug.Log("ADD mode ON !");

            creator.SetMode(GridCreator.CreatorMode.ADD);
        }
        else if (!addButtonToggled && addChecked)
        {
            addChecked = false;
            Debug.Log("Default mode !");

            creator.SetMode(GridCreator.CreatorMode.DEFAULT);
        }

        GUI.backgroundColor = Color.red;

        removeButtonToggled = GUILayout.Toggle(removeButtonToggled, "REMOVE hex mode", "Button");
        if (removeButtonToggled && removeButtonToggled != removeChecked)
        {
            removeChecked = true;
            addButtonToggled = false;
            addChecked = false;
            Debug.Log("REMOVE mode ON !");

            creator.SetMode(GridCreator.CreatorMode.REMOVE);
        }
        else if (!removeButtonToggled && removeChecked)
        {
            removeChecked = false;
            Debug.Log("Default mode !");

            creator.SetMode(GridCreator.CreatorMode.DEFAULT);
        }

        GUI.backgroundColor = defaultGUIColor;

        GUILayout.EndHorizontal();

        // Get mouse click 
        Event e = Event.current;
        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        switch (e.GetTypeForControl(controlID))
        {
            case EventType.KeyDown:
                if (e.keyCode == KeyCode.Space)
                {
                    if (creator.Mode == GridCreator.CreatorMode.ADD)
                    {
                        creator.InteractWithGhosts();
                    }
                    else if (creator.Mode == GridCreator.CreatorMode.REMOVE)
                    {
                        creator.DeleteNode();
                    }
                }
                break;

        }

    }
}
