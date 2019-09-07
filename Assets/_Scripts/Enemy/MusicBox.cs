﻿using UnityEngine;

namespace TE
{
    public class MusicBox : TE.Enemy
    {
        private float timeBtwShots;
        public float startTimeBtwShots;
        public float attackRange = 10f;
        private int counter = 0;
        private float timeBtwTripleShots = 0.2f;

        public GameObject projectile;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            attackKnockback = 0;
        }

        protected override void Knockback()
        {
            // No Knockback in this enemy
            return;
            
        }

       private void LateUpdate()
        {
            float enemyDistance = Vector2.Distance(player.position, transform.position);
            if(enemyDistance > attackRange) return;
            if (timeBtwShots <= 0)
            {
                Instantiate(projectile, transform.position, Quaternion.identity);
                counter++;
                timeBtwShots = counter % 3 == 0 ? startTimeBtwShots : timeBtwTripleShots;
            }
            else
            {
                timeBtwShots -= Time.deltaTime * Game.instance.worldTimeScale; // TODO: update it with world time
            }
        }
    }
}


