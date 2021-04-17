using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AstarPathfinding : MonoBehaviour
{
    // Only for tests //
    private bool originClick;
    private bool targetClick;
    [SerializeField] private bool reset;

    private Node st;
    private Node tg;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
        }
    }
    // //
    

    // https://github.com/davecusatis/A-Star-Sharp/blob/master/Astar.cs - FindPath method
    public Stack<Node> FindPath(Node start, Node target)
    {
        GraphGrid graph = GetComponent<GraphGrid>();

        Stack<Node> path = new Stack<Node>();
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();
        List<Node> adjacencies;
        Node current = start;

        // add start node to Open List
        openList.Add(start);

        while (openList.Count != 0 && !closedList.Exists(x => x.transform == target.transform))
        {
            current = openList[0];
            openList.Remove(current);
            closedList.Add(current);
            adjacencies = graph.GetNeighbours(transform.GetSiblingIndex());


            foreach (Node n in adjacencies)
            {
                if (!closedList.Contains(n) && !n.isOccupied)
                {
                    if (!openList.Contains(n))
                    {
                        float distanceToTarget = Mathf.Abs(n.transform.position.x - target.transform.position.x) + 
                                                 Mathf.Abs(n.transform.position.y - target.transform.position.y);
                        openList.Add(n);
                        openList = openList.OrderBy(node => distanceToTarget).ToList<Node>();
                    }
                }
            }
        }

        // construct path, if end was not closed return null
        if (!closedList.Exists(x => x.transform == target.transform))
        {
            return null;
        }

        // if all good, return path
        Node temp = closedList[closedList.IndexOf(current)];
        if (temp == null) return null;
        do
        {
            path.Push(temp);
        } while (temp != start && temp != null);
        return path;
    }
}
