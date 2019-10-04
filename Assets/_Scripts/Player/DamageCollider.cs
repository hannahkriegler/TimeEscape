using UnityEngine;

namespace TE
{
    /// <summary>
    /// Attaches to Players Sword
    /// </summary>
    public class DamageCollider : MonoBehaviour
    {
        public Collider2D col;

        public bool isBossCollider;

        private void OnTriggerEnter2D(Collider2D other)
        {
            IHit hit = other.GetComponent<IHit>();
            if (hit != null)
            {
                int damage = isBossCollider ? GetComponentInParent<FinalBoss>().attackDamage : GetComponentInParent<Player>().damageModifier;
                hit.OnHit(damage, other.gameObject);
                AllowHit(false);
            }
        }

        public void AllowHit(bool canHit)
        {
            col.enabled = canHit;
        }
    }
}