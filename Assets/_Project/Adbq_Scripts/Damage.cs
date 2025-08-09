using UnityEngine;

namespace Platformer
{
    public class Damage : MonoBehaviour
    {
        [SerializeField] int damageAmount = 10;
        [SerializeField] LayerMask targetLayer;

        private void OnTriggerEnter(Collider other)
        {
            // Check if the collided object is on the target layer
            if ((targetLayer.value & (1 << other.gameObject.layer)) != 0)
            {
                Health targetHealth = other.GetComponent<Health>();
                if (targetHealth != null && !targetHealth.IsDead)
                {
                    Debug.Log($"Applying {damageAmount} damage to {targetHealth.gameObject.name}");
                    targetHealth.TakeDamage(damageAmount);
                }
            }
        }
    }
}
