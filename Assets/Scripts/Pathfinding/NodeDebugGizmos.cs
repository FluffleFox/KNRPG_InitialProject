using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class NodeDebugGizmos : MonoBehaviour
{
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // Draw if only this object is selected
        if (Selection.activeTransform == transform)
        {
            float sphereRadius = transform.parent.GetComponent<GraphGrid>().SphereRadius;
            Vector3 originPosition = transform.position;
            originPosition.y += sphereRadius + GetComponent<MeshRenderer>().bounds.size.y;

            if (!GetComponent<Node>().isOccupied)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.red;
            }
            try
            {
                var neighbours = transform.parent.GetComponent<GraphGrid>().GetNeighbours(transform.GetSiblingIndex());
                foreach (var node in neighbours)
                {
                    Vector3 targetPosition = node.transform.position;
                    targetPosition.y += sphereRadius + node.GetComponent<MeshRenderer>().bounds.size.y;
                    Gizmos.DrawLine(originPosition, targetPosition);
                    Gizmos.DrawSphere(targetPosition, sphereRadius);
                }
                Gizmos.DrawSphere(originPosition, sphereRadius);
            }
            catch (System.NullReferenceException e)
            {
                Debug.Log("DEV INFO: Graph map IS NOT generated, CLICK ON 'Generate Graph Map' on 'GraphGrid.cs'");
            }
        }
    }

#endif
}
