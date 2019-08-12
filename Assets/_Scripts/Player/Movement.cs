using UnityEngine;

namespace TE
{
    public class Movement
    {
        private Player _player;
        private Game _game;
        

        public bool grounded { get; private set; }
        public bool facingRight { get; private set; } = true;

        public bool jump;

        public Movement(Player player, Game game)
        {
            _player = player;
            _game = game;
        }

        public void Tick()
        {
            float delta = _player.fixedDelta;

            grounded = CheckGrounded(0, -1.2f) || CheckGrounded(0.2f, -1.2f) || CheckGrounded(-0.2f, -1.2f);
            Rigidbody2D rb = _player.rigidBody;

            if (!grounded)
            {
                rb.velocity += Vector2.up * delta * Physics2D.gravity * _player.gravityMultiplier;
            }
            if(rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (_player.fallMultiplier - 1) * delta;
            }
            else if(rb.velocity.y > 0 && !jump)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (_player.lowJumpMultiplier - 1) * delta;
            }
        }

        public void Move(Vector2 direction)
        {
            float delta = _player.fixedDelta;
            Rigidbody2D rb = _player.rigidBody;
            float h = direction.x;

            //TODO Move Animation

            _player.animator.SetFloat("MoveSpeed", direction.magnitude);
            
            if(direction.magnitude < 0.1f)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            if (h * rb.velocity.x < _player.maxSpeed)
                rb.AddForce(h * _player.moveSpeed * Vector2.right);

            if (Mathf.Abs (rb.velocity.x) > _player.maxSpeed)
                rb.velocity = new Vector2(Mathf.Sign (rb.velocity.x) * _player.maxSpeed, rb.velocity.y);
            
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
                _player.rigidBody.velocity = Vector2.up * _player.jumpVelocity;
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

        bool CheckGrounded(float rightOffset, float upOffset)
        {
            return Physics2D.Linecast(_player.transform.position + Vector3.right * rightOffset, 
                _player.transform.position + Vector3.up * upOffset + Vector3.right * rightOffset,
                _player.groundLayerCheck);
        }
    }
}