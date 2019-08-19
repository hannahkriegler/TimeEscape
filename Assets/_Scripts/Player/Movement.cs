using UnityEngine;

namespace TE
{
    public class Movement
    {
        private Player _player;
        private Game _game;

        public bool grounded { get; private set; }
        public bool facingRight { get; private set; } = true;

        //Settings
        private float raySpacing = 0.05f;
        private float skinWidth = 0.01f;

        //Private Values
        private float _horizontalRaySpacing;
        private float _horizontalRayCount;
        private float _verticalRaySpacing;
        private float _verticalRayCount;

        private bool readyToJump;

       
        private RaycastOrigins _raycastOrigins;

        protected struct RaycastOrigins
        {
            public Vector2 topLeft, topRight, bottomLeft, bottomRight;
        }

        public Movement(Player player, Game game)
        {
            _player = player;
            _game = game;
             Physics2D.queriesStartInColliders = false;
            CalculateSpacing();
        }

        public void Tick()
        {
            float delta = _player.fixedDelta;
            
            UpdateRaycastOrigins();
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
            if (readyToJump)
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

            for (int i = 0; i < _verticalRayCount; i++)
            {
                Vector2 rayOrigin = facingRight ? _raycastOrigins.bottomLeft : _raycastOrigins.bottomRight;
                rayOrigin += (facingRight ? Vector2.right : Vector2.left) * (_verticalRaySpacing * i);
                rayOrigin.y += skinWidth * 2f;
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down,
                    skinWidth * 5f, _player.groundLayerCheck);

                if (hit)
                {
                    Debug.DrawRay(rayOrigin, Vector2.down * skinWidth * 2, Color.blue);
                    grounded = true;
                    readyToJump = true;
                    break;
                }

                if (!readyToJump)
                {
                    RaycastHit2D hit2 = Physics2D.Raycast(rayOrigin, Vector2.down * 1.5f,
                      skinWidth * 5f, _player.groundLayerCheck);
                    if(hit2)
                    {
                        readyToJump = true;
                    }
                }
            }
        }
        
        #region Calculations

        void CalculateSpacing()
        {
            Bounds bounds = _player.col.bounds;
            bounds.Expand(skinWidth * -2);
            _horizontalRayCount = Mathf.Round(bounds.size.y / raySpacing);
            _verticalRayCount = Mathf.Round(bounds.size.x / raySpacing);
            _horizontalRaySpacing = bounds.size.y / (_horizontalRayCount - 1);
            _verticalRaySpacing = bounds.size.x / (_verticalRayCount - 1);
        }

        void UpdateRaycastOrigins()
        {
            Bounds bounds = _player.col.bounds;
            bounds.Expand(skinWidth * -2);   
            _raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            _raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
            _raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
            _raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
        }
        
        #endregion
    }
}