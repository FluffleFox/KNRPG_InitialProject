using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Enemy))]
public class EnemyAI : MonoBehaviour
{
	public Enemy enemy;
	public int moveRange = 3;
	public void AddPlan(UnityAction action)
	{
		if(enemy.planTurn == null)
		{
			enemy.planTurn = new UnityEvent();
		}
		enemy.planTurn.AddListener(action);
	}
	public Node[] GetNodesInRange(int range)
	{
		return new Node[1];
	}
}
