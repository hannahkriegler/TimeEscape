using UnityEngine;

namespace TE
{
    public class Movement
    {
        private Player _player;
        private Game _game;

        public bool grounded { get; private set; }
        public bool facingRight { get; private set; } = true;

        float readyToJump;

        public bool higherJump;
        const float jumpWindow = 0.2f;

        const float dashCD = 0.3f;

        float dashCDTimer;
        float dashActiveTimer = 1;
        float curDashSpeed;
        Vector2 dashDir;

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
            UpdateReadyToJump(delta);

            if (grounded)
            {
                //Prevents spamming dash on ground
                if(dashCDTimer < dashCD)
                 dashCDTimer += delta;
            }
            else
            {
                //Allows to dash immediatly after jumping on ground
                if (dashCDTimer > 0.02f)
                    dashCDTimer = dashCD;
            }

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
                float multiplier = higherJump ? 0.05f: 7.5f;
                rb.velocity += Vector2.up * Physics2D.gravity.y * multiplier * delta;
            }
        }

        public void Move(Vector2 direction)
        {
            float delta = _player.fixedDelta;
            Rigidbody2D rb = _player.rigidBody;
            float h = direction.x;
            if (h > 0 && !facingRight)
                FlipCharacter();
            else if (h < 0 && facingRight)
                FlipCharacter();

            if (Mathf.Abs(h) < 0.01)
                h = 0;

            _player.animator.SetFloat("MoveSpeed", direction.magnitude);

            float modifier = grounded ? 1.0f : 1.2f;

            float dashModifier = 0;

            if(dashActiveTimer < 0.5f)
            {
                curDashSpeed = Mathf.Lerp(_player.dashVelocity, 0, dashActiveTimer * 2);
                dashModifier = dashDir.x * curDashSpeed;
                dashActiveTimer += delta;
                if (!_player.trailRenderer.enabled)
                {
                    _player.trailRenderer.enabled = true;
                    _player.trailRenderer.Clear();
                }
            }
            else
            {
                _player.trailRenderer.enabled = false;
            }

            rb.velocity = new Vector2(h * _player.moveSpeed * delta * modifier + dashModifier * delta, rb.velocity.y);

    
        }

        public bool Jump()
        {
            //Short Window for Jump to improve responsiveness
            if (readyToJump < jumpWindow)
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
                if(dashCDTimer >= dashCD)
                {
                    dashDir = facingRight ? Vector2.right : Vector2.left;
                    dashActiveTimer = 0;
                    dashCDTimer = 0;
                }
            }
        }

        void FlipCharacter()
        {
            facingRight = !facingRight;
            Vector3 playerScale = _player.transform.localScale;
            playerScale.x *= -1;
            _player.transform.localScale = playerScale;
        }

        void UpdateGrounded()
        {
            grounded = false;

            for (int i = 0; i < 12; i++)
            {
                Vector2 rayOrigin = _player.groundCheck.position;
                rayOrigin += Vector2.right * (i - 6) * 0.035f;
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 0.1f, _player.groundLayerCheck);
              
                if (hit)
                {
                    Debug.DrawRay(rayOrigin, Vector2.down * 0.1f, Color.blue);
                    grounded = true;
                    break;
                }
            }
        }

        void UpdateReadyToJump(float delta)
        {
            if (grounded)
            {
                readyToJump = 0;
                return;
            }

            if (readyToJump < jumpWindow)
                readyToJump += delta;

            //Addtional raycast with furter distance to let jumping feel more responsive
            RaycastHit2D hit = Physics2D.Raycast(_player.groundCheck.position, Vector2.down, 0.15f, _player.groundLayerCheck);
            if (hit)
            {
                readyToJump = 0;
            }
        }
    }
}