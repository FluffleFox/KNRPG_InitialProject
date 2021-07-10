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
    [SerializeField] private GraphGrid grid;
    [SerializeField] private GameObject hexGhostPrefab;
    private RoomCreatorMode mode = RoomCreatorMode.DEFAULT;
    private List<GameObject> hexGhosts;
    private GameObject currentNode;   // mouse hover on hex
    private GameObject selectedGhost; // mouse hover on green hex
    private Vector3 mousePos;
    // Objects tab
    private EditorPrefabsScriptable.PrefabRoomEditorType roomEditorPrefab;
    private EditorPrefabsScriptable selectedScriptable;
    private GameObject selectedPrefab;
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
    private GameObject possibleCollision;


    public Room RoomToEdit { get { return roomToEdit; } }

    public GraphGrid Grid { get { return grid; } }

    public RoomCreatorMode Mode { get { return mode; } }

    public EditorPrefabsScriptable.PrefabRoomEditorType RoomEditorType { get { return roomEditorPrefab; } set { roomEditorPrefab = value; } }

    public EditorPrefabsScriptable SelectedScriptable { get { return selectedScriptable; } set { selectedScriptable = value; } }

    public GameObject SelectedPrefab { get { return selectedPrefab; } set { selectedPrefab = value; } }

    public RotationType Rotation { get { return rotation; } set { rotation = value; } }

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

            // Show selected node if this node from this room
            GameObject nodeObject = GetMouseOverlap(typeof(Node));
            if (nodeObject)
            {
                if (nodeObject.GetComponentInParent<Room>() != RoomToEdit)
                {
                    nodeObject = null;
                }
            }

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
                    float radius = currentNode.GetComponent<Node>().NodeConnectRadius / 2;
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
        Debug.Log(currentNode.transform.eulerAngles.y);
        List<Node> nodeNeighbours = grid.GetNeighbours(currentNode.GetComponent<Node>());
        List<bool> isNodeExists = Enumerable.Repeat(false, 6).ToList();

        //  Depending on clockwise direction find sides with hex
        float originPositionX = currentNode.transform.position.x;
        float originPositionZ = currentNode.transform.position.z;
        if ((int)Mathf.Abs(currentNode.transform.eulerAngles.y) % 60 != 0)
        {
            float tmpVal = originPositionX;
            originPositionX = originPositionZ;
            originPositionZ = tmpVal;
        }

        foreach (Node neighbourNode in nodeNeighbours)
        {
            float targetPositionX = neighbourNode.transform.position.x;
            float targetPositionZ = neighbourNode.transform.position.z;
            if ((int)Mathf.Abs(neighbourNode.transform.eulerAngles.y) % 60 != 0)
            {
                float tmpVal = targetPositionX;
                targetPositionX = targetPositionZ;
                targetPositionZ = tmpVal;
            }

            // TODO rework to model width istead of strict values (because sometimes it doesn't see exist nodes)
            if (targetPositionX - originPositionX == 0.0f && targetPositionZ - originPositionZ == -1.0f)
            {
                isNodeExists[0] = true;
            }
            else if (targetPositionX - originPositionX <= -0.8f && targetPositionZ - originPositionZ == -0.5f)
            {
                isNodeExists[1] = true;
            }
            else if (targetPositionX - originPositionX <= -0.8f && targetPositionZ - originPositionZ == 0.5f)
            {
                isNodeExists[2] = true;
            }
            else if (targetPositionX - originPositionX == 0.0f && targetPositionZ - originPositionZ == 1.0f)
            {
                isNodeExists[3] = true;
            }
            else if (targetPositionX - originPositionX >= 0.8f && targetPositionZ - originPositionZ == 0.5f)
            {
                isNodeExists[4] = true;
            }
            else if (targetPositionX - originPositionX >= 0.8f && targetPositionZ - originPositionZ == -0.5f)
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

                float offsetZ = 0f;
                float offsetX = 0f;

                switch (i)
                {
                    case (0):
                        offsetZ -= 1.0f;
                        break;
                    case (1):
                        offsetX -= 0.866f;
                        offsetZ -= 0.5f;
                        break;
                    case (2):
                        offsetX -= 0.866f;
                        offsetZ += 0.5f;
                        break;
                    case (3):
                        offsetZ += 1.0f;
                        break;
                    case (4):
                        offsetX += 0.866f;
                        offsetZ += 0.5f;
                        break;
                    case (5):
                        offsetX += 0.866f;
                        offsetZ -= 0.5f;
                        break;
                }

                if ((int)Mathf.Abs(currentNode.transform.eulerAngles.y) % 60 != 0)
                {
                    float tmpVal = offsetX;
                    offsetX = offsetZ;
                    offsetZ = tmpVal;
                }

                position.x += offsetX;
                position.z += offsetZ;

                GameObject newNode = Instantiate(hexGhostPrefab, position, currentNode.transform.rotation);
                newNode.GetComponent<NodeGhost>().ParentForNode = parent;
                hexGhosts.Add(newNode);
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
            if (hit.transform.GetComponent(comp))
            {
                return hit.transform.gameObject;
            }
        }
        return null;
    }

}
#endif
