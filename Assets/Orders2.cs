using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orders2 : MonoBehaviour
{
	[System.Serializable]
	public struct Order
	{
		public List<Node> Path;
		public int SkillId;
		public Vector3 SkillTarget;
	}
	public Order UnitOrder;
	public GraphGrid Grid;
	public void MakePath(Vector3 target)
	{
		Node currentNode = gameObject.GetComponent<UnitMovement>().currentNode;
		Node startNode = currentNode;
		Node targetNode = Grid.FindNode(target);
		if (currentNode != null && targetNode != null)
		{
			UnitOrder.Path = AstarPathfinding.Instance.FindPath(startNode, targetNode);
		}
	}

}
