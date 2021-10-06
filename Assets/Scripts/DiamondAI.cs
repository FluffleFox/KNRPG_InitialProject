using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondAI : EnemyAI
{
	private Node[] possibleMoves;
	private int[] moveValue;
	private void Start()
	{
		AddPlan(MakePath);
	}
	public void MakePath()
	{
		possibleMoves = GetNodesInRange(moveRange);
		moveValue = new int[possibleMoves.Length];
		CalculateMoveValues();
		Node target = FindBestMove();
		enemy.MakePath(target);
	}
	private void CalculateMoveValues()
	{

	}
	private Node FindBestMove()
	{
		int maxVal = Mathf.Max(moveValue);
		int index = System.Array.IndexOf(moveValue, maxVal);
		return possibleMoves[index];
	}
}
