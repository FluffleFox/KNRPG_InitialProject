using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = "AbilityParams", menuName = "ScriptableObjects/AbilityParams", order = 3)]
    public abstract class AbilityParams : ScriptableObject
    {

        public string abilityName;
        public Sprite abilityUISprite;

        public abstract void Initalize(GameObject obj);
    }
}