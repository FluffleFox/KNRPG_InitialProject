using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphGrid : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField] private GameObject mapObj;

    [Header("Debug")]
    [SerializeField] private bool debug;
    [SerializeField] private bool generateGraphMap; // Just click on the checkbox
    [SerializeField] private bool drawGraphEdges;
    [SerializeField] private float sphereRadius;
    public float SphereRadius { get { return sphereRadius; } }

    private List<List<bool>> adjacencyMatrix;
    public List<List<bool>> AdjacencyMatrix 
    { 
        get 
        {
            return adjacencyMatrix;
        } 
    }

    private void Start()
    {
        InitGrid();
    }
    public void InitGrid()
    {
        adjacencyMatrix = new List<List<bool>>();
        // Iterate all nodes and make Adjacency matrix
        foreach (Transform origin in mapObj.transform)
        {
            List<bool> row = new List<bool>();
            Vector2 origin2DPosition = new Vector2(origin.position.x, origin.position.z);
            foreach (Transform target in mapObj.transform)
            {
                Vector2 target2DPosition = new Vector2(target.position.x, target.position.z);
                float reachDistance = origin.GetComponent<MeshRenderer>().bounds.size.x/2 + target.GetComponent<MeshRenderer>().bounds.size.x / 2;
                
                if (Vector2.Distance(origin2DPosition, target2DPosition) <= reachDistance && !target.GetComponent<Node>().isOccupied)
                {
                    row.Add(true);
                }
                else
                {
                    row.Add(false);
                }
            }
            // Add adjacency list to matrix
            adjacencyMatrix.Add(row);
        }
    }
    public Node FindNode(Vector3 position)
    {
        foreach (Transform nodeTransform in mapObj.transform)
        {
            Node node = nodeTransform.GetComponent<Node>();
            float reachDistance = node.GetComponent<MeshRenderer>().bounds.size.x / 2;
            // is point inside of hex
            if (Mathf.Pow((position.x - node.transform.position.x),2) + Mathf.Pow((position.z - node.transform.position.z),2) <= Mathf.Pow(reachDistance,2))
            {
                return node;
            }
        }
        Debug.Log("No node exist");
        return null;
    }
    public void UpdateGrid(Node start, Node target)
    {
        start.isOccupied = false;
        target.isOccupied = true;
    }

    public List<Node> GetNeighbours(int index)
    {
        List<Node> neighboursNodes = new List<Node>();
        for(int j = 0; j < adjacencyMatrix[index].Count; j++)
        {
            bool isPossible = adjacencyMatrix[index][j];
            if (isPossible)
            {
                neighboursNodes.Add(transform.GetChild(j).GetComponent<Node>());
            }
        }
        return neighboursNodes;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (debug)
        {
            if (generateGraphMap)
            {
                InitGrid();
                Debug.Log("DEV INFO: Graph map is generated :)");
                generateGraphMap = false;
            }

            // Draw vertices
            foreach (Transform node in mapObj.transform)
            {
                Vector3 gizmosPosition = node.position;
                gizmosPosition.y += sphereRadius + node.GetComponent<MeshRenderer>().bounds.size.y;

                if (!node.GetComponent<Node>().isOccupied)
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.red;
                }
                Gizmos.DrawSphere(gizmosPosition, sphereRadius);
            }

            // Draw edges
            if (drawGraphEdges)
            {
                // Something like DFS
                List<bool> visitedVertices = new List<bool>();
                try
                {
                    for (int i = 0; i < adjacencyMatrix.Count; i++)
                    {
                        visitedVertices.Add(false);
                    }

                    foreach (Transform node in mapObj.transform)
                    {
                        bool visited = visitedVertices[node.GetSiblingIndex()];
                        if (!visited)
                        {
                            Vector3 originPosition = node.position;
                            originPosition.y += sphereRadius + node.GetComponent<MeshRenderer>().bounds.size.y;

                            var neighbours = GetNeighbours(node.GetSiblingIndex());
                            foreach (var neighbour in neighbours)
                            {
                                Vector3 targetPosition = neighbour.transform.position;
                                targetPosition.y += sphereRadius + neighbour.GetComponent<MeshRenderer>().bounds.size.y;
                                if (!neighbour.isOccupied && !node.GetComponent<Node>().isOccupied)
                                {
                                    Gizmos.color = Color.green;
                                }
                                else
                                {
                                    Gizmos.color = Color.red;
                                }
                                Gizmos.DrawLine(originPosition, targetPosition);
                            }

                            visitedVertices[node.GetSiblingIndex()] = true;
                        }
                    }
                }
                catch (System.NullReferenceException e)
                {
                    Debug.Log("DEV INFO: Graph map IS NOT generated, CLICK ON 'Generate Graph Map' checkbox OR uncheck 'Draw Graph Edges'");
                }
            }
        }
    }
    #endif
}
