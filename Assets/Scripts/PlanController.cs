using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlanController : MonoBehaviour
{
	public GameObject ArrowPrefab;
	public float arrowYcord = 0.3f;
	private struct UnitAndOrder
	{
		public GameObject GO;
		public Orders2 orders;
		public LineRenderer arrow;
	}
	private UnitAndOrder[] Units;
	private UnitAndOrder ActiveUnit;
    // Start is called before the first frame update
    void Awake()
    {
		GetUnits();
		ChangeActiveUnit(Units[0].GO);
		UpdatePathsVisuals();
	}

    // Update is called once per frame
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
		if(Input.GetMouseButtonDown(1))
		{
			MakeNewPath();
		}
	}

	public void ChangeActiveUnit(GameObject newActiveUnit)
	{
		for (int i = 0; i < Units.Length; i++)
		{
			if(Units[i].GO == newActiveUnit)
			{
				ActiveUnit = Units[i];
				break;
			}
		}
		//get all nodes in move range -> mark range
		//change ui elements
	}
	private void MakeNewPath()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out RaycastHit raycastHit))
		{
			ActiveUnit.orders.MakePath(raycastHit.point);
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
			Units[i].orders = Units[i].GO.GetComponent<Orders2>();
			Units[i].arrow = Instantiate(ArrowPrefab,Units[i].GO.transform).GetComponent<LineRenderer>();
		}

	}
	private void DrawPath()
	{
		foreach(UnitAndOrder unit in Units)
		{
			Vector3[] newPositions = new Vector3[unit.orders.UnitOrder.Path.Count + 1];
			newPositions[0] = unit.GO.transform.position;
			for (int i = 1; i <= unit.orders.UnitOrder.Path.Count; i++)
			{
				Vector3 nextPos = unit.orders.UnitOrder.Path[i - 1].transform.position;
				newPositions[i] = nextPos;
			}
			for (int i = 0; i < unit.orders.UnitOrder.Path.Count+1; i++)
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
}
