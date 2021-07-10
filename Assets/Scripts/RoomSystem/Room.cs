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
    public enum RoomType
    {
        STARTROOM,
        COMMONROOM,
        BOSSROOM
    }
    [SerializeField] protected RoomType type;
    [SerializeField] protected GraphGrid grid;
    [SerializeField] protected GameObject doors;
    [SerializeField] protected GameObject enemies;
    [SerializeField] protected GameObject environment;
    [SerializeField] protected Transform cameraPoint; // tmp solution
    protected RoomState state;
    protected bool isPlayerInside;


    public RoomType Type { get { return type; } }

    public GraphGrid Grid { get { return grid; } }

    public GameObject DoorsObj { get { return doors; } }

    public Door[] Doors { get { return doors.GetComponentsInChildren<Door>(); } }

    public GameObject EnemiesObj { get { return enemies; } }

    public GameObject Environment { get { return environment; } }

    public Transform CameraPoint { get { return cameraPoint; } set { cameraPoint = value; } } // tmp solution

    public bool IsPlayerInside { get { return isPlayerInside; } set { isPlayerInside = value; } }

    public RoomState State { get { return state; } }

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


    public void PlayerEnterInRoom()
    {
        InitRoom();
        AstarPathfinding.Instance.Graph = grid;

        // Camera
        Camera.main.transform.position = cameraPoint.position;
    }

    public void InitRoom()
    {
        SetState(RoomState.DEFAULT);
        gameObject.active = true;
        grid.InitGrid();
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
            if (door.IsDoorConnected)
            {
                door.IsLocked = false;
                door.OnNode.isOccupied = false;
            }
        }
        grid.InitGrid();
    }

    public void LockDoors()
    {
        foreach (Door door in Doors)
        {
            door.IsLocked = true;
            //door.OnNode.isOccupied = true;
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
