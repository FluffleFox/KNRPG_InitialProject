using UnityEngine;

    [CreateAssetMenu(fileName = "Ability", menuName = "ScriptableObjects/Ability", order = 3)]
    public class Ability : ScriptableObject
    {
        public enum AbilityType
            {
                PASSIVE,
                ACTIVE
            }
        
        public AbilityType abilityType;
        public string abilitynName;
        public Texture abilityUITexture;
        public uint damage;
    }