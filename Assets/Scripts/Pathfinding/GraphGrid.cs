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

    private List<List<int>> adjacencyMatrix;
    public List<List<int>> AdjacencyMatrix 
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
        adjacencyMatrix = new List<List<int>>();
        // Iterate all nodes and make Adjacency Matrix (int - index of neighbouring vertex)
        foreach (Transform origin in mapObj.transform)
        {
            List<int> adjacencyList = new List<int>();
            Vector2 origin2DPosition = new Vector2(origin.position.x, origin.position.z);
            foreach (Transform target in mapObj.transform)
            {
                Vector2 target2DPosition = new Vector2(target.position.x, target.position.z);
                float reachDistance = origin.GetComponent<Node>().ModelWidth / 2 + target.GetComponent<Node>().ModelWidth / 2;
                
                if (Vector2.Distance(origin2DPosition, target2DPosition) <= reachDistance && !target.GetComponent<Node>().isOccupied)
                {
                    adjacencyList.Add(target.GetComponent<Node>().IndexInGrid);
                }
            }
            // Add adjacency list to matrix
            adjacencyMatrix.Add(adjacencyList);
        }

      
    }
    public Node FindNode(Vector3 position)
    {
        foreach (Transform nodeTransform in mapObj.transform)
        {
            Node node = nodeTransform.GetComponent<Node>();
            float reachDistance = node.ModelWidth / 2;
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

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighboursNodes = new List<Node>();
        int index = node.IndexInGrid;
        for (int j = 0; j < adjacencyMatrix[index].Count; j++)
        {
            neighboursNodes.Add(transform.GetChild(adjacencyMatrix[index][j]).GetComponent<Node>());
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
            foreach (Transform nodeTransform in mapObj.transform)
            {
                Vector3 gizmosPosition = nodeTransform.position;
                gizmosPosition.y += sphereRadius + nodeTransform.GetComponent<Node>().ModelHeight;

                if (!nodeTransform.GetComponent<Node>().isOccupied)
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

                    foreach (Transform nodeTransform in mapObj.transform)
                    {
                        bool visited = visitedVertices[nodeTransform.GetComponent<Node>().IndexInGrid];
                        if (!visited)
                        {
                            Vector3 originPosition = nodeTransform.position;
                            originPosition.y += sphereRadius + nodeTransform.GetComponent<Node>().ModelHeight;

                            var neighbours = GetNeighbours(nodeTransform.GetComponent<Node>());
                            foreach (var neighbour in neighbours)
                            {
                                Vector3 targetPosition = neighbour.transform.position;
                                targetPosition.y += sphereRadius + neighbour.GetComponent<Node>().ModelHeight;
                                if (!neighbour.isOccupied && !nodeTransform.GetComponent<Node>().isOccupied)
                                {
                                    Gizmos.color = Color.green;
                                }
                                else
                                {
                                    Gizmos.color = Color.red;
                                }
                                Gizmos.DrawLine(originPosition, targetPosition);
                            }
                            visitedVertices[nodeTransform.GetComponent<Node>().IndexInGrid] = true;
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
