using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
public class DungeonCreator : MonoBehaviour
{
    public enum DungeonCreatorMode
    {
        DEFAULT,
        ADD,
        REMOVE
    }
    private DungeonCreatorMode mode = DungeonCreatorMode.DEFAULT;
    public DungeonCreatorMode Mode { get { return mode; } }
    private Vector3 mousePos;
    private GameObject mouseHoverDoor;
    [HideInInspector] public GameObject[] RoomPrefabs;
    private GameObject selectedRoomPrefab;
    public GameObject SelectedRoomPrefab { get { return selectedRoomPrefab; } set { selectedRoomPrefab = value; } }
    private GameObject selectedRoomDoor;
    private string oldSelectedRoomDoorName;
    public GameObject SelectedRoomDoor { get { return selectedRoomDoor; } set { selectedRoomDoor = value; } }
    private GameObject ghostRoom;


    public void SetMode(DungeonCreatorMode newMode)
    {
        RemoveGhostRoom();
        switch (newMode)
        {
            case DungeonCreatorMode.DEFAULT:
                mode = DungeonCreatorMode.DEFAULT;
                break;
            case DungeonCreatorMode.ADD:
                mode = DungeonCreatorMode.ADD;
                break;
            case DungeonCreatorMode.REMOVE:
                mode = DungeonCreatorMode.REMOVE;
                break;
        }
    }

    public void RemoveGhostRoom()
    {
        DestroyImmediate(ghostRoom);
    }

    public void InitGhostRoom()
    {
        // Set objects for current room doors
        Door mouseHoverDoorFixed = mouseHoverDoor.GetComponent<Door>();
        Room currentRoom = mouseHoverDoor.GetComponentInParent<Room>();
        currentRoom.Grid.InitGrid();
        foreach (Door door in currentRoom.Doors)
        {
            door.Room1 = currentRoom.GetComponent<Room>();
            door.OnNode = currentRoom.GetComponent<Room>().Grid.FindNode(door.transform.position);
            if (mouseHoverDoorFixed.name == door.name)
            {
                mouseHoverDoorFixed = door;
            }
        }

        // Set proper position
        Vector3 hoverDoorPosition = mouseHoverDoor.transform.position;

        Quaternion rotation = mouseHoverDoor.transform.GetComponentInParent<Room>().transform.rotation;
        ghostRoom = Instantiate(selectedRoomPrefab, mouseHoverDoor.transform.GetComponentInParent<Room>().transform.position, rotation);
        // rotate to door identity
        ghostRoom.transform.Rotate(Vector3.up, mouseHoverDoor.transform.eulerAngles.y - selectedRoomDoor.transform.eulerAngles.y);
        // rotate around door
        ghostRoom.transform.RotateAround(mouseHoverDoor.GetComponent<Door>().OnNode.transform.position, Vector3.up, 180f);

        // Set objects for ghost room doors
        Door ghostRoomPrefabDoor = selectedRoomDoor.GetComponent<Door>();
        ghostRoom.GetComponent<Room>().Grid.InitGrid();
        foreach (Door door in ghostRoom.GetComponent<Room>().Doors)
        {
            door.Room1 = ghostRoom.GetComponent<Room>();
            door.OnNode = ghostRoom.GetComponent<Room>().Grid.FindNode(door.transform.position);
            if (ghostRoomPrefabDoor.name == door.name)
            {
                ghostRoomPrefabDoor = door;
            }
        }

        // Move to identity position
        Vector3 offset = new Vector3(mouseHoverDoorFixed.GetComponent<Door>().OnNode.transform.position.x - ghostRoomPrefabDoor.GetComponent<Door>().OnNode.transform.position.x,
                                     0,
                                     mouseHoverDoorFixed.GetComponent<Door>().OnNode.transform.position.z - ghostRoomPrefabDoor.GetComponent<Door>().OnNode.transform.position.z);
        ghostRoom.transform.position += offset;

        //ghostRoom.transform.parent = mouseHoverDoor.transform.GetComponentInParent<Room>().transform.parent;
        ghostRoom.name = System.String.Format("{0} ({1})", ghostRoom.name, transform.childCount + 1);
        
    }

    // Detect cursor overlap GameObject
    private GameObject GetMouseOverlap(System.Type comp)
    {
        Ray ray = UnityEditor.HandleUtility.GUIPointToWorldRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            if (hit.transform.GetComponent(comp))
            {
                return hit.transform.gameObject;
            }
        }
        return null;
    }

    private void OnDrawGizmosSelected()
    {
        mousePos = Event.current.mousePosition;

        // Set color
        switch (mode)
        {
            case DungeonCreatorMode.DEFAULT:
                Gizmos.color = Color.gray;
                break;
            case DungeonCreatorMode.ADD:
                Gizmos.color = Color.blue;
                break;
            case DungeonCreatorMode.REMOVE:
                Gizmos.color = Color.red;
                break;
        }
        mouseHoverDoor = GetMouseOverlap(typeof(Door));
        if (mouseHoverDoor)
        {
            Gizmos.DrawCube(mouseHoverDoor.GetComponent<Door>().SocketPoint.transform.position, new Vector3(0.5f, 0.5f, 0.5f));
            Debug.Log(mouseHoverDoor);
        }

        // Actions
        switch (mode)
        {
            case DungeonCreatorMode.DEFAULT:
                break;
            case DungeonCreatorMode.ADD:
                if (!ghostRoom && mouseHoverDoor && selectedRoomDoor && selectedRoomPrefab)
                {
                    oldSelectedRoomDoorName = selectedRoomDoor.name;
                    InitGhostRoom();
                }
                break;
            case DungeonCreatorMode.REMOVE:
                break;
        }
    }
}
#endif