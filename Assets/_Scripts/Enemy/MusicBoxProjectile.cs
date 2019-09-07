using TE;
using UnityEngine;

namespace TE
{
    public class MusicBoxProjectile : TE.Enemy
    {
        public float speed;

        private Vector3 moveVector;
        private Vector2 target;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            var position = player.position;

            moveVector = (position - transform.position).normalized;
        }

        private void LateUpdate()
        {
            transform.position += moveVector * speed * Time.deltaTime;
        } 

        public override void OnHit(int damage)
        {
            Debug.Log("You can't destroy a projectile");
        }

        protected override void Attack(GameObject target)
        {
            
            if (target.CompareTag("Player"))
            {
                IHit hit = target.GetComponent<IHit>();
                hit.OnHit(damageAmount);
                Die();
            }
            if(target.CompareTag("Grid")) Die();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.gameObject.CompareTag("Grid"))
            {
                Die();
            }
        }

        protected override void Die()
        {
            Destroy(gameObject);
        }
    }
}
