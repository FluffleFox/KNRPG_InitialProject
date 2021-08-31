using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour
{
	public string Skill1;
	public string Skill2;
	public string Skill3;
	private IEnumerator PerformSkill1(Vector3 position)
	{
		Debug.Log("Casting spell " + Skill1 + " on position: " + position);
		yield return null;
	}
	private IEnumerator PerformSkill2(Vector3 position)
	{
		Debug.Log("Casting spell " + Skill2 + " on position: " + position);
		yield return null;
	}
	private IEnumerator PerformSkill3(Vector3 position)
	{
		Debug.Log("Casting spell " + Skill3 + " on position: " + position);
		yield return null;
	}
	public void CastSkill(int skillId, Vector3 target)
	{
		switch(skillId)
		{
			case 1:
				StartCoroutine(PerformSkill1(target));
				break;
			case 2:
				StartCoroutine(PerformSkill2(target));
				break;
			case 3:
				StartCoroutine(PerformSkill3(target));
				break;
			default:
				Debug.Log("Incorrect skill Id: " + skillId);
				break;
		}
	}
}
