using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AstarPathfinding : MonoBehaviour
{
    private static AstarPathfinding instance;
    public static AstarPathfinding Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AstarPathfinding();
            }
            return instance;
        }
    }

    [SerializeField] private bool debug;
    private List<Node> pathDebug;
    [SerializeField] private GraphGrid graph;
    public GraphGrid Graph { set { graph = value; } }


    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // https://github.com/davecusatis/A-Star-Sharp/blob/master/Astar.cs - FindPath method
    public List<Node> FindPath(Node start, Node target)
    {
        this.pathDebug = null;

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
            adjacencies = graph.GetNeighbours(current);

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
        this.pathDebug = path.ToList();
        return path.ToList();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // Debug only in game mode
        if (debug && Application.isPlaying && pathDebug != null)
        {
            if (pathDebug.Count != 0)
            {
                Gizmos.color = Color.yellow;
                float sphereRadius = GetComponent<GraphGrid>().SphereRadius;

                Node originNode = pathDebug[0];
                Vector3 originPosition = originNode.gameObject.transform.position;
                originPosition.y += sphereRadius + originNode.ModelHeight;
                Gizmos.DrawSphere(originPosition, sphereRadius);

                for (int i = 1; i < pathDebug.Count; i++)
                {
                    Node targetNode = pathDebug[i];
                    Vector3 targetPosition = targetNode.gameObject.transform.position;
                    targetPosition.y += sphereRadius + targetNode.ModelHeight;
                    Gizmos.DrawLine(originPosition, targetPosition);

                    originNode = targetNode;
                    originPosition = originNode.gameObject.transform.position;
                    originPosition.y += sphereRadius + originNode.ModelHeight;
                }
                Gizmos.DrawSphere(originPosition, sphereRadius);
            }
        }
    }
#endif
}
