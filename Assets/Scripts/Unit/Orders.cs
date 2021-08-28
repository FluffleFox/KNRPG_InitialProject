using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orders : MonoBehaviour
{
	
	public List<order> OrdersList = new List<order>();

	[Header("Connections")]
	public UnitMovement movement;
	public Skills skills;
	public GraphGrid grid;

	private Node currentNode;
	public enum OrderType
	{
		move,
		attack,
		skill1,
		skill2,
		skill3,
		interact,
		wait,
		fortify 
	}
	[System.Serializable]
	public struct order
	{
		public OrderType type;
		public Vector3 param;
		public order(OrderType newType, Vector3 newParam)
		{
			type = newType;
			param = newParam;
		}
		
	}
	private void Start()
	{
		OrdersList = new List<order>();
	}
	public void ShowOrders()
	{
		foreach (order x in OrdersList)
		{
			switch (x.type)
			{
				case OrderType.move:
					ShowMove(x.param);
					break;
				case OrderType.attack:
					
					break;
				case OrderType.skill1:
					
					break;
				case OrderType.skill2:
					
					break;
				case OrderType.skill3:
					
					break;
				case OrderType.wait:
					
					break;
			}
		}
	}
	void ShowMove(Vector3 target)
	{
		List<Node> path = MakePath(target);
		foreach(Node node in path)
		{
			node.GetComponent<Renderer>().material.color /= 2f;
		}
	}
	List<Node> MakePath(Vector3 target)
	{
		currentNode = grid.FindNode(transform.position);
		Node startNode = currentNode;
		Node targetNode = grid.FindNode(target);
		List<Node> path = new List<Node>();
		if (currentNode != null && targetNode != null)
		{
			path = AstarPathfinding.Instance.FindPath(startNode, targetNode);
		}
		return path;
	}
	public void NewOrder(OrderType orderType, Vector3 param)
	{
		OrdersList.Add(new order(orderType,param));
	}
	public void ExecuteOrdersButton()
	{
		StartCoroutine(ExecuteOrders());
	}
	IEnumerator ExecuteOrders()
	{
		foreach(order x in OrdersList)
		{
			switch(x.type)
			{
				case OrderType.move:
					yield return StartCoroutine(movement.Move(MakePath(x.param)));
					break;
				case OrderType.attack:
					yield return StartCoroutine(movement.Attack(x.param));
					break;
				case OrderType.skill1:
					//yield return StartCoroutine(skills.PerformSkill1(x.param));
					break;
				case OrderType.skill2:
					//yield return StartCoroutine(skills.PerformSkill2(x.param));
					break;
				case OrderType.skill3:
					//yield return StartCoroutine(skills.PerformSkill3(x.param));
					break;
				case OrderType.wait:
					yield return new WaitForSeconds(3f);
					break;
			}
		}
		OrdersList = new List<order>();
	}
	
}
