using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuAnims : MonoBehaviour
{
	public float time = 0.3f;
	
	private void OnEnable()
	{
		transform.localScale = Vector3.zero;
		LeanTween.scale(gameObject, Vector3.one, time).setEaseInQuad();
	}
	public void Close()
	{
		transform.localScale = Vector3.one;
		LeanTween.scale(gameObject, Vector3.zero, time).setEaseOutQuad().setOnComplete(OnComplete);
	}
	private void OnComplete()
	{
		gameObject.SetActive(false);
	}
}
