using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AstarPathfinding : MonoBehaviour
{
    // Only for tests in editor //
    [SerializeField] private bool reset;
    [SerializeField] private Node origin;
    [SerializeField] private Node destination;
    private bool pathFound;
    // //

    private List<Node> path;
    private void Start()
    {
        pathFound = false;
    }
    private void Update()
    {
        if (origin != null && destination != null && !pathFound)
        {
            path = FindPath(origin, destination);
            pathFound = true;

            if (path == null)
            {
                Debug.Log("DEV INFO: There is no possible Astar path");
            }
        }
        if (reset)
        {
            reset = false;
            pathFound = false;
        }
    }

    // https://github.com/davecusatis/A-Star-Sharp/blob/master/Astar.cs - FindPath method
    public List<Node> FindPath(Node start, Node target)
    {
        GraphGrid graph = GetComponent<GraphGrid>();

        Stack<Node> path = new Stack<Node>();
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();
        List<Node> adjacencies;
        Node current = start;

        // add start node to Open List
        openList.Add(start);

        while (openList.Count != 0 && !closedList.Exists(x => x.transform.position == target.transform.position))
        {
            current = openList[0];
            openList.Remove(current);
            closedList.Add(current);
            adjacencies = graph.GetNeighbours(current.transform.GetSiblingIndex());

            foreach (Node n in adjacencies)
            {
                if (!closedList.Contains(n) && !n.isOccupied)
                {
                    if (!openList.Contains(n))
                    {
                        n.Parent = current;
                        float distanceToTarget = Mathf.Abs(n.transform.position.x - target.transform.position.x) + 
                                                 Mathf.Abs(n.transform.position.z - target.transform.position.z);
                        openList.Add(n);
                        openList = openList.OrderBy(node => distanceToTarget).ToList<Node>();
                    }
                }
            }
        }

        // construct path, if end was not closed return null
        if (!closedList.Exists(x => x.transform.position == target.transform.position))
        {
            return null;
        }

        // if all good, return path
        Node temp = closedList[closedList.IndexOf(current)];
        if (temp == null) return null;
        do
        {
            path.Push(temp);
            temp = temp.Parent;
        } while (temp != start && temp != null);
        return path.ToList();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // Only in game mode
        if (Application.isPlaying && path != null)
        {
            if (pathFound && path.Count != 0)
            {
                Gizmos.color = Color.yellow;
                float sphereRadius = GetComponent<GraphGrid>().SphereRadius;

                Node originNode = path[0];
                Vector3 originPosition = originNode.gameObject.transform.position;
                originPosition.y += sphereRadius + originNode.transform.GetComponent<MeshRenderer>().bounds.size.y;
                Gizmos.DrawSphere(originPosition, sphereRadius);

                for (int i = 1; i < path.Count; i++)
                {
                    Node targetNode = path[i];
                    Vector3 targetPosition = targetNode.gameObject.transform.position;
                    targetPosition.y += sphereRadius + targetNode.transform.GetComponent<MeshRenderer>().bounds.size.y;
                    Gizmos.DrawLine(originPosition, targetPosition);

                    originNode = targetNode;
                    originPosition = originNode.gameObject.transform.position;
                    originPosition.y += sphereRadius + originNode.transform.GetComponent<MeshRenderer>().bounds.size.y;
                }
                Gizmos.DrawSphere(originPosition, sphereRadius);
            }
        }
    }
#endif
}
