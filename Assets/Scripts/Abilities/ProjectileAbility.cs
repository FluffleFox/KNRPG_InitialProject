using UnityEngine;
namespace Abilities
{
    [CreateAssetMenu (menuName = "Abilities/ActiveAbility/ProjectileAbility")]

    public class ProjectileAbility : ActiveAbility
    {

        public override void Initalize(GameObject obj)
        {
            Debug.Log("DUPA");
        }

        public override void Triggers()
        {
            Debug.Log("DUPA");
        }
    }
}