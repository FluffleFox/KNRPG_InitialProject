using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    // Start is called before the first frame update
    Node[,] nodeArray = new Node[200, 200];
    
    void Start()
    {
        foreach (Node child in gameObject.GetComponentsInChildren<Node>())
            {
                //Debug.Log(child.normalCoordinates);
                nodeArray[child.normalCoordinates.x, 
                    child.normalCoordinates.y] = child;
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
