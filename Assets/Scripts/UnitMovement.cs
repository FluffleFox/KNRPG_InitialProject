using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
	[Header("Connections")]
	public Animator animator;
	[Header("Parameters")]
	public float moveTime = 0.5f;
	public IEnumerator Move(Vector3 targetPosition)
	{
		Debug.Log("it works");
		animator.SetFloat("Speed", 1f);
		Vector3 startPos = transform.position;
		float startTime = Time.time;
		while(startTime+moveTime>Time.time)
		{
			transform.position = Vector3.Lerp(startPos, targetPosition, (Time.time - startTime) / moveTime);
			yield return null;
		}
		animator.SetFloat("Speed", 0f);
	}
}
