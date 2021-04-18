using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    // Start is called before the first frame update
    Node[,] nodeArray = new Node[200, 200];

    Node selectedNode1;
    Node selectedNode2;
    void Start()
    {
        foreach (Node child in gameObject.GetComponentsInChildren<Node>())
        {
            //Debug.Log(child.normalCoordinates);
            nodeArray[child.normalCoordinates.x, 
                      child.normalCoordinates.y] = child;
        }
        selectedNode1 = nodeArray[0, 1];
        selectedNode1 = nodeArray[10, 1];
    }
    // Update is called once per frame
    
    void Update()
    {
        
    }

    



    private Node[] getNeighbouringNodes(Node node){
        Node[] neighbours = new Node[6];
        if(node.normalCoordinates.x % 2 == 0)
        {
            neighbours[0] = nodeArray[node.normalCoordinates.x, 
                            node.normalCoordinates.y + 1];
            neighbours[1] = nodeArray[node.normalCoordinates.x + 1, 
                            node.normalCoordinates.y + 1];
            neighbours[2] = nodeArray[node.normalCoordinates.x + 1, 
                            node.normalCoordinates.y];
            neighbours[3] = nodeArray[node.normalCoordinates.x, 
                            node.normalCoordinates.y - 1];
            neighbours[4] = nodeArray[node.normalCoordinates.x - 1, 
                            node.normalCoordinates.y];
            neighbours[5] = nodeArray[node.normalCoordinates.x - 1, 
                            node.normalCoordinates.y + 1];
        }
        else
        {
            neighbours[0] = nodeArray[node.normalCoordinates.x, 
                            node.normalCoordinates.y + 1];
            neighbours[1] = nodeArray[node.normalCoordinates.x + 1, 
                            node.normalCoordinates.y];
            neighbours[2] = nodeArray[node.normalCoordinates.x + 1, 
                            node.normalCoordinates.y - 1];
            neighbours[3] = nodeArray[node.normalCoordinates.x, 
                            node.normalCoordinates.y - 1];
            neighbours[4] = nodeArray[node.normalCoordinates.x - 1, 
                            node.normalCoordinates.y - 1];
            neighbours[5] = nodeArray[node.normalCoordinates.x - 1, 
                            node.normalCoordinates.y];
        }
        return neighbours;
    }

    private void OnMouseDown()
    {
        Debug.Log("XD");

    }
    private void OnMouseEnter()
    {

    }

    private void OnMouseExit()
    {

    }
}
