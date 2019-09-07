using UnityEngine;

namespace TE
{
    public class MusicBox : TE.Enemy
    {
        private float timeBtwShots;
        public float startTimeBtwShots;

        public GameObject projectile;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            
        }

        protected override void Knockback()
        {
            // No Knockback in this enemy
            return;
            
        }

        private void LateUpdate()
        {
            if (timeBtwShots <= 0)
            {
                Instantiate(projectile, transform.position, Quaternion.identity);
                timeBtwShots = startTimeBtwShots;
            }
            else
            {

                timeBtwShots -= Game.instance.worldTimeScale; // TODO: update it with world time
            }
        }
    }
}



