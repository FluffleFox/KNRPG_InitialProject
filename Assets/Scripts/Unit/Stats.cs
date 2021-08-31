using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
	public float maxHealth = 100f;
	public float maxMana = 30f;
	public float baseDamage = 10f;
	public int baseSpeed = 2;
	public int moveRange = 4;

	private float health = 100f;
	private float mana = 30f;
	private float damage = 10f;
	private int speed = 2;
	private void Start()
	{
		health = maxHealth;
		mana = maxMana;
		damage = baseDamage;
		speed = baseSpeed;
		//load stats
	}
}
