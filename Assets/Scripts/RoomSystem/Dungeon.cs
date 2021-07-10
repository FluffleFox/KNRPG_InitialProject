using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    Room[] rooms;


    void Start()
    {
        rooms = GetComponentsInChildren<Room>();
        InitDungeon();
    }

    public void InitDungeon()
    {
        foreach (Room room in rooms)
        {
            room.InitRoom();
            foreach (Door door in room.Doors)
            {
                door.SetReferences();
            }
        }

        foreach (Room room in rooms)
        {
            if (room.Type != Room.RoomType.STARTROOM)
            {
                room.DeInitRoom();
            }
        }
    }
}
