using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsManager : MonoBehaviour
{
	public string Skill1;
	public string Skill2;
	public string Skill3;
	public IEnumerator PerformSkill1(Vector3 position)
	{
		Debug.Log("Casting spell " + Skill1 + " on position: " + position);
		yield return null;
	}
	public IEnumerator PerformSkill2(Vector3 position)
	{
		Debug.Log("Casting spell " + Skill2 + " on position: " + position);
		yield return null;
	}
	public IEnumerator PerformSkill3(Vector3 position)
	{
		Debug.Log("Casting spell " + Skill3 + " on position: " + position);
		yield return null;
	}
}
