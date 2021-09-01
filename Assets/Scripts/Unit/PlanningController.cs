using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanningController : MonoBehaviour
{
	[Header("Connections")]
	public Orders Orders;
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			//Orders2.MakePath(cameraToCursor);
		}
	}
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
