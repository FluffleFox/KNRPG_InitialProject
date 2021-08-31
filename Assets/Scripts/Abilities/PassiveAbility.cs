using System.Collections.Generic;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu (menuName = "Abilities/PassiveAbility")]
    public class PassiveAbility : AbilityParams
    {
        public List<AbilityModifiers> abilityModifiers;
        
        public override void Initalize(GameObject obj)
        {
            Debug.Log("DUPA");
        }
    }
}