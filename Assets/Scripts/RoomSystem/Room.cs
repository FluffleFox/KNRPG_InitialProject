using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public enum RoomType
    {
        COMMONROOM,
        BOSSROOM
    }
    [SerializeField] protected RoomType type;
    public RoomType Type { get { return type; } }
    [SerializeField] protected GraphGrid grid;
    public GraphGrid Grid { get { return grid; } }
    [SerializeField] protected GameObject objects;
    public GameObject Objects { get { return objects; } }


    protected void Start()
    {
        DeInitRoom();
    }

    protected void InitRoom()
    {
        grid.gameObject.active = true;
        grid.InitGrid();
        AstarPathfinding.Instance.Graph = grid;
    }

    protected void DeInitRoom()
    {
        grid.gameObject.active = false;
    }
}
