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
    //

    private void Start()
    {
        Vector3 position = transform.position;
        int x = Mathf.FloorToInt(position.x / 0.866f);
        int z = Mathf.FloorToInt(position.z) - (x - (x & 1)) / 2;
        int y = -x - z;
        normalCoordinates = new NormalCoordinates(x, Mathf.FloorToInt(position.z));
        cubeCoordinates = new CubeCoordinates(x, y, z);
    }
    private void OnMouseDown()
    {
        //Debug.Log("X: " + cubeCoordinates.x + " Y: " + cubeCoordinates.y + "Z: " + cubeCoordinates.z);
        Debug.Log(normalCoordinates);

    }

    private void OnMouseEnter()
    {
        GetComponent<Renderer>().material.color *= 1.2f;
    }

    private void OnMouseExit()
    {
        GetComponent<Renderer>().material.color /= 1.2f;
    }

}


