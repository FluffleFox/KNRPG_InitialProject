using UnityEngine;

public class Node : MonoBehaviour
{

    public readonly struct CubeCoordinates
    {

        public CubeCoordinates(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public int x { get; }
        public int y { get; }
        public int z { get; }
        
    };

    public readonly struct NormalCoordinates
    {
        public NormalCoordinates(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public int x { get; }
        public int y { get; }
    }

    public bool isOccupied = false;
    public NormalCoordinates normalCoordinates;
    public CubeCoordinates cubeCoordinates;

    // For Astar path
    private Node parent = null;
    public Node Parent { get { return parent; } set { parent = value; } }
    private int indexInGrid;
    public int IndexInGrid 
    { 
        get
        {
            // in play mode and var is initialized
            if (Application.isPlaying && indexInGrid != 0) return indexInGrid;
            // in editor mode
            else return transform.GetSiblingIndex(); 
        } 
    }
    private float modelHeight;
    public float ModelHeight 
    { 
        get
        {
            // in play mode and var is initialized
            if (Application.isPlaying && modelHeight != 0.0f) return modelHeight;
            // in editor mode
            else return GetComponent<MeshRenderer>().bounds.size.y;
        } 
    }
    private float modelWidth;
    public float ModelWidth
    {
        get
        {
            // in play mode and var is initialized
            if (Application.isPlaying && modelWidth != 0.0f) return modelWidth;
            // in editor mode
            else return GetComponent<MeshRenderer>().bounds.size.x;
        }
    }
    //

    private void Start()
    {
        Vector3 position = transform.position;
        int x = Mathf.FloorToInt(position.x / 0.866f);
        int z = Mathf.FloorToInt(position.z) - (x - (x & 1)) / 2;
        int y = -x - z;
        normalCoordinates = new NormalCoordinates(x, Mathf.FloorToInt(position.z));
        cubeCoordinates = new CubeCoordinates(x, y, z);

        indexInGrid = transform.GetSiblingIndex();
        modelHeight = GetComponent<MeshRenderer>().bounds.size.y;
        modelWidth  = GetComponent<MeshRenderer>().bounds.size.x;
    }
    private void OnMouseDown()
    {
        Debug.Log("X: " + cubeCoordinates.x + " Y: " + cubeCoordinates.y + "Z: " + cubeCoordinates.z);
        Debug.Log("NORM COORDS: " + normalCoordinates.x + " " + normalCoordinates.y);

    }

    private void OnMouseEnter()
    {
        GetComponent<Renderer>().material.color *= 1.2f;
    }

    private void OnMouseExit()
    {
        GetComponent<Renderer>().material.color /= 1.2f;
    }

    public bool IsPlayerOnNode()
    {
        RaycastHit hit;

        Vector3 upVector = new Vector3(transform.position.x, transform.position.y + 5.0f, transform.position.z);
        if (Physics.Raycast(transform.position, upVector, out hit, Mathf.Infinity, LayerMask.GetMask("Player")))
        {
            Debug.DrawLine(transform.position, upVector, Color.green);
            return true;
        }
        else
        {
            Debug.DrawLine(transform.position, upVector, Color.red);
            return false;
        }
    }
}


