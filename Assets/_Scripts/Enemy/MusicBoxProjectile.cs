using TE;
using UnityEngine;

namespace TE
{
    public class MusicBoxProjectile : MonoBehaviour
    {
        public float speed;

        private Vector2 moveVector;
        private Vector2 target;

        private Player _player;
        public int damageAmount = 1;

        Rigidbody2D rigid;
        Collider2D col;

        bool follow;
        bool right;
        bool toClose;

        private void Start()
        {
            _player = Game.instance.player;
            rigid = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();

            right = _player.transform.position.x > transform.position.x;

            timer = 0;
            follow = false;
        }

        float timer;
        private void Update()
        {
            if (timer > 0.2f && !follow)
            {
                moveVector = (_player.transform.position - transform.position + Vector3.up * 
                    Random.Range(-0.1f, 0.1f));
                moveVector.Normalize();
                follow = true;
                if (Vector2.Distance(_player.transform.position, transform.position) < 0.8f)
                {
                    toClose = true;
                }
            }
            else if (timer <= 0.2f)
            {
                timer += Time.deltaTime * Game.instance.worldTimeScale;
            }

            if (follow && !toClose)
            {
                transform.position += (Vector3) moveVector * speed  * Time.deltaTime *
                   Game.instance.worldTimeScale;
            }
            else
            {
                Vector2 dir = right ? Vector2.right : Vector2.left;
                transform.position += (Vector3)dir * speed  * Time.deltaTime *
               Game.instance.worldTimeScale;
            }
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                IHit hit = other.gameObject.GetComponent<IHit>();
                hit.OnHit(damageAmount, gameObject);
                Destroy(gameObject);
            }
        }

        float destroyTick = 0;
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Grid"))
            {
                destroyTick += Time.deltaTime;
                if(destroyTick > 0.1f)
                 Destroy(gameObject);
            }
        }
    }
}
