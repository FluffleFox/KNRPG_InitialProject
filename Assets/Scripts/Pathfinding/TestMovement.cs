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
		OnRoomEnter();
    }

	public void OnRoomEnter()
	{ 
		currentNode = GetOnNode(transform.position);
		Debug.Log(currentNode);
		if (currentNode)
		{
			currentRoom = currentNode.GetComponentInParent<Room>();
			currentRoom.IsPlayerInside = true;
			grid = currentRoom.Grid;
			currentNode.isOccupied = true;

			transform.position = new Vector3(currentNode.transform.position.x, currentNode.transform.position.y + 0.25f, currentNode.transform.position.z);
		}
	}

    public IEnumerator Move(Node targetNode, List<Node> path)
	{
		animator.SetFloat("Speed", 1f);
		foreach (Node node in path)
        {
			Vector3 startPos = transform.position;
			Vector3 nextStepPos = new Vector3(node.transform.position.x, node.transform.position.y + 0.25f, node.transform.position.z);

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

	public void InitMove(GameObject targetNodeObject)
    {
		// Exit if node not exists
		if (!targetNodeObject || !currentNode)
        {
			Debug.Log("No node exists way");
			return;
        }

		Node startNode = currentNode;
		Node targetNode = targetNodeObject.GetComponent<Node>();

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

    private void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			var target = GetMouseOverlap(typeof(Node));
			InitMove(target);
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
		if (Physics.Raycast(position, -Vector3.up, out hit, 100f))
		{
			Debug.Log(hit.transform.name);
			Debug.DrawRay(position, -Vector3.up, Color.black, 5f);
			if (hit.transform.GetComponent<Node>())
			{
				return hit.transform.GetComponent<Node>();
			}
		}
		return null;
	}
}

