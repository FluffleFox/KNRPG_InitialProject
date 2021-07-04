using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For move just LEFT MOUSE CLICK on any Hex node
public class TestMovement : MonoBehaviour
{
	[Header("Connections")]
	public Animator animator;
	[Header("Parameters")]
	// Time to move overcome 1 hex
	public float moveTime = 0.25f;
	[Space(10)]
	[SerializeField] private Room currentRoom;
	private GraphGrid grid;
	private Node currentNode;
	public Node CurrentNode { get { return currentNode; } }


    private IEnumerator Start()
    {
		yield return null;
		SetPlayerPosition();
    }

	public void SetPlayerPosition()
    {
		currentNode = GetOnNode(transform.position);
		if (currentNode)
		{
			currentRoom = currentNode.GetComponentInParent<Room>();
			currentRoom.IsPlayerInside = true;
			grid = currentRoom.Grid;
			currentNode.isOccupied = true;

			transform.position = currentNode.transform.position;
		}
	}

    public IEnumerator Move(Node targetNode, List<Node> path)
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
    private void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			Node startNode = currentNode;
			var target = GetMouseOverlap(typeof(Node));
			if (currentNode && target)
			{
				Node targetNode = target.GetComponent<Node>();
				List<Node> path = new List<Node>();
				path = AstarPathfinding.Instance.FindPath(startNode, targetNode);
				if (path != null && !targetNode.isOccupied)
				{
					StopAllCoroutines();
					StartCoroutine(Move(targetNode, path));
				}
				else
				{
					Debug.Log("No possible way");
				}
				
			}
			else
			{
				Debug.Log("No node exists way");
			}
		}
    }


	private GameObject GetMouseOverlap(System.Type comp)
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100f))
		{
			if (hit.transform.GetComponent(comp))
			{
				return hit.transform.gameObject;
			}
		}
		return null;
	}

	private Node GetOnNode(Vector3 position)
	{
		RaycastHit hit;
		if (Physics.Raycast(position, -Vector3.up, out hit))
		{
			Debug.DrawRay(position, -Vector3.up, Color.black, 5f);
			if (hit.transform.GetComponent<Node>())
			{
				return hit.transform.GetComponent<Node>();
			}
		}
		return null;
	}
}

