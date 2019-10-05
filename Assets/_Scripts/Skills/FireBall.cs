using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TE
{
    public class FireBall : MonoBehaviour
    {
       Rigidbody2D rigid;

        private int _damage = 1;
        public float speed = 500;
        bool playerFireBall;

        public void Shoot(int damage, Vector2 dir, bool player = true)
        {
            _damage = damage;
            playerFireBall = player;
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
                if (playerFireBall && collision.transform.name == "Player_TrueMe")
                    return;

                hit.OnHit(_damage, collision.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
