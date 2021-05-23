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
        COMMONROOM,
        BOSSROOM
    }
    [SerializeField] protected RoomType type;
    public RoomType Type { get { return type; } }
    [SerializeField] protected GraphGrid grid;
    public GraphGrid Grid { get { return grid; } }
    [SerializeField] protected GameObject doors;
    public GameObject Doors { get { return doors; } }
    [SerializeField] protected GameObject enemies;
    public GameObject Enemies { get { return enemies; } }

    [SerializeField] protected GameObject environment;
    public GameObject Environment { get { return environment; } }
    protected bool isPlayerInside;
    public bool IsPlayerInside { get { return isPlayerInside; } set { isPlayerInside = value; } }


    protected void Start()
    {
        DeInitRoom();
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
        // If there are enemies in the room
        if (isPlayerInside && state != RoomState.FIGHT && enemies.transform.childCount != 0)
        {
            SetState(RoomState.FIGHT);
        }

        // If there are no enemies
        else if (isPlayerInside && state == RoomState.FIGHT && enemies.transform.childCount == 0)
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

    protected void InitRoom()
    {
        SetState(RoomState.DEFAULT);
        grid.gameObject.active = true;
        grid.InitGrid();
        AstarPathfinding.Instance.Graph = grid;
    }

    protected void DeInitRoom()
    {
        SetState(RoomState.DEFAULT);
        grid.gameObject.active = false;
        isPlayerInside = false;
    }

    public void UnlockDoors()
    {

    }

    public void LockDoors()
    {

    }
}
