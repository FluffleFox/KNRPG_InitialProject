using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public enum RoomState
    {
        DEFAULT,
        FIGHT,
        FREEMOVE
    }
    protected RoomState state;
    public RoomState State { get { return state; } }
    public enum RoomType
    {
        STARTROOM,
        COMMONROOM,
        BOSSROOM
    }
    [SerializeField] protected RoomType type;
    public RoomType Type { get { return type; } }
    [SerializeField] protected GraphGrid grid;
    public GraphGrid Grid { get { return grid; } }
    [SerializeField] protected GameObject doors;
    public GameObject DoorsObj { get { return doors; } }
    public Door[] Doors { get { return doors.GetComponentsInChildren<Door>(); } }
    [SerializeField] protected GameObject enemies;
    public GameObject EnemiesObj { get { return enemies; } }

    [SerializeField] protected GameObject environment;
    public GameObject Environment { get { return environment; } }
    [SerializeField] protected Transform cameraPoint; // tmp solution
    public Transform CameraPoint { get { return cameraPoint; } set { cameraPoint = value; } } // tmp solution
    protected bool isPlayerInside;
    public bool IsPlayerInside { get { return isPlayerInside; } set { isPlayerInside = value; } }


    protected void Start()
    {
        if (type != RoomType.STARTROOM)
        {
            DeInitRoom();
        }
    }

    protected void SetState(RoomState newState)
    {
        switch (newState)
        {
            case RoomState.DEFAULT:
                state = newState;
                break;
            case RoomState.FIGHT:
                state = newState;
                LockDoors();
                break;
            case RoomState.FREEMOVE:
                state = newState;
                UnlockDoors();
                break;
        }
    }

    protected void Update()
    {
        // States
        // If there are enemies in the room
        if (isPlayerInside && state != RoomState.FIGHT && enemies.transform.childCount != 0)
        {
            SetState(RoomState.FIGHT);
        }

        // If there are no enemies
        else if (isPlayerInside && state != RoomState.FREEMOVE && enemies.transform.childCount == 0)
        {
            SetState(RoomState.FREEMOVE);
        }

    }

    public void OnRoomEnter()
    {
        InitRoom();
    }

    public void OnRoomExit()
    {
        DeInitRoom();
    }

    public void InitRoom()
    {
        SetState(RoomState.DEFAULT);
        gameObject.active = true;
        grid.InitGrid();
        AstarPathfinding.Instance.Graph = grid;
        FindObjectOfType<TestMovement>().SetPlayerPosition();

        // Camera

        Camera.main.transform.position = cameraPoint.position;
    }

    public void DeInitRoom()
    {
        SetState(RoomState.DEFAULT);
        gameObject.active = false;
        isPlayerInside = false;
    }

    public void UnlockDoors()
    {
        foreach (Door door in Doors)
        {
            door.IsLocked = false;
            door.OnNode.isOccupied = false;
        }
    }

    public void LockDoors()
    {
        foreach (Door door in Doors)
        {
            door.IsLocked = true;
            door.OnNode.isOccupied = true;
        }
    }

    public void SetReferencesForDoors()
    {
        grid.InitGrid();
        foreach (Door door in Doors)
        {
            door.SetReferences();
        }
    }
}
