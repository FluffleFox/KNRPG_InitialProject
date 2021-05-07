using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableUnit : MonoBehaviour
{
    [SerializeField] protected GraphGrid grid;
    [SerializeField] protected ItemData itemData;
    protected Node currentNode;
    protected List<Node> rangeNodes;
    protected bool inRange;


    protected virtual void Awake()
    {
        inRange = false;
        currentNode = grid.FindNode(transform.position);
        StartCoroutine(InitRangeNodes());
    }

    private IEnumerator InitRangeNodes()
    {
        yield return null; // wait for grid initialization

        rangeNodes = new List<Node>();
        List<Node> helperList = new List<Node>();
        List<Node> containerList = new List<Node>();
        rangeNodes.Add(currentNode);
        helperList.Add(currentNode);

        for (int i = 0; i < itemData.RangeToInteract; i++)
        {
            foreach (var nodeMain in helperList)
            {
                foreach (var nodeNeighbour in grid.GetNeighbours(nodeMain))
                {
                    if (!rangeNodes.Contains(nodeNeighbour))
                    {
                        containerList.Add(nodeNeighbour);
                    }
                }
            }
            helperList.Clear();
            helperList = new List<Node>(containerList);
            rangeNodes.AddRange(containerList);
            containerList.Clear();
        }
    }

    protected virtual void Update()
    {
        if (rangeNodes != null)
        {
            inRange = IsPlayerInRange();
        }
    }

    public virtual bool IsPlayerInRange()
    {
        foreach(var node in rangeNodes)
        {
            if (node.IsPlayerOnNode())
            {
                return true;
            }
        }
        return false;
    }

    protected virtual void Interact()
    {

    } 

#if UNITY_EDITOR
    protected virtual void OnDrawGizmosSelected()
    {
        // Only in game mode, when grid is initialized
        if (Application.isPlaying)
        {
            try
            {
                Gizmos.color = Color.blue;

                currentNode = grid.FindNode(transform.position);
                InitRangeNodes();
                foreach (var node in rangeNodes)
                {
                    Vector3 positionNode = new Vector3(node.transform.position.x, node.transform.position.y + grid.SphereRadius + node.ModelHeight, node.transform.position.z);
                    Gizmos.DrawSphere(positionNode, grid.SphereRadius);
                }
            }
            catch (System.NullReferenceException e) { }
        }
    }
#endif
}
