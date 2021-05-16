using System;
using UnityEngine;
public class TestDamage : MonoBehaviour

    {
        public Ability fireball;
        public GameObject goToDamage;
        public void TestBasicDamage()
        {
            goToDamage.GetComponent<EntityDamage>().Damage(fireball.damage);
            Debug.Log("Damage dealt");
        }
    }