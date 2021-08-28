using System;
using System.Collections.Generic;
using UnityEngine;

public class FightController : MonoBehaviour
{
	public GameObject UnitsParent;
	private Orders2[] AllUnitsOrders;
	
	public void FightStage()
	{
		AllUnitsOrders = UnitsParent.GetComponentsInChildren<Orders2>();
		//Debug.Log(AllUnitsOrders.Length);
		SolveColisions();
		MoveAllUnits();
		CastSkillAllUnits();
	}
	private void MoveAllUnits()
	{
		foreach (Orders2 orders in AllUnitsOrders)
		{
			UnitMovement movement = orders.gameObject.GetComponent<UnitMovement>();
			movement.StartCoroutine(movement.Move(orders.UnitOrder.Path));
		}
	}
	private void CastSkillAllUnits()
	{
		foreach (Orders2 orders in AllUnitsOrders)
		{
			Skills skills = orders.gameObject.GetComponent<Skills>();
			skills.CastSkill(orders.UnitOrder.SkillId, orders.UnitOrder.SkillTarget);
		}
	}
	private int GetLongestPathLength(Orders2[] allOrders)
	{
		int maxLength = 0;
		foreach(Orders2 orders in allOrders )
		{
			if(orders.UnitOrder.Path!=null)
			maxLength = Mathf.Max(maxLength, orders.UnitOrder.Path.Count);
		}
		return maxLength;
	}
	private void SolveColisions()
	{
		for (int tick = 0; tick < GetLongestPathLength(AllUnitsOrders); tick++)
		{
			//check if any unit would like to go to the same position this tick
			for (int i = 0; i < AllUnitsOrders.Length - 1;)
			{
				for (int j = i + 1; j < AllUnitsOrders.Length;)
				{
					if (AllUnitsOrders[i].UnitOrder.Path.Count <= tick || AllUnitsOrders[j].UnitOrder.Path.Count <= tick)
					{
						i++;
						j++;
						continue;
					}
					if (AllUnitsOrders[i].UnitOrder.Path[tick] == AllUnitsOrders[j].UnitOrder.Path[tick])
					{
						if (tick == 0)
						{
							AllUnitsOrders[i].UnitOrder.Path.Insert(tick, AllUnitsOrders[i].gameObject.GetComponent<UnitMovement>().currentNode);
						}
						else
						{
							AllUnitsOrders[i].UnitOrder.Path.Insert(tick, AllUnitsOrders[i].UnitOrder.Path[tick - 1]);
						}
					}
					else
					{
						//go forward only if nothing changed
						i++;
						j++;
					}
				}
			}
		}
	}
}
