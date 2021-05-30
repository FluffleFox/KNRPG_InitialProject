using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class RoomCreator : MonoBehaviour
{
    public enum RoomCreatorMode
    {
        DEFAULT,
        ADD,
        REMOVE,
        ADDOBJ
    }
    [SerializeField] private Room roomToEdit;
    public Room RoomToEdit { get { return roomToEdit; } }
    [SerializeField] private GraphGrid grid;
    public GraphGrid Grid { get { return grid; } }
    [SerializeField] private GameObject hexGhostPrefab;
    private RoomCreatorMode mode = RoomCreatorMode.DEFAULT;
    public RoomCreatorMode Mode { get { return mode; } }
    private List<GameObject> hexGhosts;
    private GameObject currentNode;   // mouse hover on hex
    private GameObject selectedGhost; // mouse hover on green hex
    private Vector3 mousePos;

    // Objects tab
    private EditorPrefabsScriptable.PrefabRoomEditorType roomEditorPrefab;
    public EditorPrefabsScriptable.PrefabRoomEditorType RoomEditorType { get { return roomEditorPrefab; } set { roomEditorPrefab = value; } }
    private EditorPrefabsScriptable selectedScriptable;
    public EditorPrefabsScriptable SelectedScriptable { get { return selectedScriptable; } set { selectedScriptable = value; } }
    private GameObject selectedPrefab;
    public GameObject SelectedPrefab { get { return selectedPrefab; } set { selectedPrefab = value; } }
    [HideInInspector] public EditorPrefabsScriptable[] editorScriptables;
    public enum RotationType
    {
        ROTATION_0,
        ROTATION_30,
        ROTATION_60,
        ROTATION_90,
        ROTATION_120,
        ROTATION_150,
        ROTATION_180,
        ROTATION_210,
        ROTATION_240,
        ROTATION_270,
        ROTATION_300,
        ROTATION_330,
    }
    private RotationType rotation;
    public RotationType Rotation { get { return rotation; } set { rotation = value; } }
    private GameObject possibleCollision;
    

    public void SetMode(RoomCreatorMode newMode)
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
        if (Selection.activeTransform == transform)
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
                case (RoomCreatorMode.DEFAULT):
                    Gizmos.color = Color.gray;
                    break;
                case (RoomCreatorMode.ADD):
                    Gizmos.color = Color.green;
                    break;
                case (RoomCreatorMode.REMOVE):
                    Gizmos.color = Color.red;
                    break;
                case (RoomCreatorMode.ADDOBJ):
                    Gizmos.color = Color.blue;
                    break;
            }

            // Show selected node
            GameObject nodeObject = GetMouseOverlap(typeof(Node));
            if (nodeObject && currentNode != nodeObject)
            {
                if (mode == RoomCreatorMode.ADD)
                {
                    ClearGhosts();
                }
                currentNode = nodeObject;
                Node node = nodeObject.GetComponent<Node>();
                Gizmos.DrawCube(new Vector3(node.transform.position.x, node.transform.position.y + node.ModelHeight + 0.25f, node.transform.position.z), Vector3.one / 5);
            }

            // Actions
            switch (mode)
            {
                case (RoomCreatorMode.DEFAULT):
                    break;
                case (RoomCreatorMode.ADD):
                    if (hexGhosts != null && hexGhosts.Count == 0 && currentNode) // init ghosts
                    {
                        InitGhosts();
                    }
                    else if (hexGhosts != null && hexGhosts.Count != 0)
                    {
                        selectedGhost = GetMouseOverlap(typeof(NodeGhost));
                    }
                    break;
                case (RoomCreatorMode.REMOVE):
                    break;
                case (RoomCreatorMode.ADDOBJ):
                    // Draw blue line depending on rotation angle
                    float radius = currentNode.GetComponent<Node>().ModelWidth / 2;
                    Vector3 nodePos = currentNode.transform.position;
                    float height = currentNode.GetComponent<Node>().ModelHeight + 0.25f;
                    float angle = (int)rotation * 30 * Mathf.PI / 180;
                    Vector3 startPoint = new Vector3(nodePos.x + Mathf.Cos(angle) * radius, nodePos.y + height, nodePos.z - Mathf.Sin(angle) * radius);
                    Vector3 endPoint = new Vector3(nodePos.x - Mathf.Cos(angle) * radius, nodePos.y + height, nodePos.z + Mathf.Sin(angle) * radius);
                    Gizmos.DrawLine(startPoint, endPoint);
                    //

                    possibleCollision = GetMouseOverlap(typeof(MeshFilter));
                    break;
            }
        }
    }

    private void InitGhosts()
    {
        List<Node> nodeNeighbours = grid.GetAllNeighbours(currentNode.GetComponent<Node>());
        List<bool> isNodeExists = Enumerable.Repeat(false, 6).ToList();

        //  Depending on clockwise direction find sides with hex
        foreach (Node node in nodeNeighbours)
        {
            // TODO rework to model width istead of values
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
        // Highlight (green hex ghost) sides without hex  
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
    public void SpawnObject()
    {
        if (currentNode)
        {
            Vector3 spawnPosition = currentNode.transform.position + selectedScriptable.InstanitiateOffset;
            spawnPosition.y += currentNode.GetComponent<Node>().ModelHeight + 0.15f;

            if (possibleCollision)
            {
                if (possibleCollision.transform.position == spawnPosition)
                {
                    Debug.Log("Can't Add, the same object is already placed");
                    return;
                }
            }
            if (selectedScriptable.IsNodeOccupied)
            {
                currentNode.GetComponent<Node>().isOccupied = true;
            }

            Quaternion objectRotation = Quaternion.identity;
            objectRotation.eulerAngles = new Vector3(objectRotation.eulerAngles.x, (int)rotation * 30, objectRotation.eulerAngles.z);
            GameObject newObject = PrefabUtility.InstantiatePrefab(selectedPrefab) as GameObject; // spawn prefab not clone
            newObject.transform.position = spawnPosition;
            newObject.transform.rotation = objectRotation;

            if (selectedScriptable.Type == EditorPrefabsScriptable.PrefabRoomEditorType.DOOR)
            {
                newObject.name = System.String.Format("{0} ({1})", newObject.name, roomToEdit.GetComponent<Room>().DoorsObj.transform.childCount+1);
                newObject.transform.parent = roomToEdit.GetComponent<Room>().DoorsObj.gameObject.transform;
                newObject.GetComponent<Door>().OnNode = currentNode.GetComponent<Node>();
                newObject.GetComponent<Door>().Room1 = transform.parent.GetComponent<Room>();
            }
            else if (selectedScriptable.Type == EditorPrefabsScriptable.PrefabRoomEditorType.ENEMY)
            {
                newObject.transform.parent = roomToEdit.GetComponent<Room>().EnemiesObj.gameObject.transform;
            }
            else
            {
                newObject.transform.parent = roomToEdit.GetComponent<Room>().Environment.gameObject.transform;
            }
        }
    }
	// Detect cursor overlap GameObject
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
