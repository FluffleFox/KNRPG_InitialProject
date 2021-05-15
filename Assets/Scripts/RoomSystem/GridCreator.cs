using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
public class GridCreator : MonoBehaviour
{
    public enum CreatorMode
    {
        DEFAULT,
        ADD,
        REMOVE
    }

    [SerializeField] private GraphGrid grid;
    public GraphGrid Grid { get { return grid; } }
    [SerializeField] private GameObject hexGhostPrefab;
    private CreatorMode mode = CreatorMode.DEFAULT;
    public CreatorMode Mode { get { return mode; } }
    private List<GameObject> hexGhosts;
    private GameObject currentNode;
    private GameObject selectedGhost;
    private Vector3 mousePos;

    public void SetMode(CreatorMode newMode)
    {
        ClearGhosts();
        hexGhosts = new List<GameObject>();
        // update grid
        grid.InitGrid();
		mode = newMode;
    }
    public void ClearGhosts()
    {
        if (hexGhosts != null && hexGhosts.Count != 0)
        {
            foreach (GameObject ghost in hexGhosts)
            {
                if (ghost)
                {
                    ghost.transform.GetComponent<NodeGhost>().Destroy();
                }
            }
            hexGhosts.Clear();
        }
    }

    private void OnDrawGizmosSelected()
    {
        mousePos = Event.current.mousePosition;
        if (grid.AdjacencyMatrix == null)
        {
            Debug.Log("Generowany jest grid");
			grid.InitGrid();
        }

        // Set color
        switch (mode)
        {
            case (CreatorMode.DEFAULT):
                Gizmos.color = Color.gray;
                break;
            case (CreatorMode.ADD):
                Gizmos.color = Color.green;
                break;
            case (CreatorMode.REMOVE):
                Gizmos.color = Color.red;
                break;
        }

        // Show selected node
        GameObject nodeObject = GetMouseOverlap(typeof(Node));
        if (nodeObject && currentNode != nodeObject)
        {
            if (mode == CreatorMode.ADD)
            {
                ClearGhosts();
            }
            currentNode = nodeObject;
            Node node = nodeObject.GetComponent<Node>();
            Gizmos.DrawCube(new Vector3(node.transform.position.x, node.transform.position.y+node.ModelHeight+0.25f, node.transform.position.z),Vector3.one/5);
        }

        // Actions
        switch (mode)
        {
            case (CreatorMode.DEFAULT):
                break;
            case (CreatorMode.ADD):
                if (hexGhosts != null && hexGhosts.Count == 0 && currentNode) // init ghosts
                {
                    InitGhosts();
                }
                else if(hexGhosts != null && hexGhosts.Count != 0)
                {
                    selectedGhost = GetMouseOverlap(typeof(NodeGhost));
                }
                break;
            case (CreatorMode.REMOVE):
                
                break;
        }
    }
    private void InitGhosts()
    {
        List<Node> nodeNeighbours = grid.GetNeighbours(currentNode.GetComponent<Node>());
        List<bool> isNodeExists = Enumerable.Repeat(false, 6).ToList();
        foreach (Node node in nodeNeighbours)
        {
            if (node.transform.position.x - currentNode.transform.position.x == 0.0f
                && node.transform.position.z - currentNode.transform.position.z == -1.0f)
            {
                isNodeExists[0] = true;
            }
            else if (node.transform.position.x - currentNode.transform.position.x <= -0.8f
                && node.transform.position.z - currentNode.transform.position.z == -0.5f)
            {
                isNodeExists[1] = true;
            }
            else if (node.transform.position.x - currentNode.transform.position.x <= -0.8f
                && node.transform.position.z - currentNode.transform.position.z == 0.5f)
            {
                isNodeExists[2] = true;
            }
            else if (node.transform.position.x - currentNode.transform.position.x == 0.0f
                && node.transform.position.z - currentNode.transform.position.z == 1.0f)
            {
                isNodeExists[3] = true;
            }
            else if (node.transform.position.x - currentNode.transform.position.x >= 0.8f
                && node.transform.position.z - currentNode.transform.position.z == 0.5f)
            {
                isNodeExists[4] = true;
            }
            else if (node.transform.position.x - currentNode.transform.position.x >= 0.8f
                && node.transform.position.z - currentNode.transform.position.z == -0.5f)
            {
                isNodeExists[5] = true;
            }
        }
        for (int i = 0; i < isNodeExists.Count; i++)
        {
            if (!isNodeExists[i])
            {
                Transform parent = currentNode.transform.parent;
                Vector3 position = currentNode.transform.position;
                GameObject newNode;
                switch (i)
                {
                    case (0):
                        position.z -= 1.0f;
                        newNode = Instantiate(hexGhostPrefab, position, Quaternion.identity);
                        newNode.GetComponent<NodeGhost>().ParentForNode = parent;
                        hexGhosts.Add(newNode);
                        break;
                    case (1):
                        position.x -= 0.866f;
                        position.z -= 0.5f;
                        newNode = Instantiate(hexGhostPrefab, position, Quaternion.identity);
                        newNode.GetComponent<NodeGhost>().ParentForNode = parent;
                        hexGhosts.Add(newNode);
                        break;
                    case (2):
                        position.x -= 0.866f;
                        position.z += 0.5f;
                        newNode = Instantiate(hexGhostPrefab, position, Quaternion.identity);
                        newNode.GetComponent<NodeGhost>().ParentForNode = parent;
                        hexGhosts.Add(newNode);
                        break;
                    case (3):
                        position.z += 1.0f;
                        newNode = Instantiate(hexGhostPrefab, position, Quaternion.identity);
                        newNode.GetComponent<NodeGhost>().ParentForNode = parent;
                        hexGhosts.Add(newNode);
                        break;
                    case (4):
                        position.x += 0.866f;
                        position.z += 0.5f;
                        newNode = Instantiate(hexGhostPrefab, position, Quaternion.identity);
                        newNode.GetComponent<NodeGhost>().ParentForNode = parent;
                        hexGhosts.Add(newNode);
                        break;
                    case (5):
                        position.x += 0.866f;
                        position.z -= 0.5f;
                        newNode = Instantiate(hexGhostPrefab, position, Quaternion.identity);
                        newNode.GetComponent<NodeGhost>().ParentForNode = parent;
                        hexGhosts.Add(newNode);
                        break;
                }
            }
        }
    }
    public void InteractWithGhosts()
    {
        if (selectedGhost)
        {
            selectedGhost.GetComponent<NodeGhost>().InitNode();
            grid.InitGrid();
        }
    }
    public void DeleteNode()
    {
        if (currentNode)
        {
            DestroyImmediate(currentNode);
            grid.InitGrid();
        }
    }
	// Detect cursor coords
	private GameObject GetMouseOverlap(System.Type comp)
    {
        Ray ray = UnityEditor.HandleUtility.GUIPointToWorldRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            //Debug.DrawRay(ray.origin, hit.transform.position, Color.blue, 5f);
            //Debug.Log(hit.transform.name);
            //Debug.Log(hit.transform.position);
            if (hit.transform.GetComponent(comp))
            {
                return hit.transform.gameObject;
            }
        }
        return null;
    }
}
#endif
