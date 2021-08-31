using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
	public Node currentNode;
	[Header("Connections")]
	public Animator animator;
	[SerializeField] GraphGrid grid;
	[Header("Parameters")]
	public float moveTime = 0.5f;
	
	
	private IEnumerator Start()
	{
		yield return null;
		currentNode = grid.FindNode(transform.position);
		currentNode.isOccupied = true;
	}
	public IEnumerator Move(List<Node> path)
	{
		if (path != null)
		{
			animator.SetFloat("Speed", 1f);
			foreach (Node node in path)
			{

				Vector3 startPos = transform.position;
				Vector3 nextStepPos = node.transform.position;

				transform.LookAt(nextStepPos);

				float startTime = Time.time;
				while (startTime + moveTime > Time.time)
				{
					transform.position = Vector3.Lerp(startPos, nextStepPos, (Time.time - startTime) / moveTime);
					yield return null;
				}
				grid.UpdateGrid(currentNode, node);
				currentNode = node;
			}
			animator.SetFloat("Speed", 0f);
		}
		else
		{
			Debug.Log("No possible path");
		}
	}
	public IEnumerator Attack(Vector3 targetPosition)
	{
		animator.SetBool("Jump", true);
		Vector3 startPos = transform.position;
		float startTime = Time.time;
		while (startTime + moveTime > Time.time)
		{
			transform.position = Vector3.Lerp(startPos, targetPosition, (Time.time - startTime) / moveTime);
			yield return null;
		}
		animator.SetBool("Jump", false);
	}
}
