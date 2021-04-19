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

	[SerializeField] GraphGrid grid;
	private Node currentNode;
    private IEnumerator Start()
    {
		yield return null;
		currentNode = grid.FindNode(transform.position);
		currentNode.isOccupied = true;
    }
    public IEnumerator Move(Vector3 targetPosition)
	{
		Node startNode = currentNode;
		Node targetNode = grid.FindNode(targetPosition);
		List<Node> path = new List<Node>();
		if (currentNode != null && targetNode != null)
        {
			path = AstarPathfinding.Instance.FindPath(startNode, targetNode);
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
	}
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
			StopAllCoroutines();
			var clickPoint = cursorOnTransform;
			StartCoroutine(Move(clickPoint));
		}
    }

	// Get mouse click world coordinates helpers
	private static Vector3 cursorWorldPosOnNCP
	{
		get
		{
			return Camera.main.ScreenToWorldPoint(
				new Vector3(Input.mousePosition.x,
				Input.mousePosition.y,
				Camera.main.nearClipPlane));
		}
	}
	private static Vector3 cameraToCursor
	{
		get
		{
			return cursorWorldPosOnNCP - Camera.main.transform.position;
		}
	}
	private Vector3 cursorOnTransform
	{
		get
		{
			Vector3 camToTrans = transform.position - Camera.main.transform.position;
			return Camera.main.transform.position +
				cameraToCursor *
				(Vector3.Dot(Camera.main.transform.forward, camToTrans) / Vector3.Dot(Camera.main.transform.forward, cameraToCursor));
		}
	}
}
