using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orders : MonoBehaviour
{
	[Header("Connections")]
	public GraphGrid Grid;
	public PlanController planController;
	
	public List<Node> Path;
	public int SkillId;
	public Vector3 SkillTarget;
	
	
	
	public void MakePath(Vector3 target)
	{
		
		Node currentNode = Grid.FindNode(transform.position);
		Node startNode = currentNode;
		Node targetNode = Grid.FindNode(target);
		

		if (currentNode != null && targetNode != null)
		{
			List <Node> path = AstarPathfinding.Instance.FindPath(startNode, targetNode);
			if(path == null)
			{
				Debug.Log("astar nie dziala");
				return;
			}
			if (path.Count <= gameObject.GetComponent<Stats>().moveRange)
			{
				Path = path;
				planController.UpdatePathsVisuals();
			}
			else
			{
				Debug.Log("This tile is too far away");
			}
				
		}
	}

}
