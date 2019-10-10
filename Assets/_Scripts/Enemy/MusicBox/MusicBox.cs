using UnityEngine;

namespace TE
{
    public class MusicBox : Enemy
    {
        private float timeBtwShots;
        public float startTimeBtwShots;
        public float attackRange = 10f;
        private int counter = 0;
        private float timeBtwTripleShots = 0.2f;

        public GameObject projectile;

        protected override void Setup()
        {
            //attackKnockback = 0.5f;
        }

       protected override void Tick()
        {
            float enemyDistance = Vector2.Distance(player.transform.position, transform.position);
            if (enemyDistance > attackRange)
            {
                AttackAnim(false);
                return;
            }
            AttackAnim(true);
            if (timeBtwShots <= 0)
            {
                GameObject note = Instantiate(projectile, transform.position + Vector3.up * 0.2f, 
                    Quaternion.identity);
                MusicBoxProjectile proj = note.GetComponent<MusicBoxProjectile>();
                proj.AssignShooter(gameObject);
                counter++;
                timeBtwShots = counter % 3 == 0 ? startTimeBtwShots : timeBtwTripleShots;
            }
            else
            {
                timeBtwShots -= Time.deltaTime * Game.instance.worldTimeScale; 
            }
        }
    }
}



