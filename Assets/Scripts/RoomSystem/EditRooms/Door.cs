using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] protected bool isLocked;
    public bool IsLocked { get { return isLocked; } set { isLocked = value; } }
    [SerializeField] protected GameObject socketPoint;
    public GameObject SocketPoint { get { return socketPoint; } }
    protected Door doorReference; // need to connect rooms
    public Door DoorReference { set { doorReference = value; } get { return doorReference; } }
    protected Room room1;
    public Room Room1 { set { room1 = value; } }
    protected Room room2;
    public Room Room2 { set { room2 = value; } }
    private Node onNode;
    public Node OnNode { get { return onNode; } set { onNode = value; } }


    private void Start()
    {
        doorReference = gameObject.GetComponent<Door>();
        isLocked = true;
    }

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(socketPoint.transform.position, new Vector3(0.3f, 0.3f, 0.3f));
    }
    #endif
}
