using UnityEngine;

namespace TE
{
    /// <summary>
    /// Attaches to Players Sword
    /// </summary>
    public class DamageCollider : MonoBehaviour
    {
        public Collider2D col;

        [HideInInspector]
        public int knockback = 7;

        private void OnTriggerEnter2D(Collider2D other)
        {
            IHit hit = other.GetComponent<IHit>();
            if (hit != null)
            {
                hit.OnHit(this.GetComponentInParent<Player>().damageModifier, other.gameObject);
                AllowHit(false);
            }
        }

        public void AllowHit(bool canHit)
        {
            col.enabled = canHit;
        }
        
        
    }
}