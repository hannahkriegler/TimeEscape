using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TE
{
    public class FireBall : MonoBehaviour
    {
       Rigidbody2D rigid;

        private int _damage = 1;

        //Whether its the fireball of the player or an enemy.
        bool playerFireBall;

        /// <summary>
        /// Spawns the fireball.
        /// </summary>
        /// <param name="damage">Damage of the fireball on hit.</param>
        /// <param name="dir">Direction the Fireball moves</param>
        /// <param name="player">Whether the owner of the fireball is the player</param>
        /// <param name="speed">Speed of the fireball</param>
        public void Shoot(int damage, Vector2 dir, bool player = true, float speed = 15)
        {
            _damage = damage;
            playerFireBall = player;
            //Destroy the fireball after 4 seconds to prevent endless flying
            Destroy(gameObject, 4.0f);
            rigid = GetComponent<Rigidbody2D>();
            if (dir.x < 0)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            rigid.AddForce(dir * speed, ForceMode2D.Impulse);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IHit hit = collision.GetComponent<IHit>();
            if (hit != null)
            {
                //Ignore owners collision
                if (playerFireBall && collision.transform.name == "Player_TrueMe")
                    return;

                if (!playerFireBall && collision.transform.name == "FinalBoss")
                    return;

                hit.OnHit(_damage, collision.gameObject);
                //Destroy fireball on hit
                Destroy(gameObject);
            }
        }
    }
}
