using UnityEngine;
namespace Abilities
{
    public abstract class ActiveAbility : AbilityParams
    {
        public AbilityTargets abilityTargets;
        public abstract void Triggers();
    }
}