using UnityEngine;

namespace TE
{
    public class Movement
    {
        private Player _player;
        private Game _game;
        

        public bool grounded { get; private set; }
        public bool facingRight { get; private set; } = true;

        public Movement(Player player, Game game)
        {
            _player = player;
            _game = game;
        }

        public void Move(Vector2 direction)
        {
            float delta = _player.fixedDelta;
            Rigidbody2D rigid = _player.rigidBody;
            float h = direction.x;
            
            //TODO Move Animation
            
            grounded = Physics2D.Linecast(_player.transform.position, _player.groundCheck.position, _player.groundLayerCheck);
            
            if (h * rigid.velocity.x < _player.maxSpeed)
                rigid.AddForce(h * _player.moveSpeed * Vector2.right);

            if (Mathf.Abs (rigid.velocity.x) > _player.maxSpeed)
                rigid.velocity = new Vector2(Mathf.Sign (rigid.velocity.x) * _player.maxSpeed, rigid.velocity.y);
            
            if (h > 0 && !facingRight)
                FlipCharacter ();
            else if (h < 0 && facingRight)
                FlipCharacter ();
        }

        public void Jump()
        {
            if (grounded)
            {
                //TODO Trigger Jump Animation
                _player.rigidBody.AddForce(new Vector2(0f, _player.jumpForce));
            }
        }

        public void Dash()
        {
            if (_game.session.IsDashUnlocked())
            {
                Debug.Log("Player dashed!");
            }
        }

        void FlipCharacter()
        {
            facingRight = !facingRight;
            Vector3 theScale = _player.transform.localScale;
            theScale.x *= -1;
            _player.transform.localScale = theScale;
        }
    }
}