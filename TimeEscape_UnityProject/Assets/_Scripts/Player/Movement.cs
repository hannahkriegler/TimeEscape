using UnityEngine;

namespace TE
{
    /// <summary>
    /// Handles Movement, Jump, Dash, Teleport and OnGround Check
    /// </summary>
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

        bool jump1;
        bool jump2;

        bool knockBack;
        float knockBackTimer;

        public Movement(Player player, Game game)
        {
            _player = player;
            _game = game;
            Physics2D.queriesStartInColliders = false;
        }

        public void Tick()
        {
            float delta = _player.fixedDelta;

            bool prevGrounded = grounded;

            UpdateGrounded();

            if (grounded && prevGrounded == false)
                SoundManager.instance.PlayLand();

            UpdateReadyToJump(delta);

            if (grounded)
            {
                //Prevents spamming dash on ground
                if(dashCDTimer < dashCD)
                 dashCDTimer += delta;

                //Reset Jumps
                jump1 = false;
                jump2 = false;
            }
            else
            {
                //Allows to dash immediatly after jumping on ground
                if (dashCDTimer > 0.02f)
                    dashCDTimer = dashCD;
            }

            Rigidbody2D rb = _player.rigidBody;

            //Optimized Jump Behavior for a smoother jump movememnt curve.
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

        /// <summary>
        /// Moves the character according to iput move direction. Additionaly handels movement during dash and jump.
        /// </summary>
        /// <param name="direction">Movement Input of the player</param>
        public void Move(Vector2 direction)
        {
            float delta = _player.fixedDelta;
            //Handle Knockback
            if(knockBack)
            {
                if (knockBackTimer < 0.5f)
                {
                    knockBackTimer += delta;
                    return;
                }
                else
                    knockBack = false;
            }
          
            Rigidbody2D rb = _player.rigidBody;
            float h = direction.x;
            if (h > 0 && !facingRight)
                FlipCharacter();
            else if (h < 0 && facingRight)
                FlipCharacter();

            if (Mathf.Abs(h) < 0.1)
            {
                h = 0;
            }

            _player.animator.SetFloat("MoveSpeed", Mathf.Abs(h));

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

        /// <summary>
        /// Checks if the character and jump and than execute the jump action.
        /// </summary>
        /// <returns>Whether the character actually jumped</returns>
        public bool Jump()
        {
            //Short Window for Jump to improve responsiveness
            if (readyToJump < jumpWindow && !jump1)
            {
                _player.rigidBody.velocity = Vector2.up * _player.jumpVelocity;
                jump1 = true;
                if(_player.hasSword)
                    _player.animator.Play("Jump");
                else
                    _player.animator.Play("Jump_NoSword");
                _player.CombatMelee.ResetAttackState();
                SoundManager.instance.PlayJump();
                return true;
            }

            //Handle Double Jump
            if(jump1 && _game.session.IsDoubleJumpUnlocked() && !jump2)
            {
                _player.rigidBody.velocity = Vector2.up * _player.jumpVelocity;
                jump2 = true;
                if (_player.hasSword)
                    _player.animator.Play("Jump");
                else
                    _player.animator.Play("Jump_NoSword");
                _player.CombatMelee.ResetAttackState();
                SoundManager.instance.PlayJump();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks whether can dash and if it holds true prepare character for dash.
        /// </summary>
        public void Dash()
        {
            if (_game.session.IsDashUnlocked())
            {
                if(dashCDTimer >= dashCD)
                {
                    dashDir = facingRight ? Vector2.right : Vector2.left;
                    dashActiveTimer = 0;
                    dashCDTimer = 0;
                    _player.animator.Play("Dash");
                    SoundManager.instance.PlayDash();
                    _player.CombatMelee.ResetAttackState();
                }
            }
        }

        /// <summary>
        ///   Flips the character sprite by multiplying local x scale with -1. Additionally Rotates the buttomPrompts to prevent the text being flipped
        /// </summary>
        void FlipCharacter()
        {
            facingRight = !facingRight;
            Vector3 playerScale = _player.transform.localScale;
            playerScale.x *= -1;
            _player.transform.localScale = playerScale;

            _player.buttomPrompt.transform.localScale = new Vector2(_player.buttomPrompt.transform.localScale.x * -1 , 0.01f);
            _player.stopWatch.transform.localScale = new Vector2(_player.stopWatch.transform.localScale.x * -1, 0.01f);
        }

        public Vector2 GetForwardDir()
        {
            return facingRight? Vector2.right: Vector2.left;
        }

        /// <summary>
        /// Checks wether the Player is currently on Ground. Updates the state in value grounded.
        /// </summary>
        void UpdateGrounded()
        {
            grounded = false;

            for (int i = 0; i < 10; i++)
            {
                Vector2 rayOrigin = _player.groundCheck.position;
                rayOrigin += Vector2.right * (i - 5) * 0.025f;
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, 0.1f, _player.groundLayerCheck);

                if (hit)
                {
                    Debug.DrawRay(rayOrigin, Vector2.down * 0.1f, Color.red);
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
        }

        /// <summary>
        /// Teleports during another position without regarding physics. (For Timetravel)
        /// </summary>
        /// <param name="teleportPos"></param>
        public void Teleport(Vector2 teleportPos)
        {
            _player.rigidBody.isKinematic = true;
            _player.transform.position = teleportPos;
            _player.rigidBody.velocity = Vector2.zero;
            _player.rigidBody.isKinematic = false;
        }

        /// <summary>
        /// Moves the character away from the attacker. Prevents additional movement during knockback.
        /// </summary>
        /// <param name="strength">How much distance the player gets knocked back.</param>
        /// <param name="attacker">Transform to move away from</param>
        public void KnockBack(float strength, Transform attacker)
        {
            knockBack = true;
            knockBackTimer = 0;
            _player.rigidBody.velocity = Vector2.zero;
            Vector3 dir = _player.transform.position - attacker.transform.position;      
            dir.Normalize();
            if (dir.x < 0)
                dir -= Vector3.right * 0.2f;
            else
                dir += Vector3.right * 0.2f;
            _player.rigidBody.AddForce(strength * dir, ForceMode2D.Force);
        }
    }
}