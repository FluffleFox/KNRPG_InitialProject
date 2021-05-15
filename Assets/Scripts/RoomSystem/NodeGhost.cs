using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
public class NodeGhost : MonoBehaviour
{
    [SerializeField] private GameObject nodePrefab;
    private Transform parentForNode;
    public Transform ParentForNode { set { parentForNode = value; } }

    public void Destroy()
    {
        DestroyImmediate(gameObject);
    }
    public void HighlightGhost()
    {
        GetComponent<Renderer>().material.color *= 1.2f;
    }
    public void SetDefaultGhost()
    {
        GetComponent<Renderer>().material.color /= 1.2f;
    }
    public void InitNode()
    {
        GameObject nodeObj = Instantiate(nodePrefab, transform.position, Quaternion.identity, parentForNode);
        nodeObj.transform.name = System.String.Format("{0} ({1})",nodeObj.transform.name, parentForNode.childCount);
        nodeObj.transform.parent = parentForNode;
        DestroyImmediate(gameObject);
    }
}
#endif
