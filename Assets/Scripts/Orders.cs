using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orders : MonoBehaviour
{
	public List<order> OrdersList;

	[Header("Connections")]
	public UnitMovement movement;

	public enum OrderType
	{
		move,
		attack,
		skill,
		interact,
		wait,
		fortify 
	}
	public class order
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
	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.W))
		{
			NewOrder(OrderType.move, new Vector3(transform.position.x, 0f, transform.position.z + 1f));
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			NewOrder(OrderType.move, new Vector3(transform.position.x, 0f, transform.position.z - 1f));
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			StartCoroutine(ExecuteOrders());
			
			Debug.Log(OrdersList.Count);
		}
	}
	public void NewOrder(OrderType orderType, Vector3 param)
	{
		OrdersList.Add(new order(orderType,param));
	}
	IEnumerator ExecuteOrders()
	{
		foreach(order x in OrdersList)
		{
			switch(x.type)
			{
				case OrderType.move:
					yield return StartCoroutine(movement.Move(x.param));
					break;
			}
		}
		OrdersList = new List<order>();
	}
}
