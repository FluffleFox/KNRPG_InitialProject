using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] protected bool isLocked;
    public bool IsLocked { get { return isLocked; } set { isLocked = value; } }
    [SerializeField] protected GameObject socketPoint;
    public GameObject SocketPoint { get { return socketPoint; } }
    protected Room room1;
    public Room Room1 { get { return room1; } set { room1 = value; } }
    protected Room room2;
    public Room Room2 { get { return room2; } set { room2 = value; } }
    private Node onNode;
    public Node OnNode { get { return onNode; } set { onNode = value; } }


    private void Start()
    {
        isLocked = true;
        SetReferences();
    }

    public void SetReferences()
    {
        room1 = GetComponentInParent<Room>();
        onNode = room1.Grid.FindNode(socketPoint.transform.position); // it's tmp measure, need to add new door point
    }

    private void Update()
    {
        // Check is player on door node
        if (onNode == FindObjectOfType<TestMovement>().CurrentNode) // tmp measure need make player singletone
        {
            GoToNextRoom(); // better place GotToNextRoom in Player.cs (tmp for tests now)
        }
    }

    private void GoToNextRoom()
    {
        Room2.InitRoom();
        Room1.DeInitRoom();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(socketPoint.transform.position, new Vector3(0.3f, 0.3f, 0.3f));
    }
    #endif
}
