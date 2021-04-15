using UnityEngine;

public class Node : MonoBehaviour
{
    //Cube coordinates
    int x;
    int y;
    int z;

    private void Start()
    {
        x = Mathf.FloorToInt(transform.position.x / 0.866f);
        z = Mathf.FloorToInt(transform.position.z) - (x - (x & 1)) / 2;
        y = -x - z;
    }

    private void OnMouseDown()
    {
        Debug.Log("X: " + x + " Y: " + y + "Z: " + z);
    }
}
