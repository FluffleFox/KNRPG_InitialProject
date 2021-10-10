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
    private List<List<int>> adjacencyMatrix;


    public List<List<int>> AdjacencyMatrix { get { return adjacencyMatrix; } }

    public float SphereRadius { get { return sphereRadius; } }

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
                float reachDistance = origin.GetComponent<Node>().NodeConnectRadius / 2 + target.GetComponent<Node>().NodeConnectRadius / 2;

                if (Vector2.Distance(origin2DPosition, target2DPosition) <= reachDistance) // && !target.GetComponent<Node>().isOccupied
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
            float reachDistance = node.NodeConnectRadius / 2;
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

    // Return all neighbours
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbourNodes = new List<Node>();
        int index = node.IndexInGrid;
		if (adjacencyMatrix == null) return neighbourNodes;
		for (int j = 0; j < adjacencyMatrix[index].Count; j++)
        {
            neighbourNodes.Add(transform.GetChild(adjacencyMatrix[index][j]).GetComponent<Node>());
        }
        return neighbourNodes;
    }

    // Return only not occupied nodes
    public List<Node> GetAvailableNeighbours(Node node)
    {
        List<Node> neighbourNodes = new List<Node>();
        int index = node.IndexInGrid;
        for (int j = 0; j < adjacencyMatrix[index].Count; j++)
        {
            Node neighbourNode = transform.GetChild(adjacencyMatrix[index][j]).GetComponent<Node>();
            if (!neighbourNode.isOccupied)
            {
                neighbourNodes.Add(transform.GetChild(adjacencyMatrix[index][j]).GetComponent<Node>());
            }
        }
        return neighbourNodes;
    }
	struct NodeAndDistance
	{
		public NodeAndDistance(Node n, int dist)
		{
			node = n;
			distance = dist;
		}
		public Node node;
		public int distance;
	}
	public List<Node> GetAllNodesInRange(Node sourceNode, int range)
	{
		List<Node> nodesInRange = new List<Node>();
		nodesInRange.Add(sourceNode);
		Queue<NodeAndDistance> bfsQueue = new Queue<NodeAndDistance>();
		bfsQueue.Enqueue(new NodeAndDistance(sourceNode,0));
		while(bfsQueue.Count > 0)
		{
			NodeAndDistance actualNode = bfsQueue.Dequeue();
			if(actualNode.distance >= range)
			{
				continue;
			}
			
			foreach (Node neighbour in GetNeighbours(actualNode.node))
			{
				if(!nodesInRange.Contains(neighbour))
				{
					nodesInRange.Add(neighbour);
					bfsQueue.Enqueue(new NodeAndDistance(neighbour, actualNode.distance + 1));
				}
			}
		}
		return nodesInRange;
	}

    // for debug graph matrix
    private void PrintMatrix(List<List<int>> matrix)
    {
        int rowIndex = 0;
        foreach (List<int> row in matrix)
        {
            string nextNodes = "";
            foreach (int nodeIndex in row)
            {
                nextNodes += transform.GetChild(nodeIndex).name + " ";
            }
            Debug.Log(string.Format("{0}: {1}", transform.GetChild(rowIndex).name, nextNodes));
            rowIndex++;
        }
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
                //PrintMatrix(adjacencyMatrix);
                Debug.Log(transform.GetChild(1).GetComponent<Node>().NodeConnectRadius/2);
                //Debug.Log(transform.GetChild(1).eulerAngles);
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
