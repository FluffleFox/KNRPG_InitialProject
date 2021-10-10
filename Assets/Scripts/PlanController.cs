using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlanController : MonoBehaviour
{
	public GameObject ArrowPrefab;
	public float arrowYcord = 0.3f;
	[SerializeField] private Material nodeInRangeMaterial;
	[SerializeField] private Material normalNodeMaterial;
	[SerializeField] private Material activeUnitNodeMaterial;
	private struct UnitAndOrder
	{
		public GameObject GO;
		public Orders orders;
		public LineRenderer arrow;
	}
	private UnitAndOrder[] Units;
	private UnitAndOrder ActiveUnit;

	private int activeSkillId;

    [System.Serializable]
	public enum Status
	{
		Move,
		Skill
	}
	[SerializeField]
	private Status status;

    void Awake()
    {
		GetUnits();
		ChangeActiveUnit(Units[0].GO);
		UpdatePathsVisuals();
		status = Status.Move;
	}

    
    void Update()
    {
		
        if(Input.GetKeyDown(KeyCode.F1))
		{
			ChangeActiveUnit(Units[0].GO);
		}
		if (Input.GetKeyDown(KeyCode.F2))
		{
			ChangeActiveUnit(Units[1].GO);
		}
		if (Input.GetKeyDown(KeyCode.F3))
		{
			ChangeActiveUnit(Units[2].GO);
		}
		switch(status)
		{
			case Status.Move:
				//ShowMoveRange();
				if (Input.GetMouseButtonDown(1))
				{
					MakeNewPath();
				}
				break;
			case Status.Skill:
				ShowSkillRange();
				break;
			default:
				Debug.Log("no plan controller status");
				break;
		}
		
	}
	
	public void ChangeStatusToSkill(int skillID)
	{
		status = Status.Skill;

	}
	public void ChangeActiveUnit(GameObject newActiveUnit)
	{
		ShowMoveRange(false, ActiveUnit);
		ActiveUnit = FindUnitByGO(newActiveUnit);
		Debug.Log("New active unit is " + ActiveUnit.GO.name);
		ShowMoveRange(true, ActiveUnit);
		DrawPath();
		//change ui elements
	}
	private UnitAndOrder FindUnitByGO(GameObject newActiveUnit)
	{
		for (int i = 0; i < Units.Length; i++)
		{
			if (Units[i].GO == newActiveUnit)
			{
				return Units[i];
			}
		}
		return new UnitAndOrder();
	}
	private void MakeNewPath()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out RaycastHit raycastHit))
		{
			ActiveUnit.orders.MakePath(raycastHit.point);
			UpdatePathsVisuals();
		}
	}
	public void UpdatePathsVisuals()
	{
		DrawPath();
	}
	private void GetUnits()
	{
		GameObject[] GOs = GameObject.FindGameObjectsWithTag("Player");

		if (GOs.Length == 0)
		{
			Debug.LogError("Did not find any units!");
			return;
		}
		Units = new UnitAndOrder[GOs.Length];
		for (int i = 0; i < GOs.Length; i++)
		{
			Units[i].GO = GOs[i];
			Units[i].orders = Units[i].GO.GetComponent<Orders>();
			Units[i].arrow = Instantiate(ArrowPrefab,Units[i].GO.transform).GetComponent<LineRenderer>();
		}

	}
	private void DrawPath()
	{
		foreach(UnitAndOrder unit in Units)
		{
			Vector3[] newPositions = new Vector3[unit.orders.Path.Count + 1];
			newPositions[0] = unit.GO.transform.position;
			for (int i = 1; i <= unit.orders.Path.Count; i++)
			{
				Vector3 nextPos = unit.orders.Path[i - 1].transform.position;
				newPositions[i] = nextPos;
			}
			for (int i = 0; i < unit.orders.Path.Count+1; i++)
			{
				newPositions[i].y = arrowYcord;
			}
			unit.arrow.positionCount = newPositions.Length;
			unit.arrow.SetPositions(newPositions);
			unit.arrow.startWidth = 0.1f;
			unit.arrow.endWidth = 0.1f;
		}
		ActiveUnit.arrow.startWidth = 0.2f;
		ActiveUnit.arrow.endWidth = 0.2f;
	}
	private void ShowMoveRange(bool show, UnitAndOrder unit)
	{
		if (unit.orders == null) return;
		GraphGrid grid = unit.orders.Grid;
		List<Node> nodesInRange = grid.GetAllNodesInRange(grid.FindNode(unit.GO.transform.position), unit.GO.GetComponent<Stats>().moveRange);
		foreach(Node node in nodesInRange)
		{
			if(show)
			{
				node.gameObject.GetComponent<MeshRenderer>().material = nodeInRangeMaterial;
			}
			else
			{
				node.gameObject.GetComponent<MeshRenderer>().material = normalNodeMaterial;
			}
		}
		if (show)
		{
			Node activeUnitNode = grid.FindNode(unit.GO.transform.position);
			activeUnitNode.gameObject.GetComponent<MeshRenderer>().material = activeUnitNodeMaterial;
		}
	}
	private void ShowSkillRange()
	{
		//Debug.Log("If only i knew the nodes in range...");
	}
	public void EndPlanningPhase()
	{

		ShowMoveRange(false, ActiveUnit);
		foreach (UnitAndOrder unit in Units)
		{
			unit.arrow.positionCount = 0;
		}
	}
}
