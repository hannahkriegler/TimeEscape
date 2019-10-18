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

        public LayerMask layerMask;

        bool canHit;

        private void Update()
        {
            if (canHit)
            {
                //Physic2D.OverlapArea works better on static objects like music box
                Collider2D[] hits = Physics2D.OverlapAreaAll(col.bounds.min, col.bounds.max, layerMask);
                foreach (Collider2D hit in hits)
                {
                    Damage(hit);
                }
            }
        }

        void Damage(Collider2D other)
        {
            IHit hit = other.GetComponent<IHit>();
            if (hit != null)
            {
                int damage = isBossCollider ? GetComponentInParent<FinalBoss>().attackDamage : GetComponentInParent<Player>().damageModifier;
                hit.OnHit(damage, transform.parent.gameObject);
                //Only allow to hit one unit during an attack.
                AllowHit(false);
            }
        }

        public void AllowHit(bool canHit)
        {
            this.canHit = canHit;
        }   
    }
}