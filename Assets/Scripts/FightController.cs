using System;
using System.Collections.Generic;
using UnityEngine;

public class FightController : MonoBehaviour
{
	public GameObject UnitsParent;
	private Orders[] AllUnitsOrders;
	
	public void FightPhase()
	{
		AllUnitsOrders = UnitsParent.GetComponentsInChildren<Orders>();
		//Debug.Log(AllUnitsOrders.Length);
		SolveColisions();
		MoveAllUnits();
		CastSkillAllUnits();
	}
	private void MoveAllUnits()
	{
		foreach (Orders orders in AllUnitsOrders)
		{
			UnitMovement movement = orders.gameObject.GetComponent<UnitMovement>();
			movement.StartCoroutine(movement.Move(orders.Path));
		}
	}
	private void CastSkillAllUnits()
	{
		foreach (Orders orders in AllUnitsOrders)
		{
			Skills skills = orders.gameObject.GetComponent<Skills>();
			skills.CastSkill(orders.SkillId, orders.SkillTarget);
		}
	}
	private int GetLongestPathLength(Orders[] allOrders)
	{
		int maxLength = 0;
		foreach(Orders orders in allOrders )
		{
			if(orders.Path!=null)
			maxLength = Mathf.Max(maxLength, orders.Path.Count);
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
					if (AllUnitsOrders[i].Path.Count <= tick || AllUnitsOrders[j].Path.Count <= tick)
					{
						i++;
						j++;
						continue;
					}
					if (AllUnitsOrders[i].Path[tick] == AllUnitsOrders[j].Path[tick])
					{
						if (tick == 0)
						{
							AllUnitsOrders[i].Path.Insert(tick, AllUnitsOrders[i].gameObject.GetComponent<UnitMovement>().currentNode);
						}
						else
						{
							AllUnitsOrders[i].Path.Insert(tick, AllUnitsOrders[i].Path[tick - 1]);
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
