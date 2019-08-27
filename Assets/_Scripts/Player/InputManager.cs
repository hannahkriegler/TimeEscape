using System;
using UnityEngine;

namespace TE
{
    public class InputManager : MonoBehaviour
    {
        private Game game;

        [Header("References")] public Player player;

        //Inputs
        private bool active_skill;
        private bool attack;
        private bool jump;
        private bool timeStamp;
        private bool timeSkill1, timeSkill2, timeSkill3, timeSkill4;
        private bool dash;
        private bool pause;
        private Vector2 movement;

        private bool dashCheck;

        float jumpTimer;
        bool didJump;

        public void Init(Game game)
        {
            this.game = game;
            player.Init(game);
            dashCheck = false;
        }

        private void FixedUpdate()
        {
            UpdateInputs();
            UpdatePlayer();
        }

        void UpdateInputs()
        {
            movement.x = Input.GetAxis("Horizontal");
            movement.y = Input.GetAxis("Vertical");

            attack = Input.GetButtonDown("Attack");
            active_skill = Input.GetButtonDown("Skill");

            //Jump Handling
            jump = Input.GetButtonDown("Jump");

            if(jump)
            {
                jumpTimer = 0;
                didJump = false;
            }
            bool jumpBuffer = Input.GetButton("Jump");
            if(jumpBuffer && ! didJump)
            {
                jumpTimer += Time.deltaTime;
                if (jumpTimer < 0.2f)
                    jump = true;
            }

            timeStamp = Input.GetButtonDown("TimeStamp");

            bool controllerDash = Input.GetAxis("Dash") > 0.5f;
            bool keyDash = Input.GetButtonDown("Dash");
            dash = controllerDash || keyDash;

            if (dashCheck)
            {
                if (!controllerDash && !keyDash)
                    dashCheck = false;
            }

            //TODO Timeskills
        }

        void UpdatePlayer()
        {
            //Movement
            player.Movement.Tick();
            player.Movement.Move(movement);

            if (jump)
            {
                didJump = player.Movement.Jump();
            }


            if (dash && !dashCheck)
            {
                dash = false;
                player.Movement.Dash();
                dashCheck = true;
            }


            //Attack Handling
            if (attack)
            {
                player.CombatMelee.Attack();
            }

            //Skills
            if (active_skill)
            {
                active_skill = false;
                player.CombatSkill.ActivateActiveSkill();
            }
        }
    }
}