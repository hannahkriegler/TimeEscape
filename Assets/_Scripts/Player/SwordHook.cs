using UnityEngine;

namespace TE
{
    /// <summary>
    /// Attaches to Players Sword
    /// </summary>
    public class SwordHook : MonoBehaviour
    {
        bool canHit;
        public Collider2D col;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!canHit)
                return;

            IHit hit = other.GetComponent<IHit>();
            if (hit != null)
            {
                hit.OnHit(5);
                AllowHit(false);
            }
        }

        public void AllowHit(bool canHit)
        {
            col.enabled = canHit;
            this.canHit = canHit;
        }
    }
}