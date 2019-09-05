using UnityEngine;

namespace TE
{
    /// <summary>
    /// Attaches to Players Sword
    /// </summary>
    public class DamageCollider : MonoBehaviour
    {
        public Collider2D col;

        private void OnTriggerEnter2D(Collider2D other)
        {
            IHit hit = other.GetComponent<IHit>();
            if (hit != null)
            {
                hit.OnHit(this.GetComponentInParent<Player>().damage);
                AllowHit(false);
            }
        }

        public void AllowHit(bool canHit)
        {
            col.enabled = canHit;
        }
    }
}