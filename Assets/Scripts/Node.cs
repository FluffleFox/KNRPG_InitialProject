using UnityEngine;

public class Node : MonoBehaviour
{

    public readonly struct CubeCoordinates
    {

        public CubeCoordinates(int X, int Y, int Z)
        {
            x = X;
            y = Y;
            z = Z;
        }
        public int x { get; }
        public int y { get; }
        public int z { get; }
        
    };

    public Vector2Int normalCoordinates;

    public bool isOccupied = false;
    public CubeCoordinates cubeCoordinates;

    private void Start()
    {
        Vector3 position = transform.position;
        int x = Mathf.FloorToInt(position.x / 0.866f);
        int z = Mathf.FloorToInt(position.z) - (x - (x & 1)) / 2;
        int y = -x - z;
        cubeCoordinates = new CubeCoordinates(x, y, z);

        normalCoordinates = new Vector2Int(x, Mathf.FloorToInt(position.z));

    }

    private void OnMouseDown()
    {
        Debug.Log("X: " + cubeCoordinates.x + " Y: " + cubeCoordinates.y + "Z: " + cubeCoordinates.z);
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


