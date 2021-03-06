using UnityEngine;
public class EntityDamage : MonoBehaviour, IDamageable
    {
        private IDamageable damageable;
     
        private void Start()
        {
            damageable = (IDamageable)GetComponent(typeof(IDamageable));
            if (damageable == null)
            {
                throw new MissingComponentException("Requires an implementation of IDamageable");
            }
        }

        public void Damage(uint damageTaken)
        {
            gameObject.GetComponent<UnitStats>().health -= damageTaken;
            Debug.Log("Im damaged");
        }
    }