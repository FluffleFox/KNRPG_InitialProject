using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] protected bool isLocked;
    [SerializeField] protected GameObject socketPoint;
    protected Room room1;
    protected Room room2;
    protected Node onNode;
    protected bool transitionMade; // tmp flag


    public bool IsLocked { get { return isLocked; } set { isLocked = value; } }

    public GameObject SocketPoint { get { return socketPoint; } }

    public Room Room1 { get { return room1; } set { room1 = value; } }

    public Room Room2 { get { return room2; } set { room2 = value; } }

    public Node OnNode { get { return onNode; } set { onNode = value; } }

    public bool TransitionMade { get { return transitionMade; } set { transitionMade = value; } }

    public bool IsDoorConnected 
    {
        get 
        {
            if (room2)
            {
                return true;
            }
            else
            {
                onNode.isOccupied = true;
                return false;
            }
        } 
    }

    private void Start()
    {
        isLocked = true;
        //transitionMade = false;
    }

    public void SetReferences()
    {
        room1 = GetComponentInParent<Room>();
        Node room2TransitionNode = GetOnNode(socketPoint.transform.position);
        if (room2TransitionNode)
        {
            room2 = room2TransitionNode.GetComponentInParent<Room>();
        }
        onNode = room1.Grid.FindNode(socketPoint.transform.position); // it's tmp measure, need to add new door point

        if (!room2)
        {
            isLocked = true;
            onNode.isOccupied = true;
        }
    }

    private void Update()
    {
        // Check is player on door node
        if (onNode == FindObjectOfType<TestMovement>().CurrentNode && !transitionMade) // tmp measure need make player singletone
        {
            GoToNextRoom(); // better place GotToNextRoom in Player.cs (tmp for tests now)
        }
        else if (FindObjectOfType<TestMovement>().CurrentNode != null && onNode != FindObjectOfType<TestMovement>().CurrentNode)
        {
            transitionMade = false;
        }
    }

    private Node GetOnNode(Vector3 position)
    {
        RaycastHit[] hits = Physics.RaycastAll(position, -Vector3.up, 100.0f);
        foreach (RaycastHit hit in hits)
        {
            Debug.DrawRay(position, -Vector3.up, Color.black, 5f);
            //Debug.Log(string.Format("{0} {1} : {2}", GetComponentInParent<Room>().name, transform.name, hit.transform.name));
            if (hit.transform.GetComponent<Node>() && hit.transform.GetComponent<Node>().GetComponentInParent<Room>() != room1)
            {
                return hit.transform.GetComponent<Node>();
            }
        }
        return null;
    }

    private void GoToNextRoom()
    {
        Room2.PlayerEnterInRoom();
        Room1.DeInitRoom();
        FindObjectOfType<TestMovement>().OnRoomEnter(); // tmp need player singletone

        foreach (Door door in Room2.Doors)
        {
            if (door.OnNode == FindObjectOfType<TestMovement>().CurrentNode)
            {
                door.TransitionMade = true;
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(socketPoint.transform.position, new Vector3(0.3f, 0.3f, 0.3f));
    }
    #endif
}
