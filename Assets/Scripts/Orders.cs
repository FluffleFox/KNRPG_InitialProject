using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orders : MonoBehaviour
{
	
	public List<order> OrdersList = new List<order>();

	[Header("Connections")]
	public UnitMovement movement;
	public SkillsManager skills;
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
		if (Input.GetKeyDown(KeyCode.A))
		{
			NewOrder(OrderType.move, new Vector3(transform.position.x - 1f, 0f, transform.position.z ));
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			NewOrder(OrderType.move, new Vector3(transform.position.x + 1f, 0f, transform.position.z ));
		}
		if (Input.GetKeyDown(KeyCode.X))
		{
			NewOrder(OrderType.attack, new Vector3(transform.position.x, 0f, transform.position.z));
		}
		if (Input.GetKeyDown(KeyCode.J))
		{
			NewOrder(OrderType.skill1, new Vector3(transform.position.x, 0f, transform.position.z));
		}
		if (Input.GetKeyDown(KeyCode.K))
		{
			NewOrder(OrderType.skill2, new Vector3(transform.position.x, 0f, transform.position.z));
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			NewOrder(OrderType.skill3, new Vector3(transform.position.x, 0f, transform.position.z));
		}

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
					yield return StartCoroutine(movement.Move(x.param));
					break;
				case OrderType.attack:
					yield return StartCoroutine(movement.Attack(x.param));
					break;
				case OrderType.skill1:
					yield return StartCoroutine(skills.PerformSkill1(x.param));
					break;
				case OrderType.skill2:
					yield return StartCoroutine(skills.PerformSkill2(x.param));
					break;
				case OrderType.skill3:
					yield return StartCoroutine(skills.PerformSkill3(x.param));
					break;
				case OrderType.wait:
					yield return new WaitForSeconds(3f);
					break;
			}
		}
		OrdersList = new List<order>();
	}
}
