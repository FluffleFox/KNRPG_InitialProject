using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnims : MonoBehaviour
{
    public void LeanSizeUP(GameObject trans)
	{
		trans.transform.localScale = Vector2.zero;
		LeanTween.scale(trans, Vector2.one, 0.5f).setEaseOutQuad();
	}
	public void LeanSizeDown(GameObject trans)
	{
		trans.transform.localScale = Vector2.one;
		LeanTween.scale(trans, Vector2.zero, 0.5f).setEaseOutQuad();
	}
}
