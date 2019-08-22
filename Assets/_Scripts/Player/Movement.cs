using UnityEngine;

namespace TE
{
    public class Movement
    {
        private Player _player;
        private Game _game;

        public bool grounded { get; private set; }
        public bool facingRight { get; private set; } = true;

        bool readyToJump;

        public Movement(Player player, Game game)
        {
            _player = player;
            _game = game;
             Physics2D.queriesStartInColliders = false;
        }

        public void Tick()
        {
            float delta = _player.fixedDelta;
            
            UpdateGrounded();
            Rigidbody2D rb = _player.rigidBody;

            if (!grounded)
            {
                rb.velocity += Vector2.up * delta * Physics2D.gravity * _player.gravityMultiplier;
            }

            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (_player.fallMultiplier - 1) * delta;
            }
            else if (rb.velocity.y > 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (_player.lowJumpMultiplier - 1) * delta;
            }
        }

        public void Move(Vector2 direction)
        {
            float delta = _player.fixedDelta;
            Rigidbody2D rb = _player.rigidBody;
            float h = direction.x;  

            _player.animator.SetFloat("MoveSpeed", direction.magnitude);

            float modifier = grounded ? 1.0f : 1.2f;

            rb.velocity = new Vector2(h * _player.moveSpeed * delta * modifier, rb.velocity.y);

            if (h > 0 && !facingRight)
                FlipCharacter();
            else if (h < 0 && facingRight)
                FlipCharacter();
        }

        public bool Jump()
        {
            if (grounded)
            {
                //TODO Trigger Jump Animation
                _player.rigidBody.velocity = Vector2.up * _player.jumpVelocity;
                return true;
            }
            return false;
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

        void UpdateGrounded()
        {
            grounded = false;
            readyToJump = false;

            for (int i = 0; i < 24; i++)
            {
                Vector2 rayOrigin = _player.groundCheck.position;
                rayOrigin += Vector2.right * (i - 12) * 0.05f;
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 0.15f, _player.groundLayerCheck);

                if (hit)
                {
                    Debug.DrawRay(rayOrigin, Vector2.down * 0.3f, Color.blue);
                    grounded = true;
                    readyToJump = true;
                    break;
                }

                if (readyToJump)
                    continue;

                RaycastHit2D hit2 = Physics2D.Raycast(rayOrigin, Vector2.down, 0.5f, _player.groundLayerCheck);

                if (hit2)
                {
                    Debug.DrawRay(rayOrigin, Vector2.down * 0.3f, Color.blue);
                    readyToJump = true;
                }
            }
        }
    }
}