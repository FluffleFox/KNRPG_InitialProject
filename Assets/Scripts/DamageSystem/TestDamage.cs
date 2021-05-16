using System;
using UnityEngine;

namespace DamageSystem
{
    public class TestDamage : MonoBehaviour
    {
        private void Start()
        {
            gameObject.GetComponent<EntityDamage>().Damage(20);
        }
    }
}