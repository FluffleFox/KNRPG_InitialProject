using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(Orders))]
public class Enemy : MonoBehaviour
{
	public UnityEvent planTurn;
	public Orders orders;
	private void Start()
	{
		if(orders = null)
		{
			orders = GetComponent<Orders>();
		}
	}
	
	public void MakePath(Node target)
	{
		orders.MakePath(target.transform.position);
	}
	public void SetSkillId(int id)
	{
		orders.SkillId = id;
	}
	public void SetSkillTarget(Vector3 target)
	{
		orders.SkillTarget = target;
	}
}
